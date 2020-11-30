using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchHighlighterAPI.Models;

namespace TwitchHighlighterAPI.Twitch
{
    public class TwitchChatProcessing
    {
        public static async Task<List<Highlight>> GetChatHighlights(string twitchID, double timeframe)
        {
            string url = "https://api.twitch.tv/v5/videos/" + twitchID + "/comments";
            List<TwitchChat> chats = new List<TwitchChat>();
            TwitchChat chat = new TwitchChat();
            bool firstRoll = true;
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Accept, "application/vnd.twitchtv.v5+json");
                client.Headers.Add("Client-ID", "jzkbprff40iqj646a697cyrvl0zt2m6");
                while (chat.Next != null && (chat.Next.Length > 0 || firstRoll))
                {
                    string requestUrl = url + (firstRoll ? "?content_offset_seconds=0" : "?cursor=" + chat.Next);
                    string json = await client.DownloadStringTaskAsync(new Uri(requestUrl));
                    chat = new TwitchChat().FromJson(json);
                    chats.Add(chat);
                    if (firstRoll)
                        firstRoll = false;
                }
            }
            return await FindHighlights(chats, timeframe, twitchID);
        }

        static async Task<List<Highlight>> FindHighlights(List<TwitchChat> chats, double timeframe, string twitchID)
        {
            return await Task<List<Highlight>>.Run(() =>
            {
                DateTime minDate = chats.First().Comments.First().CreatedAt.UtcDateTime;
                DateTime startDate = chats.First().Comments.First().CreatedAt.UtcDateTime;
                DateTime maxDate = chats.Last().Comments.Last().CreatedAt.UtcDateTime;
                var AllComments = chats.Select(x => x.Comments).SelectMany(x => x).ToList();
                List<Highlight> result = new List<Highlight>();
                while (minDate < maxDate)
                {
                    minDate = minDate.AddSeconds(timeframe);
                    var comments = AllComments.Where(y => y.CreatedAt.UtcDateTime <= minDate && y.CreatedAt.UtcDateTime >= startDate);
                    if (comments.Count() == 0)
                        continue;
                    var lastCreated = comments.Max(x => x.CreatedAt);
                    var firstCreated = comments.Min(x => x.CreatedAt);
                    Highlight highlight = new Highlight();
                    highlight.VOD_ID = twitchID;
                    highlight.StartTime = firstCreated.UtcDateTime;
                    highlight.EndTime = lastCreated.UtcDateTime;
                    highlight.MessageCount = comments.Count();
                    highlight.TimeFrame = new TimeSpan(lastCreated.Ticks - firstCreated.Ticks).TotalSeconds;
                    highlight.TimeOffset = TimeSpan.FromSeconds(comments.Min(x => x.ContentOffsetSeconds)).ToString("hh\\hmm\\mss\\s");
                    comments.ToList().ForEach(x => highlight.HighlightMessages.Add(new HighlightMessage() { Message = x.Message.Body, Username = x.Commenter.DisplayName, WriteTime = x.CreatedAt.UtcDateTime }));
                    result.Add(highlight);

                    startDate = startDate.AddSeconds(timeframe);
                }
                if (result.Count > 0)
                {
                    int maxCount = result.Max(x => x.MessageCount);
                    foreach (var highlight in result)
                        highlight.Fitness = Math.Round((double)highlight.MessageCount / (double)maxCount * 100.0, 2);
                }
                return result.OrderByDescending(x => x.MessageCount).ToList();
            });            
        }
    }
}
