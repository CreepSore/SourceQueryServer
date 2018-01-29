using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamRape {
    class PlayerData {

        public byte index;
        public string name;
        public int score;
        public float time;

        public byte[] generateByteArray() {
            List<byte[]> toMerge = new List<byte[]>();
            toMerge.Add(new byte[] { index });
            toMerge.Add(System.Text.Encoding.UTF8.GetBytes(name));
            toMerge.Add(new byte[] { 0x0 });
            toMerge.Add(BitConverter.GetBytes(score));
            toMerge.Add(BitConverter.GetBytes(time));
            return mergeByteArrays(toMerge);
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
