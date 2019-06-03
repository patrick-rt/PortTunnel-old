package PortTunnelLib;

import java.net.ServerSocket;
import java.net.Socket;

public class ServerClient extends Thread {
    PortTunnel main;

    public ServerClient(PortTunnel main){
        this.main = main;
        main.Connection2.Host = null;
    }

    public void run() {
        Log.Write(LogType.Info, "Starting ServerClient-Mode ...");
        Loop();
    }

    private void Loop()
    {
        try
        {
            ServerSocket listener = new ServerSocket(main.Connection2.Port);

            Socket client, dest;

            while (main.Status == 1)
            {
                Log.Write(LogType.Info, "Waiting for clients ...");
                client = listener.accept();
                Log.Write(LogType.Info, "New client from " + client.getInetAddress().toString());

                dest = new Socket(main.Connection1.Host,
                        main.Connection1.Port);

                Tunnel tunnel = new Tunnel(client, dest, main);
                tunnel.start();
            }
        }
        catch (Exception e)
        {
            Log.Write(LogType.Error, "Can't start PortTunnel");
            Log.Write(LogType.Error, e.getMessage());
        }
    }
}
