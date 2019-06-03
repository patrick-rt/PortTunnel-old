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
using System.IO;
using System.Net;
using System.Windows.Forms;

using PortTunnelLib;

namespace PortTunnel_Desktop
{
    public partial class GUI : Form
    {
        PortTunnel pt = null;

        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!intPort1.Text.Equals("") && !intPort2.Text.Equals(""))
            {
                Connection c1 = null, c2 = null;

                try
                {
                    int p1 = int.Parse(intPort1.Text);
                    int p2 = int.Parse(intPort2.Text);

                    int cp = 0;

                    if (selectSC.Checked)
                    {
                        string h1 = txtHost1.Text;

                        c1 = new Connection(h1, p1);
                        c2 = new Connection(p2);

                        this.Text = "PortTunnel ServerClient";
                    }
                    else if (selectCC.Checked)
                    {
                        string h1 = txtHost1.Text;
                        string h2 = txtHost2.Text;

                        c1 = new Connection(h1, p1);
                        c2 = new Connection(h2, p2);

                        cp = int.Parse(intCP.Text);

                        this.Text = "PortTunnel Client";
                    }
                    else if (selectSS.Checked)
                    {
                        c1 = new Connection(p1);
                        c2 = new Connection(p2);

                        cp = int.Parse(intCP.Text);

                        this.Text = "PortTunnel Server";
                    }

                    pt = new PortTunnel(c1, c2, int.Parse(intTimeout.Text), cp);

                    pt.NewLogOutput += (sen, text) =>
                    {
                        MethodInvoker invoker = new MethodInvoker(delegate
                        {
                            txtLog.Text += text;

                            Console.Write(text);

                            txtActiveConnections.Text = ((PortTunnel)sen).ActiveConnections.ToString();
                        });
                        this.Invoke(invoker);
                    };

                    if (cbStreamLog.Checked)
                    {
                        pt.NewStreamLogCompleted += (sen, text) =>
                        {
                            string filename = ((string)text).Split('\r')[0]
                                .Replace(' ', '-').Replace(":", "-Port-") + ".txt";

                            if (!Directory.Exists("Log"))
                                Directory.CreateDirectory("Log");

                            File.WriteAllText("Log/" + filename, (string)text);
                        };
                    }

                    pt.HttpProtocol = cbHttp.Checked;
                    pt.DestIsSsl = cbSsl.Checked;

                    BlockInputs();
                    
                    pt.Start();
                }
                catch { }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                pt.Stop();
            }
            catch { }
            try
            {
                PortTunnel.ExitEnvironment();
            }
            catch { }
        }

        private void selectSC_CheckedChanged(object sender, EventArgs e)
        {
            txtHost1.Visible = true;
            txtHost2.Visible = false;
            intPort1.Visible = true;
            intPort2.Visible = true;
            intCP.Visible = false;
        }

        private void selectCC_CheckedChanged(object sender, EventArgs e)
        {
            txtHost1.Visible = true;
            txtHost2.Visible = true;
            intPort1.Visible = true;
            intPort2.Visible = true;
            intCP.Visible = true;
        }

        private void selectSS_CheckedChanged(object sender, EventArgs e)
        {
            txtHost1.Visible = false;
            txtHost2.Visible = false;
            intPort1.Visible = true;
            intPort2.Visible = true;
            intCP.Visible = true;
        }
        
        private void BlockInputs()
        {
            selectCC.Enabled = false;
            selectSS.Enabled = false;
            selectSC.Enabled = false;
            btnStart.Enabled = false;
            txtHost1.Enabled = false;
            txtHost2.Enabled = false;
            intPort1.Enabled = false;
            intPort2.Enabled = false;
            intCP.Enabled = false;
            intTimeout.Enabled = false;
            cbHttp.Enabled = false;
            cbSsl.Enabled = false;
            cbStreamLog.Enabled = false;

            txtLog.Focus();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            
        }

        public static void UsingArgs(string[] args)
        {
            PortTunnel tunnel = new PortTunnel(args);
            tunnel.Start();
            Console.ReadLine();
        }
    }
}
