using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JotiApiT
{
    public class CommandInterpreter
    {

        public CommandInterpreter()
        {
            initialize();
        }

        private async void initialize()
        {
            Console.Title = "Joti API V2.0 Tester";
            key = await ApiUtil.Login();
        }

        public bool loop = true;

        public bool useRootPrefix = true;

        public string key = String.Empty;

        public async void readCommand()
        {
            string command = Console.ReadLine();

            string[] keywords = command.Split(' ');

            if(keywords.Length > 0)
            {
                switch(keywords[0].ToLower())
                {
                    case "get":
                        Console.WriteLine(await RequestUtil.Get(keywords[1]));
                        break;
                    case "post":

                        String content = String.Empty;
                        for (int i = 2; i < keywords.Length; i++)
                        {                         
                            content += runVarReplacement(keywords[i]);
                        }

                        Console.WriteLine(await RequestUtil.Post(keywords[1], content));
                        break;
                    case "clr":
                        Console.Clear();
                        break;
                    case "help":

                        Console.WriteLine("COMMANDS            DESCRIPTION");
                        Console.WriteLine("-----------------------------------------------------------------------------------");
                        Console.WriteLine("get {url}           Preforms a get request on the given url.");
                        Console.WriteLine("post {url} {data}   Preforms a post request on the given url with the given data.");
                        Console.WriteLine("clr                 Clears the screen.");
                        Console.WriteLine("help                Shows a tabel with all the commands.");

                        Console.WriteLine("");

                        Console.WriteLine("--- JSON ---");
                        Console.WriteLine("You can use variables in your json, for example give the SLEUTEL property in your json the value $key$, the $key$ will be replaced with the actually key.");
                        Console.WriteLine("VARIABLE            DESCRIPTION");
                        Console.WriteLine("-----------------------------------------------------------------------------------");
                        Console.WriteLine("$key$ The API key that is active.");
                        Console.WriteLine("$user$ The user that is logged in.");
                        break;
                }
            }
            if (loop) { readCommand(); }
        }


        public string runVarReplacement(string data)
        {
            int index = data.IndexOf("$");

            string copy = data;

            while(index != -1)
            {            
                int end = copy.IndexOf("$", index + 1);
                string variable = copy.Substring(index + 1, (end - index) - 1);

                string value = String.Empty;
                switch (variable)
                {
                    case "key":
                        value = "\"" + key + "\"";
                        break;
                    case "username":
                        value = "\"" + ApiUtil.Username + "\"";
                        break;
                }
                string first = copy.Substring(0, index);
                string second = copy.Substring(end + 1);
                copy = (first + value + second);
                index = copy.IndexOf("$");
            }
            return copy;
        }  
    }
}
