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

using System.Net.Sockets;
using System.Threading;

namespace PortTunnelLib
{
    class ClientClient
    {
        private PortTunnel main;

        public ClientClient(PortTunnel main)
        {
            this.main = main;
        }

        public void Run()
        {
            Log.Write(Enums.LogType.Info, "Starting ClientClient-Mode ...");
            Thread loop = new Thread(Loop);
            loop.Start();
        }

        private void Loop()
        {
            try
            {
                TcpClient controlClient = new TcpClient(
                    main.Connection2.Host, main.ControlPort);

                Log.Write(Enums.LogType.Info, "Connected to PortTunnel Server");

                Thread.Sleep(2);

                controlClient.GetStream().WriteByte((byte)'c');

                Thread.Sleep(2);

                while (main.Status == 1)
                {
                    if (controlClient.Available > 0)
                    {
                        if (controlClient.GetStream().ReadByte()
                            == (byte)'n')
                        {
                            TcpClient cl1 = new TcpClient(
                                main.Connection2.Host, main.Connection2.Port);

                            TcpClient tunnelClient = new TcpClient(
                                main.Connection1.Host, main.Connection1.Port);

                            Tunnel tunnel = new Tunnel();
                            tunnel.Start(cl1, tunnelClient, main);
                        }
                    }
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

