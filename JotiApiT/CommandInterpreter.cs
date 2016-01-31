using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
        }

        public bool loop = true;

        public bool useRootPrefix = true;

        public string key = String.Empty;

        public async Task startLoop()
        {
            while(loop)
            {
                 readCommand();
            }
        }

        public async Task readCommand()
        {
            string command = Console.ReadLine();

            string[] keywords = command.Split(' ');

            if(keywords.Length > 0)
            {
                string mainKeyword = keywords[0].ToLower();
                string url = String.Empty;
                if (mainKeyword == "get"  || mainKeyword == "post")
                {
                    if (useRootPrefix)
                    {
                        url = ApiUtil.RootV2 + runVarReplacement(keywords[1], false);
                    }
                    else
                    {
                        url = runVarReplacement(keywords[1], false);
                    }
                }

                switch(mainKeyword)
                {
                    case "login":
                        key = await ApiUtil.Login();
                        break;
                    case "logout":
                        ApiUtil.Username = String.Empty;
                        key = String.Empty;
                        Logger.Log("CommandInterpreter", "Succesfully logged out.", Logger.LogLevel.Info);
                        break;
                    case "account":
                        Console.WriteLine("PROPERTY                   DESCRIPTION                        VALUE");
                        Console.WriteLine("-----------------------------------------------------------------------------------");
                        Console.WriteLine("Username                   The name of your account          " + ApiUtil.Username);
                        Console.WriteLine("API-KEY                    The API-key of your account       " + key);
                        Console.WriteLine("");
                        break;      
                    case "get":                       
                        Console.WriteLine(await RequestUtil.Get(url));
                        break;
                    case "post":

                        String content = String.Empty;
                        for (int i = 2; i < keywords.Length; i++)
                        {                         
                            content += runVarReplacement(keywords[i], true);
                        }

                        Console.WriteLine(await RequestUtil.Post(url, content));
                        break;

                    case "log":

                        if(keywords.Length > 1)
                        {
                            switch (keywords[1].ToLower())
                            {
                                case "info":
                                    Console.WriteLine("LOG LEVEL           DESCRIPTION                              COLOR");
                                    Console.WriteLine("-----------------------------------------------------------------------------------");
                                    Console.WriteLine("Info                Shows message with informatic purpose.  " + Logger.InfoColor);
                                    Console.WriteLine("Debug               Shows debug messages.                   " + Logger.DebugColor);
                                    Console.WriteLine("Error               Shows error messsages.                  " + Logger.ErrorColor);
                                    break;
                                case "setcolor":

                                    switch(keywords[2].ToLower())
                                    {
                                        case "info":
                                            Enum.TryParse<ConsoleColor>(keywords[3], out Logger.InfoColor);
                                            break;
                                        case "debug":
                                            Enum.TryParse<ConsoleColor>(keywords[3], out Logger.DebugColor);
                                            break;
                                        case "error":
                                            Enum.TryParse<ConsoleColor>(keywords[3], out Logger.ErrorColor);
                                            break;
                                    }

                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("COMMANDS                    DESCRIPTION");
                            Console.WriteLine("-----------------------------------------------------------------------------------");
                            Console.WriteLine("setColor {level} {color}    Sets the color of the given level. {color} is case-sensitive.");
                        }                        
                        break;
                    case "clr":
                        Console.Clear();
                        break;
                    case "exit":
                        loop = false;
                        break;
                    case "help":

                        Console.WriteLine("COMMANDS            DESCRIPTION");
                        Console.WriteLine("-----------------------------------------------------------------------------------");
                        Console.WriteLine("login               Starts the login procedure.");
                        Console.WriteLine("logout              Logs out.");
                        Console.WriteLine("account             Shows information about the current account.");
                        Console.WriteLine("get {url}           Preforms a get request on the given url.");
                        Console.WriteLine("post {url} {data}   Preforms a post request on the given url with the given data.");
                        Console.WriteLine("log                 Shows the commands for the log.");
                        Console.WriteLine("help                Shows a tabel with all the commands.");
                        Console.WriteLine("clr                 Clears the screen.");
                        Console.WriteLine("exit                Exits the application.");

                        Console.WriteLine("");

                        Console.WriteLine("--- JSON ---");
                        Console.WriteLine("You can use variables in your json, for example give the SLEUTEL property in your json the value $key$, the $key$ will be replaced with the actually key.");
                        Console.WriteLine("VARIABLE            DESCRIPTION");
                        Console.WriteLine("-----------------------------------------------------------------------------------");
                        Console.WriteLine("$key$               The API key that is active.");
                        Console.WriteLine("$username$          The user that is logged in.");
                        Console.WriteLine("");
                        break;
                }
            }
            if (loop) {
                await readCommand();
            }
        }


        public string runVarReplacement(string data, bool isOnJson)
        {
            try
            {
                int index = data.IndexOf("$");

                string copy = data;

                while (index != -1)
                {
                    int end = copy.IndexOf("$", index + 1);
                    string variable = copy.Substring(index + 1, (end - index) - 1);

                    string value = String.Empty;
                    switch (variable)
                    {
                        case "key":

                            if (key != null && key != String.Empty)
                            {
                                if (isOnJson)
                                {
                                    value = "\"" + key + "\"";
                                }
                                else
                                {
                                    value = key;
                                }
                            }
                            else
                            {
                                Logger.Log("CommandInterpreter", "API-KEY has not been set, consider a login.", Logger.LogLevel.Error);
                            }
                            break;
                        case "username":
                            if (isOnJson)
                            {
                                value = "\"" + ApiUtil.Username + "\"";
                            }
                            else
                            {
                                value = ApiUtil.Username;
                            }
                            break;
                    }
                    string first = copy.Substring(0, index);
                    string second = copy.Substring(end + 1);
                    copy = (first + value + second);
                    index = copy.IndexOf("$");
                }
                return copy;
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return data;
            }
        }  

        public void save()
        {
            JObject jObj = new JObject();
            jObj.Add("gebruiker", ApiUtil.Username);
            jObj.Add("SLEUTEL", key);
            jObj.Add("infoColor", Logger.InfoColor.ToString());
            jObj.Add("debugColor", Logger.DebugColor.ToString());
            jObj.Add("errorColor", Logger.ErrorColor.ToString());

            FileStream fStream = new FileStream(Environment.CurrentDirectory + @"\settings.json", FileMode.Create);
            StreamWriter sWriter = new StreamWriter(fStream);
            sWriter.Write(jObj.ToString());
            sWriter.Flush();
            sWriter.Close();
            sWriter.Dispose();
        }

        public async void fromSave()
        {
            if(File.Exists(Environment.CurrentDirectory + @"\settings.json"))
            {
                FileStream fStream = new FileStream(Environment.CurrentDirectory + @"\settings.json", FileMode.Open);
                StreamReader sReader = new StreamReader(fStream);
                JObject jObj = JObject.Parse(await sReader.ReadToEndAsync());
                ApiUtil.Username = jObj["gebruiker"].ToString();
                key = jObj["SLEUTEL"].ToString();
                Enum.TryParse<ConsoleColor>(jObj["infoColor"].ToString(), out Logger.ErrorColor);
                Enum.TryParse<ConsoleColor>(jObj["debugColor"].ToString(), out Logger.ErrorColor);
                Enum.TryParse<ConsoleColor>(jObj["errorColor"].ToString(), out Logger.ErrorColor);
                sReader.Close();
                sReader.Dispose();
            }
        }

    }
}
