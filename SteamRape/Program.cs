using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamRape {
    class Program {
        static void Main(string[] args) {
            //MasterServerAuthenticator.authenticate(ServerQuery.generateRandom());

            ServerSystem ss = new ServerSystem();
            ss.start();
        }
    }
}
