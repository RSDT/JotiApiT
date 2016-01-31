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

            start();
        }

        static async void start()
        {
            CommandInterpreter interpreter = new CommandInterpreter();
            interpreter.fromSave();
            await interpreter.startLoop();

            interpreter.save();
            Console.WriteLine("Press any key to exit.");      
            Console.Read();
        }




    }
}
