package PortTunnelDesktop;

import PortTunnelLib.PortTunnel;

public class Main {

    public static void main(String[] args){
        if (args.length > 0)
        {
            PortTunnel tunnel = new PortTunnel(args);
            tunnel.Start();
        }
        else
        {
            try
            {
                Gui gui = new Gui();
            }
            catch (Exception e)
            {
                System.out.println("Please use args!");
                System.out.print(PortTunnel.GetHelp());
            }
        }

        try{
            System.in.read();
        }catch (Exception e){

        }
    }
}
