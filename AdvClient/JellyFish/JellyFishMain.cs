using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFish
{
    public class JellyFishMain
    {
        public static string ApiKey;
        public static string SubsKey;
        public static string Url;

        public JellyFishMain(string url, string subsKey, string apiKey)
        {
            Url = url;
            SubsKey = subsKey;
            ApiKey = apiKey;
        }
    }
}
