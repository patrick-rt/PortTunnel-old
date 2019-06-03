package PortTunnelLib;

import java.net.ServerSocket;
import java.net.Socket;

public class ServerServer extends Thread{
    PortTunnel main;

    public ServerServer(PortTunnel main){
        this.main = main;
    }

    public void run() {
        Log.Write(LogType.Info, "Starting ServerServer-Mode ...");
        Loop();
    }

    private void Loop()
    {
        try
        {
            ServerSocket controlListener = new ServerSocket(main.ControlPort);

            Socket controlClient =
                    controlListener.accept();

            while (true)
            {
                if (controlClient.getInputStream().available() > 0)
                {
                    //Falsche Clients aussortieren
                    if (controlClient.getInputStream().read()
                            == (byte)'c')
                    {
                        break;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            ServerSocket mainListener = new ServerSocket(main.Connection1.Port);

            ServerSocket tunnelListener = new ServerSocket(main.Connection2.Port);

            while (main.Status == 1)
            {
                Socket client = mainListener.accept();

                controlClient.getOutputStream().write((byte)'n');

                Socket tunnelClient =
                        tunnelListener.accept();

                Tunnel tunnel = new Tunnel(client, tunnelClient, main);
                tunnel.start();
            }
        }
        catch (Exception e)
        {
            Log.Write(LogType.Error,
                    "Can't start PortTunnel");
        }
    }
}
