using JotiApiT.Encryption;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JotiApiT
{
    public static class ApiUtil
    {



        public static string Username = String.Empty;
 
        public static async Task<string> Login()
        {

            Console.WriteLine("--- LOGIN ---");
            Console.Write("Username: ");
            string gebruiker = Console.ReadLine();
            Username = gebruiker;

            Console.Write("Password: ");
            string password = Console.ReadLine();

            JObject jsonObj = new JObject();
            jsonObj.Add("gebruiker", gebruiker);
            jsonObj.Add("ww", SHA1Util.SHA1HashStringForUTF8String(password));

            string result = await RequestUtil.Post(LinkBuilder.Build(new string[] { "login" }), jsonObj.ToString());
            string key = String.Empty;


            try
            {
                key = JObject.Parse(result)["SLEUTEL"].ToString();
                Logger.Log("ApiUtil", "Key aqquired: " + key, Logger.LogLevel.Info);
            } catch(Exception e)
            {
                Logger.Log("ApiUtil", result, Logger.LogLevel.Error);
            }
            return key;
        }

        public const string RootV2 = "http://jotihunt-api_v2.area348.nl/";   
    }
}
