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

namespace PortTunnelLib
{
    public class Log
    {
        internal static PortTunnel main;

        internal static void Write(Enums.LogType type, string text)
        {
            DateTime now = DateTime.Now;
            string tmp =
                "[" + now.Hour +
                ":" + now.Minute +
                ":" + now.Second +
                "] [" + Enum.GetName(typeof(Enums.LogType), type) +
                "] " + text + "\r\n";
            
            main.NewLogOutput.Invoke(main, tmp);
        }
    }
}
