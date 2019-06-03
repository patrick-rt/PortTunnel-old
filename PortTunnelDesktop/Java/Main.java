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

package PortTunnelDesktop;

import PortTunnelLib.PortTunnel;

public class Main {

    public static void main(String[] args){
        if (args.length > 0)
        {
            PortTunnel tunnel = new PortTunnel(args);
            tunnel.Start();
        }
        else
        {
            try
            {
                Gui gui = new Gui();
            }
            catch (Exception e)
            {
                System.out.println("Please use args!");
                System.out.print(PortTunnel.GetHelp());
            }
        }

        try{
            System.in.read();
        }catch (Exception e){

        }
    }
}
