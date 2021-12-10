using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace util
{

    //Benjcore Utilities For C#

    public class Main {

        //Version
        private static readonly string version = "1.1.5";

        public static string Version {
            get {
                return version;
            }
        }

        public static void main(String[] args) {
            Console.WriteLine($"Benjcore Utilities C# Version {Version}");
        }

    }

}