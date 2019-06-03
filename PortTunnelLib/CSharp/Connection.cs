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

namespace PortTunnelLib
{
    public class Connection
    {
        public string Host { get; internal set; }
        public int Port { get; private set; }
        public Enums.ConnectionType Type { get; private set; }

        public Connection(int port)
        {
            Type = Enums.ConnectionType.Server;
            Host = null;
            Port = port;
        }

        public Connection(string host, int port)
        {
            Type = Enums.ConnectionType.Client;
            Host = host;
            Port = port;
            if (Host == "")
            {
                Type = Enums.ConnectionType.Server;
                Host = null;
            }
        }
    }
}
