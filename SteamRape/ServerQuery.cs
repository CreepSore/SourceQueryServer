using System;
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
            List<byte[]> toMerge = new List<byte[]>();

            toMerge.Add(constructByteArray("ff:ff:ff:ff:49:11"));
            toMerge.Add(System.Text.Encoding.UTF8.GetBytes(hostname));
            toMerge.Add(new byte[] { 0x0 });
            toMerge.Add(System.Text.Encoding.UTF8.GetBytes(mapname));
            toMerge.Add(new byte[] { 0x0 });
            toMerge.Add(System.Text.Encoding.UTF8.GetBytes(foldername));
            toMerge.Add(new byte[] { 0x0 });
            toMerge.Add(System.Text.Encoding.UTF8.GetBytes(gamename));
            toMerge.Add(new byte[] { 0x0 });
            toMerge.Add(new byte[] { BitConverter.GetBytes(appid)[0], BitConverter.GetBytes(appid)[1] });
            toMerge.Add(new byte[] { playercount });
            toMerge.Add(new byte[] { maxplayers });
            toMerge.Add(new byte[] { botcount });

            byte st = (byte)0;
            if (servertype == servertypes.DEDICATED)
                st = (byte)'d';
            if (servertype == servertypes.NONDEDICATED)
                st = (byte)'l';
            if (servertype == servertypes.SOURCETVRELAY)
                st = (byte)'p';

            toMerge.Add(new byte[] { st });

            byte em = (byte)0;

            if (environment == environments.WINDOWS)
                em = (byte)'w';
            if (environment == environments.LINUX)
                em = (byte)'l';
            if (environment == environments.MAC)
                em = (byte)'m';

            toMerge.Add(new byte[] { em });
            toMerge.Add(new byte[] { ((isPrivate) ? (byte)0x1 : (byte)0x0) });
            toMerge.Add(new byte[] { ((isVAC) ? (byte)0x1 : (byte)0x0) });

            return mergeByteArrays(toMerge);
        }

        public static ServerQuery generateRandom() {
            ServerQuery sq = new ServerQuery();
            sq.hostname = randomString();
            sq.mapname = randomString();
            sq.gamename = randomString();
            sq.foldername = randomString();
            sq.playercount = randomByte();
            sq.maxplayers = randomByte();
            sq.environment = (environments)rnd.Next(0,2);
            sq.servertype = (servertypes)rnd.Next(0, 2);
            sq.isPrivate = ((rnd.NextDouble() > 0.5) ? true : false);
            sq.isVAC = ((rnd.NextDouble() > 0.5) ? true : false);
            return sq;
        }

        public byte[] generatePlayerList() {
            List<byte[]> toMerge = new List<byte[]>();

            toMerge.Add(constructByteArray("ff:ff:ff:ff:44"));
            toMerge.Add(new byte[] { (byte)(players.ToArray().Length) });
            foreach(PlayerData data in players) {
                toMerge.Add(data.generateByteArray());
            }

            byte[] merged = mergeByteArrays(toMerge);
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

        private byte[] mergeByteArrays(List<byte[]> arrays) {
            int neededLength = 0;
            foreach (byte[] arr in arrays) {
                neededLength += arr.Length;
            }

            byte[] merged = new byte[neededLength];

            int count = 0;
            foreach (byte[] arr in arrays) {
                for (int i = 0; i < arr.Length; i++) {
                    merged[count] = arr[i];
                    count++;
                }
            }

            return merged;
        }
    }
}
