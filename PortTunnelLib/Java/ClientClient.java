package PortTunnelLib;

import java.net.Socket;

public class ClientClient extends Thread {
    PortTunnel main;

    public ClientClient(PortTunnel main){
        this.main = main;
    }

    public void run() {
        Log.Write(LogType.Info, "Starting ClientClient-Mode ...");
        Loop();
    }

    private void Loop()
    {
        try
        {
            Socket controlClient = new Socket(
                    main.Connection2.Host, main.ControlPort);

            sleep(2);

            controlClient.getOutputStream().write((byte)'c');

            sleep(2);

            while (main.Status == 1)
            {
                if (controlClient.getInputStream().available() > 0)
                {
                    if (controlClient.getInputStream().read()
                            == (byte)'n')
                    {
                        Socket cl1 = new Socket(
                                main.Connection2.Host, main.Connection2.Port);

                        Socket tunnelClient = new Socket(
                                main.Connection1.Host, main.Connection1.Port);

                        Tunnel tunnel = new Tunnel(cl1, tunnelClient, main);
                        tunnel.start();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Log.Write(LogType.Error,
                    "Can't start PortTunnel");
        }
    }
}
