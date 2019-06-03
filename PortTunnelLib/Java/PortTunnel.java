package PortTunnelLib;

import java.io.*;
import java.util.*;

public class PortTunnel {
    //-------------------------Private Attributes------------------------------
    private int mode;

    //--------------------Package Private Attributes---------------------------
    Connection Connection1;
    Connection Connection2;
    int ControlPort;
    int Timeout;

    boolean destIsSsl = false;
    boolean httpProtocol = false;
    int TicksBetweenBytes = 1;

    int Status;
    int ActiveConnections = 0;
    List<StreamLogListener> StreamLogListeners = new ArrayList<>();
    List<LogListener> LogListeners = new ArrayList<>();

    //---------------------------Public Methods--------------------------------
    /// <summary>
    /// Constructor for command line args
    /// </summary>
    /// <param name="args">Arguments for PortTunnel</param>
    public PortTunnel(String[] args)
    {
        Log.main = this;
        boolean configured = true;

        String tmp = null;
        Map<String, String> pairs = new HashMap<>();
        pairs.put("-m", null);
        pairs.put("-h1", null);
        pairs.put("-p1", null);
        pairs.put("-h2", null);
        pairs.put("-p2", null);
        pairs.put("-cp", null);
        pairs.put("-t", null);

        AddListener(new LogListener() {
            @Override
            public void NewLogOutput(String text) {
                System.out.print(text);
            }
        });

        if (args.length > 0)
        {
            if (args.length > 1)
            {
                for (String arg : args)
                {
                    if (arg.equals("-http"))
                    {
                        httpProtocol = true;
                    }
                    else if (arg.equals("-ssl"))
                    {
                        destIsSsl = true;
                    }
                    else if (arg.equals("-log"))
                    {
                        AddListener(new StreamLogListener() {
                            @Override
                            public void NewStreamLog(String text) {
                                String filename = text.split("\r")[0]
                                        .replace(' ', '-')
                                        .replace(":", "-")+ ".txt";

                                try {
                                    new File("Log/").mkdirs();

                                    BufferedWriter writer = new BufferedWriter(new FileWriter("Log/" + filename));
                                    writer.write(text);

                                    writer.close();
                                }catch (Exception e){
                                    e.printStackTrace();
                                }
                            }
                        });
                    }
                    else
                    {
                        if (tmp == null)
                            tmp = arg;
                        else
                        {
                            try
                            {
                                pairs.replace(tmp, arg);
                            }
                            catch (Exception e)
                            {
                                Log.Write(LogType.Warning, "Unknown argument: " + tmp);
                            }
                            tmp = null;
                        }
                    }
                }

                if (pairs.get("-m").toLowerCase().equals("sc"))
                {
                    if (pairs.get("-h1") != null && pairs.get("-p1") != null && pairs.get("-p2") != null)
                    {
                        try
                        {
                            Connection1 = new Connection(pairs.get("-h1"), Integer.parseInt(pairs.get("-p1")));
                            Connection2 = new Connection(Integer.parseInt(pairs.get("-p2")));
                        }
                        catch (Exception e)
                        {
                            Log.Write(LogType.Error, "Hostname or port not supported!");
                            configured = false;
                        }
                    }
                    else
                    {
                        Log.Write(LogType.Error, "Arguments not set for mode " + pairs.get("-m"));
                        configured = false;
                    }
                }
                else if (pairs.get("-m").toLowerCase().equals("cc"))
                {
                    if (pairs.get("-h1") != null && pairs.get("-p1") != null &&
                            pairs.get("-h2") != null && pairs.get("-p2") != null && pairs.get("-cp") != null)
                    {
                        try
                        {
                            Connection1 = new Connection(pairs.get("-h1"), Integer.parseInt(pairs.get("-p1")));
                            Connection2 = new Connection(pairs.get("-h2"), Integer.parseInt(pairs.get("-p2")));
                            ControlPort = Integer.parseInt(pairs.get("-cp"));
                        }
                        catch (Exception e)
                        {
                            Log.Write(LogType.Error, "Hostname or port not supported!");
                            configured = false;
                        }
                    }
                    else
                    {
                        Log.Write(LogType.Error, "Arguments not set for mode " + pairs.get("-m"));
                        configured = false;
                    }
                }
                else if (pairs.get("-m").toLowerCase().equals("ss"))
                {
                    if (pairs.get("-p1") != null && pairs.get("-p2") != null && pairs.get("-cp") != null)
                    {
                        try
                        {
                            Connection1 = new Connection(Integer.parseInt(pairs.get("-p1")));
                            Connection2 = new Connection(Integer.parseInt(pairs.get("-p2")));
                            ControlPort = Integer.parseInt(pairs.get("-cp"));
                        }
                        catch (Exception e)
                        {
                            Log.Write(LogType.Error, "Hostname or port not supported!");
                            configured = false;
                        }
                    }
                    else
                    {
                        Log.Write(LogType.Error, "Arguments not set for mode " + pairs.get("-m"));
                        configured = false;
                    }
                }
                else
                {
                    Log.Write(LogType.Error, "Unknown mode!");
                    configured = false;
                }
            }
            else
            {
                configured = false;
                if(args[0].equals("-h"))
                {
                    Log.Write(LogType.Info, "\r\n" + GetHelp());
                }
            }
        }
        else
        {
            Log.Write(LogType.Error, "No args found!");
            configured = false;
        }

        try
        {
            Timeout = Integer.parseInt(pairs.get("-t"));
        }
        catch (Exception e)
        {
            Timeout = 3000;
        }

        if (!configured)
        {
            Log.Write(LogType.Info, "Exiting now");
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
                      int timeout, int controlPort)
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
    public boolean Start()
    {
        boolean supported = true;
        Status = 1;

        Thread Starter = null;

        if (Connection1.Type == ConnectionType.Client &&
                Connection2.Type == ConnectionType.Server)
        {
            mode = 0;
            Starter = new ServerClient(this);
        }
        else if (Connection1.Type == ConnectionType.Client &&
                Connection2.Type == ConnectionType.Client)
        {
            mode = 1;
            Starter = new ClientClient(this);
        }
        else if (Connection1.Type == ConnectionType.Server &&
                Connection2.Type == ConnectionType.Server)
        {
            mode = 2;
            Starter = new ServerServer(this);
        }
        else
        {
            supported = false;
            Log.Write(LogType.Error, "Config not supported");
        }

        ShowConfig();

        if (Starter != null)
            Starter.start();

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
            System.exit(0);
        }
        catch (Exception e) { }
    }

    /// <summary>
    /// Shows the config as log
    /// </summary>
    private void ShowConfig()
    {
        String tmp =
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

        tmp += "| HTTP: " + httpProtocol + "\r\n";

        tmp += "| SSL: " + destIsSsl + "\r\n";

        tmp += "| Timeout: " + Timeout + "\r\n";

        tmp += "------------------------------------------------\r\n";

        Log.Write(LogType.Info, "\r\n" + tmp);
    }

    /// <summary>
    /// Requests help
    /// </summary>
    /// <returns>Text of help file</returns>
    public static String GetHelp(){
        InputStream stream = PortTunnel.class.getResourceAsStream("/HelpFile.txt");
        BufferedInputStream buffer = new BufferedInputStream(stream);

        try {
            StringBuilder tmp = new StringBuilder();
            int tmpC;
            do{
                tmpC = buffer.read();
                if(tmpC == -1)
                    break;
                else
                    tmp.append((char)tmpC);
            }while(true);
            buffer.close();
            stream.close();
            return tmp.toString();
        }
        catch (Exception e) {
            return "";
        }
    }

    //-----------------------------Java specific-------------------------------

    public void AddListener(LogListener listener){
        LogListeners.add(listener);
    }

    public void AddListener(StreamLogListener listener){
        StreamLogListeners.add(listener);
    }

    public void setDestIsSsl(boolean isSsl){
        destIsSsl = isSsl;
    }

    public void setHttpProtocol(boolean usesHttp){
        httpProtocol = usesHttp;
    }

    public void setTicksBetweenBytes(int ticks){
        if(ticks >= 0)
            TicksBetweenBytes = ticks;
        else
            TicksBetweenBytes = 0;
    }

    public int getActiveConnections(){
        return ActiveConnections;
    }
}
