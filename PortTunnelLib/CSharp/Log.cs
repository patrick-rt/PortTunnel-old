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
