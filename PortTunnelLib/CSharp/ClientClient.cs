using System.Net.Sockets;
using System.Threading;

namespace PortTunnelLib
{
    class ClientClient
    {
        private PortTunnel main;

        public ClientClient(PortTunnel main)
        {
            this.main = main;
        }

        public void Run()
        {
            Log.Write(Enums.LogType.Info, "Starting ClientClient-Mode ...");
            Thread loop = new Thread(Loop);
            loop.Start();
        }

        private void Loop()
        {
            try
            {
                TcpClient controlClient = new TcpClient(
                    main.Connection2.Host, main.ControlPort);

                Log.Write(Enums.LogType.Info, "Connected to PortTunnel Server");

                Thread.Sleep(2);

                controlClient.GetStream().WriteByte((byte)'c');

                Thread.Sleep(2);

                while (main.Status == 1)
                {
                    if (controlClient.Available > 0)
                    {
                        if (controlClient.GetStream().ReadByte()
                            == (byte)'n')
                        {
                            TcpClient cl1 = new TcpClient(
                                main.Connection2.Host, main.Connection2.Port);

                            TcpClient tunnelClient = new TcpClient(
                                main.Connection1.Host, main.Connection1.Port);

                            Tunnel tunnel = new Tunnel();
                            tunnel.Start(cl1, tunnelClient, main);
                        }
                    }
                }
            }
            catch
            {
                Log.Write(Enums.LogType.Error,
                    "Can't start PortTunnel");
            }
        }
    }
}

