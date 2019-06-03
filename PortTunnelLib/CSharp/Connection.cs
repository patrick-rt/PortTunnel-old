namespace PortTunnelLib
{
    public class Connection
    {
        public string Host { get; internal set; }
        public int Port { get; private set; }
        public Enums.ConnectionType Type { get; private set; }

        public Connection(int port)
        {
            Type = Enums.ConnectionType.Server;
            Host = null;
            Port = port;
        }

        public Connection(string host, int port)
        {
            Type = Enums.ConnectionType.Client;
            Host = host;
            Port = port;
            if (Host == "")
            {
                Type = Enums.ConnectionType.Server;
                Host = null;
            }
        }
    }
}
