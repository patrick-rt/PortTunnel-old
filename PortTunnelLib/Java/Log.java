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
