using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JotiApiT
{
    public class RequestUtil
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<string> Post(string url, string data)
        {
            using (var client = new HttpClient())
            {
                Logger.Log("RequestUtil", "Posting to " + url + " with " + data, Logger.LogLevel.Debug);
                var response = await client.PostAsync(url, new StringContent(data));
                string result = await response.Content.ReadAsStringAsync();
                Logger.Log("RequestUtil", result + " got from " + url, Logger.LogLevel.Debug);
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> Get(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage message =  await client.GetAsync(url);
                string result = await message.Content.ReadAsStringAsync();            
                Logger.Log("RequestUtil", result + " got from " + url, Logger.LogLevel.Debug);
                return result;
            }
        }



    }
}
