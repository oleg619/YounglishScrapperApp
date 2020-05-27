using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YounglishScrapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var word = "exist";

            var client = new YounglishScrapper();

            // Handle exception
            var result = await client.GetWords(word);

            Console.WriteLine("Nearby words : " + string.Join(", ", result.NearbyWords));
            Console.WriteLine("Phonetic words : " + string.Join(", ", result.PhoneticWords));
        }
    }
}