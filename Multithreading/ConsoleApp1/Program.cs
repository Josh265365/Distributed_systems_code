using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading
{
    internal class Program
    {
        
        public static void Main(string[] args)
        {
            ThreadRunner runner = new ThreadRunner(); ;
            runner.Run();
        }
    }
}
