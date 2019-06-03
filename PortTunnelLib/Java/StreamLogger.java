package PortTunnelLib;

import java.net.Socket;
import java.util.HashMap;
import java.util.Map;

public class StreamLogger {
    private static Map<String, String> log = new HashMap<>();

    public static void Write(Socket source, Socket dest, byte b)
    {
        String ep = "From " +
                source.getInetAddress().toString().split("/")[1] + ":" +
                source.getPort() + " to " +
                dest.getInetAddress().toString().split("/")[1] + ":" +
                dest.getPort();

        if (!log.containsKey(ep))
        {
            log.put(ep, "StreamLog " + ep + "\r\n\r\n" +
                    (char)b);
        }
        else
        {
            String tmp = log.get(ep);
            log.put(ep, tmp + (char)b);
        }
    }

    public static void EndLogging(Socket source, Socket dest)
    {
        String ep = "From " +
                source.getInetAddress().toString().split("/")[1] + ":" +
                source.getPort() + " to " +
                dest.getInetAddress().toString().split("/")[1] + ":" +
                dest.getPort();

        if (log.containsKey(ep))
        {
            for(StreamLogListener l : Log.main.StreamLogListeners)
                l.NewStreamLog(log.get(ep));

            log.remove(ep);
        }
    }
}
