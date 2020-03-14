using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagIndex_Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MagIndex_Library {
    class MagIndex_Driver {
        private static Dictionary<string, MtgCard> cardRepo;
        //Main driver method
        static void Main(string[] args) {
            cardRepo = InitializeCardRepository();
            Console.WriteLine($"Acquired {cardRepo.Count} total card tokens from the JSON.");

            bool done = false;
            do {
                Console.WriteLine($"Please select an option...");
                Console.WriteLine("S - Search for a Card\nR - Output Random Cards\nQ - Quit Application");
                Console.Write("==> ");
                switch (Console.ReadLine().ToUpper()) {                                            
                    case "S":
                        Console.Write("Enter the name of a card to search for: ");
                        string userCardName = Console.ReadLine();
                        var foundCards = 
                            cardRepo.Where(x => x.Key.ToLower().Contains(userCardName.ToLower())).Select(y => y.Value).ToList();
                        foundCards.ForEach(x => { Console.WriteLine($"{x.ToString()}\n"); });                                                
                        break;
                    case "R":
                        Console.Write("Enter the number of cards to output: ");
                        if (int.TryParse(Console.ReadLine(), out int userNumCards) == true)
                            Console.WriteLine(GetRandomCards(userNumCards));
                        else
                            Console.WriteLine("Invalid entry entered.");
                        break;
                    case "Q":
                        done = true;
                        Console.WriteLine("Exiting application...");
                        break;
                    default:
                        break;
                }
            } while (!done);                        
        }

        //Helper Methods
        private static Dictionary<string, MtgCard> InitializeCardRepository() {
            string folderPath = $"{Environment.CurrentDirectory}\\json";
            string filePath = $"{folderPath}\\cardRepo.json";
            Dictionary<string, MtgCard> returnDict = new Dictionary<string, MtgCard>();            
            try {
                if (File.Exists(filePath)) {
                    Console.WriteLine($"Existing Json file found.");
                    Console.Write($"The Card Repository is {DateTime.UtcNow.Subtract(File.GetCreationTimeUtc(filePath).Date).TotalDays.ToString("0.####")} days out of date. Would you like to update now? (Y/N): ");
                    if (Console.ReadLine().ToUpper().Equals("Y")) {                        
                        string scrapedJson = MagIndex.GetMostRecentJson();
                        Console.WriteLine("Deleting existing Json file...");
                        File.Delete(filePath);
                        Console.WriteLine("Writing new Json file...");
                        Directory.CreateDirectory(folderPath);
                        using (StreamWriter sw = File.CreateText(filePath)) {
                            sw.WriteLine(scrapedJson);
                            sw.Close();
                        }
                    }
                } else {
                    Console.WriteLine($"No Json file found. Generating new file...");
                    string scrapedJson = MagIndex.GetMostRecentJson();
                    Console.WriteLine("Writing new Json file...");
                    Directory.CreateDirectory(folderPath);
                    using (StreamWriter sw = File.CreateText(filePath)) {
                        sw.WriteLine(scrapedJson);
                        sw.Close();
                    }
                }         
                using(StreamReader sr = File.OpenText(filePath)) {
                    returnDict = MtgCard.FromJson(sr.ReadToEnd());
                    sr.Close();
                }                
            }catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }

            return returnDict;
        }

        private static string GetRandomCards(int numCards) {
            Random rand = new Random(DateTime.Now.Millisecond);
            StringBuilder strb = new StringBuilder();

            strb.AppendLine($"Outputting {numCards} random cards:");
            strb.AppendLine("-----------------------------------------");            
            for (int i = 0; i < numCards; ++i) {
                strb.AppendLine(cardRepo[cardRepo.Keys.ElementAt(rand.Next(0, cardRepo.Count))].ToString());
                strb.AppendLine();
            }
            return strb.ToString();
        }
    }
}
