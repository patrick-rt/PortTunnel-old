/*
MIT License
Copyright (c) 2019 Patrick72762
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace PortTunnelLib
{
    class Tunnel
    {
        private TcpClient client1, client2;
        private EndPoint ep1, ep2;
        private PortTunnel main;
        private int errorCounter = 0;

        private byte[] httpHostReplacement;
        private byte[] httpHostHeaderLine;

        private byte tmpByte;
        private bool replacementRequired;

        public void Start(TcpClient client1, TcpClient client2,
            PortTunnel main)
        {
            this.client1 = client1;
            this.client2 = client2;
            ep1 = client1.Client.RemoteEndPoint;
            ep2 = client2.Client.RemoteEndPoint;
            this.main = main;

            main.ActiveConnections++;

            if (main.HttpProtocol)
            {
                if (main.Connection1.Host != null && main.Connection2.Host != null)
                {
                    httpHostReplacement = new byte[main.Connection1.Host.Length];

                    int i = 0;
                    foreach (char c in main.Connection1.Host.ToCharArray())
                    {
                        httpHostReplacement[i] = (byte)c;
                        i++;
                    }
                }
                else if (main.Connection1.Host != null && main.Connection2.Host == null)
                {
                    httpHostReplacement = new byte[main.Connection1.Host.Length];

                    int i = 0;
                    foreach (char c in main.Connection1.Host.ToCharArray())
                    {
                        httpHostReplacement[i] = (byte)c;
                        i++;
                    }
                }
            }

            httpHostHeaderLine = new byte[6] {
                (byte)'H',
                (byte)'o',
                (byte)'s',
                (byte)'t',
                (byte)':',
                (byte)' ' };

            if (main.DestIsSsl && 
                ((main.Connection1.Type == Enums.ConnectionType.Client && 
                main.Connection2.Type == Enums.ConnectionType.Client) ||
                (main.Connection1.Type == Enums.ConnectionType.Client && 
                main.Connection2.Type == Enums.ConnectionType.Server)))
            {
                ThreadPool.QueueUserWorkItem(ThreadLoopSsl);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(ThreadLoop);
            }

            Log.Write(Enums.LogType.Info, "Tunnel started");
        }

        private void ThreadLoop(object obj)
        {
            long lastData = DateTime.Now.Ticks / 10000;
            long tmp;

            while (main.Status == 1)
            {
                tmp = DateTime.Now.Ticks / 10000 - lastData;
                if (tmp >= main.Timeout)
                {
                    main.ActiveConnections--;
                    Log.Write(Enums.LogType.Info, "Connection timed out");
                    StreamLogger.EndLogging(ep1, ep2);
                    StreamLogger.EndLogging(ep2, ep1);
                    break;
                }

                try
                {
                    if (client1.Available > 0)  //client1 -> client2
                    {
                        lastData = DateTime.Now.Ticks / 10000;
                        tmpByte = (byte)client1.GetStream().ReadByte();
                        StreamLogger.Write(ep1, ep2, tmpByte);

                        if (tmpByte != httpHostHeaderLine[0] || !main.HttpProtocol)
                        {
                            client2.GetStream().WriteByte(tmpByte);
                        }
                        else
                        {
                            ChangeHttpHost(client1.GetStream(), client2.GetStream());
                        }
                    }

                    if (client2.Available > 0)  //client1 <- client2
                    {
                        tmpByte = (byte)client2.GetStream().ReadByte();
                        StreamLogger.Write(ep2, ep1, tmpByte);

                        lastData = DateTime.Now.Ticks / 10000;
                        if (tmpByte != httpHostHeaderLine[0] || !main.HttpProtocol)
                        {
                            client1.GetStream().WriteByte(tmpByte);
                        }
                        else
                        {
                            ChangeHttpHost(client2.GetStream(), client1.GetStream());
                        }
                    }

                    Thread.Sleep(new TimeSpan(main.TicksBetweenBytes));     //warten                                            
                    errorCounter = 0;
                }
                catch
                {
                    errorCounter++;
                    if (errorCounter == 200)
                    {
                        Log.Write(Enums.LogType.Warning, "Connection closed");
                        StreamLogger.EndLogging(ep1, ep2);
                        StreamLogger.EndLogging(ep2, ep1);

                        client1.Close();
                        client2.Close();

                        main.ActiveConnections--;

                        break;
                    }
                }
            }
        }

        private void ThreadLoopSsl(object obj)
        {
            SslStream ssl;

            ssl = new SslStream(client2.GetStream(), true, null, null, EncryptionPolicy.AllowNoEncryption);
            Log.Write(Enums.LogType.Info, "Encrypted: " + ssl.IsEncrypted);
            ssl.AuthenticateAsClient(main.Connection1.Host, null, System.Security.Authentication.SslProtocols.Tls12, false);
            Log.Write(Enums.LogType.Info, "Encrypted (after Auth): " + ssl.IsEncrypted);

            long lastData = DateTime.Now.Ticks / 10000;
            long tmp;

            while (main.Status == 1)
            {
                tmp = DateTime.Now.Ticks / 10000 - lastData;
                if (tmp >= main.Timeout)
                {
                    main.ActiveConnections--;
                    Log.Write(Enums.LogType.Info, "Connection timed out");
                    break;
                }

                try
                {
                    if (client1.Available > 0)  //client1 -> client2
                    {
                        lastData = DateTime.Now.Ticks / 10000;
                        tmpByte = (byte)client1.GetStream().ReadByte();
                        StreamLogger.Write(ep1, ep2, tmpByte);

                        if (tmpByte != httpHostHeaderLine[0] || !main.HttpProtocol)
                        {
                            ssl.WriteByte(tmpByte);
                        }
                        else
                        {
                            ChangeHttpHost(client1.GetStream(), ssl);
                        }
                    }

                    if (client2.Available > 0)  //client1 <- client2
                    {
                        tmpByte = (byte)ssl.ReadByte();
                        lastData = DateTime.Now.Ticks / 10000;

                        if (tmpByte != httpHostHeaderLine[0] || !main.HttpProtocol)
                        {
                            client1.GetStream().WriteByte(tmpByte);
                        }
                        else
                        {
                            ChangeHttpHost(ssl, client1.GetStream());
                        }
                    }

                    Thread.Sleep(new TimeSpan(main.TicksBetweenBytes));
                    errorCounter = 0;
                }
                catch (Exception e)
                {
                    Log.Write(Enums.LogType.Error, e.Message);

                    errorCounter++;
                    if (errorCounter == 200)
                    {
                        Log.Write(Enums.LogType.Warning, "Connection closed");
                        ssl.Close();
                        client1.Close();
                        client2.Close();

                        main.ActiveConnections--;

                        break;
                    }
                }
            }
        }

        private void ChangeHttpHost(Stream source, Stream dest)
        {
            try
            {
                replacementRequired = true;

                dest.WriteByte(tmpByte);

                int i;
                for (i = 1; i < httpHostHeaderLine.Length; i++)
                {
                    tmpByte = (byte)source.ReadByte();

                    StreamLogger.Write(ep1, ep2, tmpByte);

                    dest.WriteByte(tmpByte);

                    if (tmpByte != httpHostHeaderLine[i])
                    {
                        replacementRequired = false;
                        break;
                    }
                }

                if (replacementRequired)
                {
                    replacementRequired = false;

                    Log.Write(Enums.LogType.Info, "Host detected! Replacing...");
                    string tmpStr = "";

                    do
                    {
                        tmpByte = (byte)source.ReadByte();
                        tmpStr += (char)tmpByte;
                    } while (tmpByte != (byte)'\n');

                    Log.Write(Enums.LogType.Info, "Host received: " + tmpStr
                        .Replace('\r', ' ')
                        .Replace('\n', ' '));

                    tmpStr = "";

                    for (i = 0; i < httpHostReplacement.Length; i++)
                    {
                        tmpStr += (char)httpHostReplacement[i];
                        StreamLogger.Write(ep1, ep2, httpHostReplacement[i]);

                        dest.WriteByte(httpHostReplacement[i]);
                    }

                    Log.Write(Enums.LogType.Info, "Replaced with: " + tmpStr);

                    dest.WriteByte((byte)'\r');
                    StreamLogger.Write(ep1, ep2, (byte)'\r');

                    dest.WriteByte((byte)'\n');
                    StreamLogger.Write(ep1, ep2, (byte)'\n');
                }
            }
            catch { }
        }
    }
}
