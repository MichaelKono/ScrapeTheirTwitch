using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ScrapeTheirTwitch
{
    public class Returns
    {
        [JsonProperty("id")] public string ID { get; set; }
        [JsonProperty("userId")] public string UserId { get; set; }
        [JsonProperty("user_name")] public string UserName { get; set; }
        [JsonProperty("game_id")] public string GameId { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("viewer_count")] public int ViewerCount { get; set; }
        [JsonProperty("datetime")] public DateTime StartedAt { get; set; }
        [JsonProperty("language")] public string Language { get; set; }
        [JsonProperty("thumb_url")] public string ThumbnailUrl { get; set; }
        [JsonProperty("tag_ids")] public List<string> TagIds { get; set; }
    }

    public class Pagination
    {
        public string cursor { get; set; }
    }

    // Final data is what you use to access the data from Returns, as the Data parameter is a list, multiple streamers can be kept in this one list.
    public class FinalData
    {
        public List<Returns> Data { get; set; }
        public Pagination Pagination { get; set; }
    }

    class Scrape
    {
        public static List<FinalData> IsOnline(string[] channels)
        {
            var hc = new HttpClient();

            hc.DefaultRequestHeaders.Add("Client-ID", "Your client ID");
            hc.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "Your token");

            List<FinalData> r = new List<FinalData>();

            foreach (string channel in channels)
            {
                var response = hc.GetStringAsync($"https://api.twitch.tv/helix/streams?user_login={channel}");
                string jsonString = response.Result;

                r.Add(JsonConvert.DeserializeObject<FinalData>(jsonString));
            }
            return r;
        }

    }

    // Our program class is an example how to use, I provided a simple method that searches for multiple streamers and give's an update
    class Program
    {
        static void Main(string[] args)
        {
            // A string[] is required
            string[] Clients = { "quattroace", "shroud" };

            // Create new FinalData resonse to hold scraped data
            List<FinalData> response = new List<FinalData>(Scrape.IsOnline(Clients));

            // Iterate through the multiple entrys and utilize data. You do not have to iterate if just scraping for one streamer.
            foreach (FinalData responseIng in response)
            {
                // Checks if there is no data for current iteration
                if (responseIng.Data.Count == 0)
                    Console.WriteLine("Stream is offline");
                else
                    Console.WriteLine(responseIng.Data[0].UserName + " " + responseIng.Data[0].Type);
            }
        }
    }
}
