using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MagIndex_Library {
    class MagIndex {        
        // Helper Methods
        public static string GetMostRecentJson() {
            //Get the Json as a string
            string mtgjsonUrl = Uri.EscapeUriString("https://mtgjson.com/json/AllCards.json");
            string mtgjsonContent;
            using (System.Net.WebClient wClient = new System.Net.WebClient()) {
                Console.WriteLine($"Downloading from {mtgjsonUrl}... Please wait.");
                wClient.Encoding = System.Text.UTF8Encoding.UTF8;
                mtgjsonContent = wClient.DownloadString(mtgjsonUrl);
                Console.WriteLine($"Url content acquired. :)");
            }
            return mtgjsonContent;
        }
    }
}
