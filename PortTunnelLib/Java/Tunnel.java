/*
MIT License
Copyright (c) 2019 Patrick72762
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

package PortTunnelLib;

import javax.net.ssl.SSLSocketFactory;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.Date;

public class Tunnel extends Thread{
    private Socket client1, client2;
    private PortTunnel main;
    private int errorCounter = 0;

    private byte[] httpHostReplacement;
    private byte[] httpHostHeaderLine;

    private byte tmpByte;
    private boolean replacementRequired;

    public Tunnel(Socket client1, Socket client2,
                      PortTunnel main)
    {
        this.client1 = client1;
        this.client2 = client2;
        this.main = main;
    }

    public void run()
    {
        main.ActiveConnections++;

        if (main.httpProtocol)
        {
            if (main.Connection1.Host != null && main.Connection2.Host != null)
            {
                httpHostReplacement = new byte[main.Connection1.Host.length()];

                int i = 0;
                for (char c : main.Connection1.Host.toCharArray())
                {
                    httpHostReplacement[i] = (byte)c;
                    i++;
                }
            }
            else if (main.Connection1.Host != null && main.Connection2.Host == null)
            {
                httpHostReplacement = new byte[main.Connection1.Host.length()];

                int i = 0;
                for (char c : main.Connection1.Host.toCharArray())
                {
                    httpHostReplacement[i] = (byte)c;
                    i++;
                }
            }
        }

        httpHostHeaderLine = new byte[6];
        httpHostHeaderLine[0] = (byte)'H';
        httpHostHeaderLine[1] = (byte)'o';
        httpHostHeaderLine[2] = (byte)'s';
        httpHostHeaderLine[3] = (byte)'t';
        httpHostHeaderLine[4] = (byte)':';
        httpHostHeaderLine[5] = (byte)' ';

        if (main.destIsSsl &&
                ((main.Connection1.Type == ConnectionType.Client &&
                        main.Connection2.Type == ConnectionType.Client) ||
                        (main.Connection1.Type == ConnectionType.Client &&
                                main.Connection2.Type == ConnectionType.Server)))
        {
            ThreadLoopSsl();
        }
        else
        {
            ThreadLoop();
        }

        Log.Write(LogType.Info, "Tunnel started");
    }

    private void ThreadLoop()
    {
        long lastData = new Date().getTime();
        long tmp;

        while (main.Status == 1)
        {
            tmp = new Date().getTime() - lastData;
            if (tmp >= main.Timeout)
            {
                main.ActiveConnections--;
                Log.Write(LogType.Info, "Connection timed out");
                StreamLogger.EndLogging(client1, client2);
                StreamLogger.EndLogging(client2, client1);
                break;
            }

            try
            {
                if (client1.getInputStream().available() > 0)  //client1 -> client2
                {
                    lastData = new Date().getTime();
                    tmpByte = (byte)client1.getInputStream().read();
                    StreamLogger.Write(client1, client2, tmpByte);

                    if (tmpByte != httpHostHeaderLine[0] || !main.httpProtocol)
                    {
                        client2.getOutputStream().write(tmpByte);
                    }
                    else
                    {
                        ChangeHttpHost(client1.getInputStream(), client2.getOutputStream());
                    }
                }

                if (client2.getInputStream().available() > 0)  //client1 <- client2
                {
                    tmpByte = (byte)client2.getInputStream().read();
                    StreamLogger.Write(client2, client1, tmpByte);

                    lastData = new Date().getTime();
                    if (tmpByte != httpHostHeaderLine[0] || !main.httpProtocol)
                    {
                        client1.getOutputStream().write(tmpByte);
                    }
                    else
                    {
                        ChangeHttpHost(client2.getInputStream(), client1.getOutputStream());
                    }
                }

                Thread.sleep(0, main.TicksBetweenBytes);
                errorCounter = 0;
            }
            catch (Exception e)
            {
                errorCounter++;
                if (errorCounter == 200)
                {
                    Log.Write(LogType.Warning, "Connection closed");
                    StreamLogger.EndLogging(client1, client2);
                    StreamLogger.EndLogging(client2, client1);

                    try {
                        client1.close();
                        client2.close();
                    }catch (Exception ex){

                    }

                    main.ActiveConnections--;

                    break;
                }
            }
        }
    }

    private void ThreadLoopSsl()
    {
        try {
            client2.close();

            SSLSocketFactory ssf = (SSLSocketFactory)SSLSocketFactory.getDefault();
            Socket ssl = ssf.createSocket(main.Connection2.Host, main.Connection2.Port);

            long lastData = new Date().getTime();
            long tmp;

            while (main.Status == 1) {
                tmp = new Date().getTime() - lastData;
                if (tmp >= main.Timeout) {
                    main.ActiveConnections--;
                    Log.Write(LogType.Info, "Connection timed out");
                    StreamLogger.EndLogging(client1, client2);
                    StreamLogger.EndLogging(client2, client1);
                    break;
                }

                try {
                    if (client1.getInputStream().available() > 0)  //client1 -> client2
                    {
                        lastData = new Date().getTime();
                        tmpByte = (byte) client1.getInputStream().read();
                        StreamLogger.Write(client1, client2, tmpByte);

                        if (tmpByte != httpHostHeaderLine[0] || !main.httpProtocol) {
                            client2.getOutputStream().write(tmpByte);
                        } else {
                            ChangeHttpHost(client1.getInputStream(), client2.getOutputStream());
                        }
                    }

                    if (client2.getInputStream().available() > 0)  //client1 <- client2
                    {
                        tmpByte = (byte) client2.getInputStream().read();
                        StreamLogger.Write(client2, client1, tmpByte);

                        lastData = new Date().getTime();
                        if (tmpByte != httpHostHeaderLine[0] || !main.httpProtocol) {
                            client1.getOutputStream().write(tmpByte);
                        } else {
                            ChangeHttpHost(client2.getInputStream(), client1.getOutputStream());
                        }
                    }

                    Thread.sleep(0, main.TicksBetweenBytes);
                    errorCounter = 0;
                } catch (Exception e) {
                    errorCounter++;
                    if (errorCounter == 200) {
                        Log.Write(LogType.Warning, "Connection closed");
                        StreamLogger.EndLogging(client1, client2);
                        StreamLogger.EndLogging(client2, client1);

                        try {
                            client1.close();
                            client2.close();
                        } catch (Exception ex) {

                        }

                        main.ActiveConnections--;

                        break;
                    }
                }
            }
        }catch (Exception e){

        }
    }

    private void ChangeHttpHost(InputStream source, OutputStream dest)
    {
        try {
            replacementRequired = true;

            dest.write(tmpByte);

            int i;
            for (i = 1; i < httpHostHeaderLine.length; i++) {
                tmpByte = (byte) source.read();

                StreamLogger.Write(client1, client2, tmpByte);

                dest.write(tmpByte);

                if (tmpByte != httpHostHeaderLine[i]) {
                    replacementRequired = false;
                    break;
                }
            }

            if (replacementRequired) {
                replacementRequired = false;

                Log.Write(LogType.Info, "Host detected! Replacing...");
                String tmpStr = "";

                do {
                    tmpByte = (byte) source.read();
                    tmpStr += (char) tmpByte;
                } while (tmpByte != (byte) '\n');

                Log.Write(LogType.Info, "Host received: " + tmpStr
                        .replace('\r', ' ')
                        .replace('\n', ' '));

                tmpStr = "";

                for (i = 0; i < httpHostReplacement.length; i++) {
                    tmpStr += (char) httpHostReplacement[i];
                    StreamLogger.Write(client1, client2, httpHostReplacement[i]);

                    dest.write(httpHostReplacement[i]);
                }

                Log.Write(LogType.Info, "Replaced with: " + tmpStr);

                dest.write((byte) '\r');
                StreamLogger.Write(client1, client2, (byte) '\r');

                dest.write((byte) '\n');
                StreamLogger.Write(client1, client2, (byte) '\n');
            }
        }catch (Exception e){

        }
    }
}
