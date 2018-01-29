using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace SteamRape {
    class ServerSystem {
        UdpClient socket;
        public void start() {
            socket = new UdpClient(new IPEndPoint(IPAddress.Any, 1337));

            while (true) {
                string packet = "";
                byte[] buffer = new byte[1024];
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                try {
                    buffer = this.socket.Receive(ref sender);
                } catch { }
                
                packet = System.Text.Encoding.UTF8.GetString(buffer);
                
                if(packet.ToLower().Contains("tsource")) {
                    Console.WriteLine("Incoming Query Request from " + sender.Address);
                    ServerQuery sq = new ServerQuery();
                    sq.hostname = "Mario Kart DS - Multiplayer";
                    sq.mapname = "Luigi's Mansion";
                    sq.gamename = "Mario Kart DS";
                    sq.foldername = "";
                    sq.appid = 730;
                    sq.playercount = 2;
                    sq.maxplayers = 10;
                    sq.environment = ServerQuery.environments.WINDOWS;
                    sq.servertype = ServerQuery.servertypes.DEDICATED;
                    sq.isPrivate = false;
                    sq.isVAC = true;

                    //sq = ServerQuery.generateRandom();
                    sq.appid = 730;

                    byte[] queryPacket = sq.generateQueryPacket();

                    Console.WriteLine("Answering with following Data:");
                    Console.WriteLine("Hostname: " + sq.hostname);
                    Console.WriteLine("Mapname: " + sq.mapname);
                    Console.WriteLine("Gamename: " + sq.gamename);
                    Console.WriteLine("Foldername: " + sq.foldername);
                    Console.WriteLine("Players: " + sq.playercount);
                    Console.WriteLine("Max-Players: " + sq.maxplayers);
                    Console.WriteLine("Environment: " + sq.environment.ToString());
                    Console.WriteLine("Servertype: " + sq.servertype.ToString());
                    Console.WriteLine("Private: " + sq.isPrivate.ToString());
                    Console.WriteLine("VAC enabled: " + sq.isVAC.ToString());

                    Console.WriteLine("----------------------------------------");

                    
                    socket.Send(queryPacket, queryPacket.Length, sender);
                } else if(packet.ToLower().Contains("u") && !packet.ToLower().Contains("-1")) {
                    ServerQuery sq = new ServerQuery();
                    PlayerData ply = new PlayerData();
                    ply.index = 0;
                    ply.name = "Bush";
                    ply.score = 420;
                    ply.time = 60 * 60 * 9 + 11 * 60;
                    sq.players.Add(ply);

                    ply = new PlayerData();
                    ply.index = 0;
                    ply.name = "did";
                    ply.score = 360;
                    ply.time = 60 * 60 * 9 + 11 * 60;
                    sq.players.Add(ply);

                    ply = new PlayerData();
                    ply.index = 0;
                    ply.name = "9/11";
                    ply.score = 88;
                    ply.time = 60*60*9 + 11*60;
                    sq.players.Add(ply);

                    socket.Send(sq.generatePlayerList(), sq.generatePlayerList().Length, sender);
                } else if (packet.ToLower().Contains("u")) {
                    socket.Send(constructByteArray("FF:FF:FF:FF:41:4B:A1:D5:22"), constructByteArray("FF:FF:FF:FF:41:4B:A1:D5:22").Length, sender);
                }
            }
        }

        private byte[] constructByteArray(string filter) {
            string[] splitted = filter.Split(':');
            byte[] packet = new byte[splitted.Length];

            int k = 0;
            foreach (string str in splitted) {
                packet[k] = Convert.ToByte(str, 16);
                k++;
            }

            return packet;
        }
    }
}
