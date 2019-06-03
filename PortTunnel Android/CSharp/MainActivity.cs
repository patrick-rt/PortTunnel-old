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

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using PortTunnelLib;

namespace PortTunnel_Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        EditText etH1, etH2, etP1, etP2, etCP, etTo;
        TextView log, activeConnections;
        Button btnStart, btnStop;
        CheckBox cbHttp, cbSsl;
        private PortTunnel tunnel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            
            GetItems();
            SetActions();
        }

        void GetItems()
        {
            etH1 = FindViewById<EditText>(Resource.Id.etHost1);
            etH2 = FindViewById<EditText>(Resource.Id.etHost2);
            etP1 = FindViewById<EditText>(Resource.Id.etPort1);
            etP2 = FindViewById<EditText>(Resource.Id.etPort2);
            etCP = FindViewById<EditText>(Resource.Id.etControlPort);
            etTo = FindViewById<EditText>(Resource.Id.etTimeout);

            log = FindViewById<TextView>(Resource.Id.txtLog);
            activeConnections = FindViewById<TextView>(Resource.Id.txtActiveConnections);

            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            btnStop = FindViewById<Button>(Resource.Id.btnStop);

            cbHttp = FindViewById<CheckBox>(Resource.Id.cbHttp);
            cbSsl = FindViewById<CheckBox>(Resource.Id.cbSsl);

            ISharedPreferences preferences = GetSharedPreferences("SETUP", FileCreationMode.Private);

            etH1.Text = preferences.GetString("H1", "illumina-chemie.de");
            etH2.Text = preferences.GetString("H2", "");

            etP1.Text = preferences.GetInt("P1", 80).ToString();
            etP2.Text = preferences.GetInt("P2", 8080).ToString();

            etCP.Text = preferences.GetInt("CP", 0).ToString();
            etTo.Text = preferences.GetInt("TO", 1000).ToString();

            cbHttp.Checked = preferences.GetBoolean("HTTP", false);
            cbSsl.Checked = preferences.GetBoolean("SSL", false);
        }

        void SetActions()
        {
            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
        }

        private void BtnStop_Click(object sender, System.EventArgs e)
        {
            try
            {
                tunnel.Stop();
            }
            catch { }

            try
            {
                PortTunnel.ExitEnvironment();
            }
            catch { }
        }

        private void BtnStart_Click(object sender, System.EventArgs e)
        {
            try
            {
                string h1 = etH1.Text;
                string h2 = etH2.Text;

                int p1 = int.Parse(etP1.Text);
                int p2 = int.Parse(etP2.Text);

                int cp = int.Parse(etCP.Text);
                int to = int.Parse(etTo.Text);

                ISharedPreferences preferences = GetSharedPreferences("SETUP", FileCreationMode.Private);
                ISharedPreferencesEditor editor = preferences.Edit();

                editor.PutString("H1", h1);
                editor.PutString("H2", h2);

                editor.PutInt("P1", p1);
                editor.PutInt("P2", p2);
                editor.PutInt("CP", cp);
                editor.PutInt("TO", to);

                editor.PutBoolean("HTTP", cbHttp.Checked);
                editor.PutBoolean("SSL", cbSsl.Checked);

                editor.Commit();

                Connection c1 = new Connection(h1, p1);
                Connection c2 = new Connection(h2, p2);

                tunnel = new PortTunnel(c1, c2, to, cp)
                {
                    HttpProtocol = cbHttp.Checked,
                    DestIsSsl = cbSsl.Checked
                };

                tunnel.NewLogOutput += (sen, text) =>
                {
                    try
                    {
                        log.Text += text;

                        activeConnections.Text = ((PortTunnel)tunnel).ActiveConnections.ToString();
                    }
                    catch { }
                };

                tunnel.NewStreamLogCompleted += (sen, content) => {

                };

                BlockInputs();

                tunnel.Start();
            }
            catch { }
        }

        private void BlockInputs()
        {
            btnStart.Enabled = false;
            etH1.Enabled=false;
            etH2.Enabled=false;
            etP1.Enabled=false;
            etP2.Enabled=false;
            etCP.Enabled=false;
            etTo.Enabled=false;
            cbHttp.Enabled=false;
            cbSsl.Enabled=false;
        }
    }
}
