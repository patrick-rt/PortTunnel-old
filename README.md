# PortTunnel

# PortTunnel Help
## DESCRIPTION
Creates a "tunnel" through a firewall by
using an external server

## ARGUMENTS
-m [sc|cc|ss] - Mode (ServerClient, ClientClient,
 ServerServer)
-h1 [HOST] - Hostname 1
-h2 [HOST] - Hostname 2
-p1 [PORT] - Port 1
-p2 [PORT] - Port 2
-cp [PORT] - Control-Port
-t [ms] - Timeout in ms
-http - Replace HTTP Host-Header
-ssl - Destination needs SSL
-log - Log all Bytes of each Stream

## VARIABLES:
### General:
  Control-Port - Communication for PortTunnel
  Timeout - Timeout in ms for each tunnel

### ServerClient-Mode:
  Connection 1 - Destination server + port
  Connection 2 - Local server (port only)
  Control-Port can be empty

### ServerServer-Mode:
  Connection 1 - Local server (port only)
  Connection 2 - Local PortTunnel server (port only)

### ClientClient-Mode:
  Connection 1 - Destination server + port
  Connection 2 - External PortTunnel server + port

---------------------------------------------------------
