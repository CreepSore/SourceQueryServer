using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamRape {
    class ServerQuery {

        public string hostname, mapname, foldername, gamename;
        public byte playercount, maxplayers, botcount;
        public environments environment;
        public servertypes servertype;
        public bool isPrivate, isVAC;
        public short appid;

        public List<PlayerData> players = new List<PlayerData>();

        public byte[] generateQueryPacket() {
            MemoryStream memstream = new MemoryStream();
            memstream.write(constructByteArray("ff:ff:ff:ff:49:11"));
            memstream.write(System.Text.Encoding.UTF8.GetBytes(hostname));
            memstream.write(new byte[] { 0x0 });
            memstream.write(System.Text.Encoding.UTF8.GetBytes(mapname));
            memstream.write(new byte[] { 0x0 });
            memstream.write(System.Text.Encoding.UTF8.GetBytes(foldername));
            memstream.write(new byte[] { 0x0 });
            memstream.write(System.Text.Encoding.UTF8.GetBytes(gamename));
            memstream.write(new byte[] { 0x0 });
            memstream.write(BitConverter.GetBytes(appid));
            memstream.write(new byte[] { playercount });
            memstream.write(new byte[] { maxplayers });
            memstream.write(new byte[] { botcount });

            byte st = (byte)0;
            if (servertype == servertypes.DEDICATED)
                st = (byte)'d';
            if (servertype == servertypes.NONDEDICATED)
                st = (byte)'l';
            if (servertype == servertypes.SOURCETVRELAY)
                st = (byte)'p';

            memstream.write(new byte[] { st });

            byte em = (byte)0;

            if (environment == environments.WINDOWS)
                em = (byte)'w';
            if (environment == environments.LINUX)
                em = (byte)'l';
            if (environment == environments.MAC)
                em = (byte)'m';

            memstream.write(new byte[] { em });
            memstream.write(new byte[] { ((isPrivate) ? (byte)0x1 : (byte)0x0) });
            memstream.write(new byte[] { ((isVAC) ? (byte)0x1 : (byte)0x0) });

            return memstream.ToArray();
        }

        public static ServerQuery generateRandom() {
            ServerQuery sq = new ServerQuery();
            sq.hostname = randomString();
            sq.mapname = randomString();
            sq.gamename = randomString();
            sq.foldername = randomString();
            sq.playercount = randomByte();
            sq.maxplayers = randomByte();
            sq.botcount = ((rnd.NextDouble() > 0.5) ? (byte)1 : (byte)0);
            sq.environment = (environments)rnd.Next(0,2);
            sq.servertype = (servertypes)rnd.Next(0, 2);
            sq.isPrivate = ((rnd.NextDouble() > 0.5) ? true : false);
            sq.isVAC = ((rnd.NextDouble() > 0.5) ? true : false);
            return sq;
        }

        public byte[] generatePlayerList() {
            MemoryStream memstream = new MemoryStream();
            memstream.write(constructByteArray("ff:ff:ff:ff:44"));
            memstream.write(new byte[] { (byte)(players.ToArray().Length) });

            foreach(PlayerData data in players) {
                memstream.write(data.generateByteArray());
            }

            byte[] merged = memstream.ToArray();
            return merged;
        }

        public enum environments {
            WINDOWS, LINUX, MAC
        };

        public enum servertypes {
            DEDICATED, NONDEDICATED, SOURCETVRELAY
        };

        static Random rnd = new Random();

        private static byte randomByte() {
            return (byte)rnd.Next(1, 255);
        }

        private static string randomString() {
            
            string str = "";
            int count = rnd.Next(10, 20);

            for(int i = 0; i < count; i++) {
                str += (char)rnd.Next('A', 'Z');
            }
            
            return str;
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

    public static class MemoryStreamExtensions {
        public static void write(this MemoryStream stream, byte[] buffer) {
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}
