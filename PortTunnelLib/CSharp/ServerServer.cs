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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PortTunnelLib
{
    class ServerServer
    {
        private PortTunnel main;

        public ServerServer(PortTunnel main)
        {
            this.main = main;
        }

        public void Run()
        {
            Log.Write(Enums.LogType.Info, "Starting ServerServer-Mode ...");
            Thread loop = new Thread(Loop);
            loop.Start();
        }

        private void Loop()
        {
            try
            {
                TcpListener controlListener = new TcpListener(
                    IPAddress.Any, main.ControlPort);

                controlListener.Start();

                TcpClient controlClient =
                    controlListener.AcceptTcpClient();

                while (true)
                {
                    if (controlClient.Available > 0)
                    {
                        if (controlClient.GetStream().ReadByte()
                            == (byte)'c')
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }

                TcpListener mainListener = new TcpListener(
                    IPAddress.Any, main.Connection1.Port);
                mainListener.Start();

                TcpListener tunnelListener = new TcpListener(
                    IPAddress.Any, main.Connection2.Port);
                tunnelListener.Start();

                while (main.Status == 1)
                {
                    TcpClient client = mainListener.AcceptTcpClient();

                    controlClient.GetStream().WriteByte((byte)'n');

                    TcpClient tunnelClient =
                        tunnelListener.AcceptTcpClient();

                    Tunnel tunnel = new Tunnel();
                    tunnel.Start(client, tunnelClient, main);
                }
            }
            catch
            {
                Log.Write(Enums.LogType.Error,
                    "Can't start PortTunnel");
            }
        }
    }
}

