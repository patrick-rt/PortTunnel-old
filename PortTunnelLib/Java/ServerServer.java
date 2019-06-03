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

import java.net.ServerSocket;
import java.net.Socket;

public class ServerServer extends Thread{
    PortTunnel main;

    public ServerServer(PortTunnel main){
        this.main = main;
    }

    public void run() {
        Log.Write(LogType.Info, "Starting ServerServer-Mode ...");
        Loop();
    }

    private void Loop()
    {
        try
        {
            ServerSocket controlListener = new ServerSocket(main.ControlPort);

            Socket controlClient =
                    controlListener.accept();

            while (true)
            {
                if (controlClient.getInputStream().available() > 0)
                {
                    //Falsche Clients aussortieren
                    if (controlClient.getInputStream().read()
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

            ServerSocket mainListener = new ServerSocket(main.Connection1.Port);

            ServerSocket tunnelListener = new ServerSocket(main.Connection2.Port);

            while (main.Status == 1)
            {
                Socket client = mainListener.accept();

                controlClient.getOutputStream().write((byte)'n');

                Socket tunnelClient =
                        tunnelListener.accept();

                Tunnel tunnel = new Tunnel(client, tunnelClient, main);
                tunnel.start();
            }
        }
        catch (Exception e)
        {
            Log.Write(LogType.Error,
                    "Can't start PortTunnel");
        }
    }
}
