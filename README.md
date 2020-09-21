# ScrapeTheirTwitch

This is a simple little library made for scraping Twitch's API in c#. here is an example how to use.

Also the input must be a string array even if you want to only scrape one streamer.
```c#
static void Main(string[] args)
        {
            // A string[] is required
            string[] channels = { "quattroace", "shroud" };
            string clientID = "Your clientID";
            string token = "Your Oauth Token";

            // Create new FinalData resonse to hold scraped data
            List<FinalData> response = new List<FinalData>(Scrape.IsOnline(channels, clientID, token));

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
```
