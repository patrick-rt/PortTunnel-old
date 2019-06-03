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

public class ServerClient extends Thread {
    PortTunnel main;

    public ServerClient(PortTunnel main){
        this.main = main;
        main.Connection2.Host = null;
    }

    public void run() {
        Log.Write(LogType.Info, "Starting ServerClient-Mode ...");
        Loop();
    }

    private void Loop()
    {
        try
        {
            ServerSocket listener = new ServerSocket(main.Connection2.Port);

            Socket client, dest;

            while (main.Status == 1)
            {
                Log.Write(LogType.Info, "Waiting for clients ...");
                client = listener.accept();
                Log.Write(LogType.Info, "New client from " + client.getInetAddress().toString());

                dest = new Socket(main.Connection1.Host,
                        main.Connection1.Port);

                Tunnel tunnel = new Tunnel(client, dest, main);
                tunnel.start();
            }
        }
        catch (Exception e)
        {
            Log.Write(LogType.Error, "Can't start PortTunnel");
            Log.Write(LogType.Error, e.getMessage());
        }
    }
}
