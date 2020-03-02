/*
MIT License
Copyright (c) 2019 Patrick727
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

package PortTunnelLib;

public class Connection {
    String Host;
    int Port;
    ConnectionType Type;

    public Connection(int port)
    {
        Type = ConnectionType.Server;
        Host = null;
        Port = port;
    }

    public Connection(String host, int port)
    {
        Type = ConnectionType.Client;
        Host = host;
        Port = port;
        if (Host.equals("")) {
            Type = ConnectionType.Server;
            Host = null;
        }
    }
}
