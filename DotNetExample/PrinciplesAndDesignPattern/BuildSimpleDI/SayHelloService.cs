using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildSimpleDI
{
    public class SayHelloService : ISayHelloService
    {
        public void SayHello(string message)
        {
            Console.WriteLine(message);
        }
    }
}
