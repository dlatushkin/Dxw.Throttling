using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dxw.Throttling.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = System.Configuration.ConfigurationManager.GetSection("Throttling");

            Console.WriteLine("done"); Console.ReadKey();
        }
    }
}
