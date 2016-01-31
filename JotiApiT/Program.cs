using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JotiApiT
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandInterpreter interpreter = new CommandInterpreter();
            interpreter.readCommand();


            Console.Read();
        }
    }
}
