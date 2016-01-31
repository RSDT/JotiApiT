using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JotiApiT
{
    public static class LinkBuilder
    {
        public static string Root = ApiUtil.RootV2;
    
        public static string Build(params string[] args)
        {
            string result = Root; 
            foreach(string s in args)
            {
                result += s + "/";
            }
            return result;
        }



    }
}
