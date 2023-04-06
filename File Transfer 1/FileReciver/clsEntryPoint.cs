using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReciver
{
    class clsEntryPoint
    {
        static void Main(string[] args)
        {
            Server s = new Server(1234);
            s.StartServer();
            Console.Read();
            s.StopServer();

        }
    }
}
