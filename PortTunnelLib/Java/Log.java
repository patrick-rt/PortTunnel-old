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

import java.util.Date;

public class Log {
    static PortTunnel main;

    static void Write(LogType type, String text)
    {
        Date now = new Date();
        String tmp =
                "[" + now.getHours() +
                        ":" + now.getMinutes() +
                        ":" + now.getSeconds() +
                        "] [";
        if(type.equals(LogType.Info))
            tmp += "Info";
        else if(type.equals(LogType.Warning))
            tmp += "Warning";
        else if(type.equals(LogType.Error))
            tmp += "Error";
        tmp += "] " + text + "\r\n";

        for(LogListener l : main.LogListeners)
            l.NewLogOutput(tmp);
    }
}
