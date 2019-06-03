using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PortTunnelLib
{
    class ServerClient
    {
        private PortTunnel main;

        public ServerClient(PortTunnel main)
        {
            this.main = main;
            main.Connection2.Host = null;
        }

        public void Run()
        {
            Log.Write(Enums.LogType.Info, "Starting ServerClient-Mode ...");
            Thread loop = new Thread(Loop);
            loop.Start();
        }

        private void Loop()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any,
                    main.Connection2.Port);

                listener.Start();

                TcpClient client, dest;

                while (main.Status == 1)
                {
                    Log.Write(Enums.LogType.Info, "Waiting for clients ...");
                    client = listener.AcceptTcpClient();
                    Log.Write(Enums.LogType.Info, "New client from " +
                        ((IPEndPoint)client.Client.RemoteEndPoint)
                        .Address.ToString());

                    dest = new TcpClient(main.Connection1.Host,
                        main.Connection1.Port);

                    Tunnel tunnel = new Tunnel();
                    tunnel.Start(client, dest, main);
                }
            }
            catch
            {
                Log.Write(Enums.LogType.Error, "Can't start PortTunnel");
            }
        }
    }
}

