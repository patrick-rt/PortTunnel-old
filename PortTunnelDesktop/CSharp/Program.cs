using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace PortTunnel_Desktop
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        { 
            if (!File.Exists("PortTunnelLib.dll")) {
                WebClient client = new WebClient();
                client.DownloadFile("https://github.com/patrick72762/PortTunnel/raw/master/Release/PortTunnelLib.dll", "PortTunnelLib.dll");
            }

            if (args.Length > 0)
            {
                GUI.UsingArgs(args);
            }
            else
            {
                try
                {
                    Application.Run(new GUI());
                }
                catch
                {
                    Console.WriteLine("Please use args!");
                }
            }
        }
    }
}
