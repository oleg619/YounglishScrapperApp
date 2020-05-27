using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RestSharp;

namespace YounglishScrapper
{
    public class YounglishScrapper
    {
        private readonly HtmlDocument _document;
        private readonly RestClient _client;

        private const string NearbyPath = "//*[@id=\"nearByPanel\"]/div[2]/ul/li";
        private const string PhoneticPath = "//*[@id=\"phoneticPanel\"]/div[2]/ul[2]/li";

        public YounglishScrapper()
        {
            _client = new RestClient("https://youglish.com/");

            _document = new HtmlDocument();
        }

        public async Task<Result> GetWords(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentOutOfRangeException(nameof(word), word);
            }

            var html = await GetHtmlPage(word);

            _document.LoadHtml(html);

            if (html.Contains("No result found."))
            {
                // Replace to domain exception or change handle logic
                throw new ArgumentException($"Can not find {nameof(word)} : {word}");
            }

            var nearbyWords = GetElements(NearbyPath);
            var phoneticWords = GetElements(PhoneticPath);

            return new Result(nearbyWords, phoneticWords);
        }

        private async Task<string> GetHtmlPage(string word)
        {
            var request = new RestRequest(Method.GET) {Resource = $"pronounce/{word}/english"};

            var response = await _client.ExecuteAsync(request);

            return response.Content;
        }

        private List<string> GetElements(string path) =>
            _document.DocumentNode
                .SelectNodes(path)
                .Select(li => li.InnerText.Replace("\n", "")) // remove all '\n' 
                .ToList();
    }

    public class Result
    {
        public readonly List<string> NearbyWords;
        public readonly List<string> PhoneticWords;

        public Result(List<string> nearbyWords, List<string> phoneticWords)
        {
            NearbyWords = nearbyWords;
            PhoneticWords = phoneticWords;
        }
    }
}