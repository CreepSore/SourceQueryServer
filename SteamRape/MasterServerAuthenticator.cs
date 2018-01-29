using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamRape {
    class MasterServerAuthenticator {

        public static void authenticate(ServerQuery queryData) {
            IPEndPoint endpoint = new IPEndPoint(Dns.GetHostAddresses("hl2master.steampowered.com")[0], 27011);
            UdpClient client = new UdpClient(14143);
            client.Connect(endpoint);
            client.Send(new byte[] { (byte)'q' }, 1);

            IPEndPoint server = new IPEndPoint(IPAddress.Any, 14143);
            byte[] buffer = client.Receive(ref server);
            byte[] challenge = new byte[] { buffer[6], buffer[7], buffer[8], buffer[9] };

            string servertype = "";
            if (queryData.servertype == ServerQuery.servertypes.DEDICATED)
                servertype = "d";
            if (queryData.servertype == ServerQuery.servertypes.NONDEDICATED)
                servertype = "l";
            if (queryData.servertype == ServerQuery.servertypes.SOURCETVRELAY)
                servertype = "p";


            string os = "";
            if (queryData.environment == ServerQuery.environments.WINDOWS)
                servertype = "w";
            if (queryData.environment == ServerQuery.environments.MAC)
                servertype = "m";
            if (queryData.environment == ServerQuery.environments.LINUX)
                servertype = "l";

            queryData.foldername = "cstrike";
            string authresponse = "0.\\protocol\\47\\challenge\\" + BitConverter.ToInt32(challenge, 0) + "\\players\\" + queryData.playercount + "\\max\\" + queryData.maxplayers + "\\bots\\" + queryData.botcount + "\\gamedir\\" + queryData.foldername + "\\map\\" + queryData.mapname + "\\type\\" + servertype + "\\password\\" + (queryData.isPrivate ? '1' : '0') + "\\os\\" + os + "\\secure\\" + (queryData.isVAC ? '1' : '0') + "\\lan\\0\\version\\1.1.2.5/Stdio\\region\\255\\product\\" + queryData.foldername + ".";

            client.Send(System.Text.Encoding.UTF8.GetBytes(authresponse), authresponse.Length);

            Console.WriteLine("Registered : " + queryData.hostname);
        }

    }
}
