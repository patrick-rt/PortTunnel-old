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

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PortTunnelLib
{
    class ServerClient
    {
        private PortTunnel main;

        public ServerClient(PortTunnel main)
        {
            this.main = main;
            main.Connection2.Host = null;
        }

        public void Run()
        {
            Log.Write(Enums.LogType.Info, "Starting ServerClient-Mode ...");
            Thread loop = new Thread(Loop);
            loop.Start();
        }

        private void Loop()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any,
                    main.Connection2.Port);

                listener.Start();

                TcpClient client, dest;

                while (main.Status == 1)
                {
                    Log.Write(Enums.LogType.Info, "Waiting for clients ...");
                    client = listener.AcceptTcpClient();
                    Log.Write(Enums.LogType.Info, "New client from " +
                        ((IPEndPoint)client.Client.RemoteEndPoint)
                        .Address.ToString());

                    dest = new TcpClient(main.Connection1.Host,
                        main.Connection1.Port);

                    Tunnel tunnel = new Tunnel();
                    tunnel.Start(client, dest, main);
                }
            }
            catch
            {
                Log.Write(Enums.LogType.Error, "Can't start PortTunnel");
            }
        }
    }
}

