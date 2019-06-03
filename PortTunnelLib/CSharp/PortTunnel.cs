/*
MIT License
Copyright (c) 2019 Patrick72762
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PortTunnelLib
{
    /// <summary>
    /// PortTunnel main class
    /// </summary>
    public class PortTunnel
    {
        //-------------------------Private Attributes--------------------------
        private int mode;

        //---------------------------Inernal fields----------------------------
        internal Connection Connection1 { get; private set; }
        internal Connection Connection2 { get; private set; }
        internal int ControlPort { get; private set; }
        internal int Timeout { get; private set; }

        //----------------------------Public fields----------------------------
        private bool destIsSsl = false;
        private bool httpProtocol = false;
        private long ticksBetweenBytes = 10;
        /// <summary>
        /// Encrypted connection to destination server if set to true
        /// </summary>
        public bool DestIsSsl
        {
            get
            {
                return destIsSsl;
            }
            set
            {
                if (Status == 0)
                    destIsSsl = value;
            }
        }
        /// <summary>
        /// Replaces HTTP header if set to true
        /// </summary>
        public bool HttpProtocol
        {
            get
            {
                return httpProtocol;
            }
            set
            {
                if (Status == 0)
                    httpProtocol = value;
            }
        }
        /// <summary>
        /// Ticks between bytes in a tunnel
        /// </summary>
        public long TicksBetweenBytes
        {
            get
            {
                return ticksBetweenBytes;
            }
            set
            {
                if (value >= 0)
                    ticksBetweenBytes = value;
                else
                    ticksBetweenBytes = 0;
            }
        }

        /// <summary>
        /// Status of PortTunnel
        /// 0 - Stopped
        /// 1 - Active
        /// </summary>
        public int Status { get; private set; }
        /// <summary>
        /// Active tunnels of this PortTunnel
        /// </summary>
        public int ActiveConnections { get; internal set; } = 0;
        /// <summary>
        /// Consume to output or display log data
        /// </summary>
        public EventHandler<string> NewLogOutput;
        /// <summary>
        /// Consume to get stream data after stream is closed
        /// </summary>
        public EventHandler<string> NewStreamLogCompleted;

        //---------------------------Public Methods----------------------------
        /// <summary>
        /// Constructor for command line args
        /// </summary>
        /// <param name="args">Arguments for PortTunnel</param>
        public PortTunnel(string[] args)
        {
            Log.main = this;
            bool configured = true;

            string tmp = null;
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("-m", null);
            pairs.Add("-h1", null);
            pairs.Add("-p1", null);
            pairs.Add("-h2", null);
            pairs.Add("-p2", null);
            pairs.Add("-cp", null);
            pairs.Add("-t", null);

            NewLogOutput += (sender, text) =>
             {
                 Console.Write(text);
             };

            if (args.Length > 0)
            {
                if (args.Length > 1)
                {

                    foreach (string arg in args)
                    {
                        if (arg == "-http")
                        {
                            httpProtocol = true;
                        }
                        else if (arg == "-ssl")
                        {
                            destIsSsl = true;
                        }
                        else if (arg == "-log")
                        {
                            NewStreamLogCompleted += (sender, text) =>
                            {
                                string filename = ((string)text).Split('\r')[0]
                                    .Replace(' ', '-').Replace(":", "-Port-") + ".txt";

                                if (!Directory.Exists("Log"))
                                    Directory.CreateDirectory("Log");

                                File.WriteAllText("Log/" + filename, (string)text);
                            };
                        }
                        else
                        {
                            if (tmp == null)
                                tmp = arg;
                            else
                            {
                                try
                                {
                                    pairs[tmp] = arg;
                                }
                                catch
                                {
                                    Log.Write(Enums.LogType.Warning, "Unknown argument: " + tmp);
                                }
                                tmp = null;
                            }
                        }
                    }

                    if (pairs["-m"].ToLower() == "sc")
                    {
                        if (pairs["-h1"] != null && pairs["-p1"] != null && pairs["-p2"] != null)
                        {
                            try
                            {
                                Connection1 = new Connection(pairs["-h1"], int.Parse(pairs["-p1"]));
                                Connection2 = new Connection(int.Parse(pairs["-p2"]));
                            }
                            catch
                            {
                                Log.Write(Enums.LogType.Error, "Hostname or port not supported!");
                                configured = false;
                            }
                        }
                        else
                        {
                            Log.Write(Enums.LogType.Error, "Arguments not set for mode " + pairs["-m"]);
                            configured = false;
                        }
                    }
                    else if (pairs["-m"].ToLower() == "cc")
                    {
                        if (pairs["-h1"] != null && pairs["-p1"] != null &&
                            pairs["-h1"] != null && pairs["-p2"] != null && pairs["-cp"] != null)
                        {
                            try
                            {
                                Connection1 = new Connection(pairs["-h1"], int.Parse(pairs["-p1"]));
                                Connection2 = new Connection(pairs["-h2"], int.Parse(pairs["-p2"]));
                                ControlPort = int.Parse(pairs["-cp"]);
                            }
                            catch
                            {
                                Log.Write(Enums.LogType.Error, "Hostname or port not supported!");
                                configured = false;
                            }
                        }
                        else
                        {
                            Log.Write(Enums.LogType.Error, "Arguments not set for mode " + pairs["-m"]);
                            configured = false;
                        }
                    }
                    else if (pairs["-m"].ToLower() == "ss")
                    {
                        if (pairs["-p1"] != null && pairs["-p2"] != null && pairs["-cp"] != null)
                        {
                            try
                            {
                                Connection1 = new Connection(int.Parse(pairs["-p1"]));
                                Connection2 = new Connection(int.Parse(pairs["-p2"]));
                                ControlPort = int.Parse(pairs["-cp"]);
                            }
                            catch
                            {
                                Log.Write(Enums.LogType.Error, "Hostname or port not supported!");
                                configured = false;
                            }
                        }
                        else
                        {
                            Log.Write(Enums.LogType.Error, "Arguments not set for mode " + pairs["-m"]);
                            configured = false;
                        }
                    }
                    else
                    {
                        Log.Write(Enums.LogType.Error, "Unknown mode!");
                        configured = false;
                    }
                }
                else
                {
                    configured = false;
                    if(args[0] == "-h")
                    {
                        Log.Write(Enums.LogType.Info, "\r\n" + GetHelp());
                    }
                }
            }
            else
            {
                Log.Write(Enums.LogType.Error, "No args found!");
                configured = false;
            }

            try
            {
                Timeout = int.Parse(pairs["-t"]);
            }
            catch
            {
                Timeout = 3000;
            }

            if (!configured)
            {
                Log.Write(Enums.LogType.Info, "Exiting now");
                ExitEnvironment();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection1">Object</param>
        /// <param name="connection2">Object</param>
        /// <param name="timeout">Timeout for each tunnel</param>
        /// <param name="controlPort">PortTunnel communication port</param>
        public PortTunnel(Connection connection1, Connection connection2,
            int timeout, int controlPort = 0)
        {
            Log.main = this;

            Connection1 = connection1;
            Connection2 = connection2;
            ControlPort = controlPort;
            Timeout = timeout;
        }

        /// <summary>
        /// Starts PortTunnel
        /// </summary>
        /// <returns>false if config is not supported</returns>
        public bool Start()
        {
            bool supported = true;
            Status = 1;

            if (Connection1.Type == Enums.ConnectionType.Client &&
                Connection2.Type == Enums.ConnectionType.Server)
            {
                mode = 0;
                ServerClient Starter = new ServerClient(this);
                ShowConfig();
                Starter.Run();
            }
            else if (Connection1.Type == Enums.ConnectionType.Client &&
                Connection2.Type == Enums.ConnectionType.Client)
            {
                mode = 1;
                ClientClient Starter = new ClientClient(this);
                ShowConfig();
                Starter.Run();
            }
            else if (Connection1.Type == Enums.ConnectionType.Server &&
                Connection2.Type == Enums.ConnectionType.Server)
            {
                mode = 2;
                ServerServer Starter = new ServerServer(this);
                ShowConfig();
                Starter.Run();
            }
            else
            {
                supported = false;
                Log.Write(Enums.LogType.Error, "Config not supported");
            }

            return supported;
        }

        /// <summary>
        /// Stops PortTunnel
        /// </summary>
        public void Stop()
        {
            Status = 0;
        }

        /// <summary>
        /// Exits the main process
        /// </summary>
        public static void ExitEnvironment()
        {
            try
            {
                Environment.Exit(0);
            }
            catch { }
        }

        /// <summary>
        /// Shows the config as log
        /// </summary>
        public void ShowConfig()
        {
            string tmp =
                "---------------------Config---------------------\r\n" +
                "| Mode: ";

            if (mode == 0)
            {
                tmp += "ServerClient\r\n" +
                    "| Internal Port: " +
                    Connection2.Port + "\r\n" +
                    "| Destination: " +
                    Connection1.Host + ":" +
                    Connection1.Port + "\r\n";

            }
            else if (mode == 1)
            {
                tmp += "ClientClient\r\n" +
                    "| Destination-Server: " +
                    Connection1.Host + ":" +
                    Connection1.Port + "\r\n" +
                    "| PortTunnel Server: " +
                    Connection2.Host + ":" +
                    Connection2.Port + "\r\n" +
                    "| PortTunnel Control-Port: " +
                    ControlPort + "\r\n";
            }
            else if (mode == 2)
            {
                tmp += "ServerServer\r\n" +
                    "| Internal Server-Port: " +
                    Connection1.Port + "\r\n" +
                    "| PortTunnel Server-Port: " +
                    Connection2.Port + "\r\n" +
                    "| PortTunnel Control-Port: " +
                    ControlPort + "\r\n";
            }

            tmp += "| HTTP: " + HttpProtocol + "\r\n";

            tmp += "| SSL: " + DestIsSsl + "\r\n";

            tmp += "| Timeout: " + Timeout + "\r\n";

            tmp += "------------------------------------------------\r\n";

            Log.Write(Enums.LogType.Info, "\r\n" + tmp);
        }

        /// <summary>
        /// Requests help
        /// </summary>
        /// <returns>Text of help file</returns>
        public static string GetHelp()
        {
            Assembly _Assembly = Assembly.GetExecutingAssembly();
            Stream str = _Assembly
                .GetManifestResourceStream("PortTunnelLib.HelpFile.txt");
            StreamReader rd = new StreamReader(str);

            string tmp = rd.ReadToEnd();

            rd.Close();

            return tmp;
        }
    }
}
