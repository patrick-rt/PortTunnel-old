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

