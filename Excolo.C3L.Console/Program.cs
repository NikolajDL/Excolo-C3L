using Excolo.C3L.Executer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Console
{
    public class Program
    {
        static void Main(string[] args)
        {

            StreamReader input = new StreamReader(System.Console.OpenStandardInput());;
            StreamWriter output = new StreamWriter(System.Console.OpenStandardOutput());;
            StreamWriter log = new StreamWriter(new FileStream("log.txt", FileMode.Append));;

            var shell = new Shell(input, output, log);
            shell.Start();

        }
    }
}
