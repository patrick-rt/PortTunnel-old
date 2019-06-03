package PortTunnelLib;

public class Connection {
    String Host;
    int Port;
    ConnectionType Type;

    public Connection(int port)
    {
        Type = ConnectionType.Server;
        Host = null;
        Port = port;
    }

    public Connection(String host, int port)
    {
        Type = ConnectionType.Client;
        Host = host;
        Port = port;
        if (Host.equals("")) {
            Type = ConnectionType.Server;
            Host = null;
        }
    }
}
