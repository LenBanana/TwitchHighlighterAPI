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
        public static Dictionary<string, bool> QueuedRequests { get; set; } = new Dictionary<string, bool>();

        public static Dictionary<string, List<TwitchChat>> RequestedHighlights { get; set; } = new Dictionary<string, List<TwitchChat>>();

        public static HighlightQueueResult QueueRequest(string twitchID, double timeframe)
        {
            HighlightQueueResult result = new HighlightQueueResult();
            if (!QueuedRequests.ContainsKey(twitchID))
            {
                if (QueuedRequests.Count >= 125)
                {
                    result.Message = "There are currently too many requests being processed, please wait until one finishes and request again";
                    return result;
                }
                QueuedRequests.Add(twitchID, false);
                GetChatHighlights(twitchID, timeframe);
                result.Message = "ID is now being queued, please wait until the processing is done, and request again.";
            }
            else
            {
                if (!QueuedRequests[twitchID])
                {
                    result.Message = "This ID was already requested and is currently being processed";
                }
                else
                {
                    result.Highlights = FindHighlights(RequestedHighlights[twitchID], timeframe, twitchID);
                }
            }
            CheckQueue();
            return result;
        }

        public static void CheckQueue()
        {
            if (RequestedHighlights.Count > 1000)
            {
                RequestedHighlights.Remove(RequestedHighlights.First().Key);
            }
        }

        public static async void GetChatHighlights(string twitchID, double timeframe)
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
                RequestedHighlights.Add(twitchID, chats);
                QueuedRequests[twitchID] = true;
            }
        }

        static List<Highlight> FindHighlights(List<TwitchChat> chats, double timeframe, string twitchID)
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
                {
                    startDate = startDate.AddSeconds(timeframe);
                    continue;
                }
                var lastCreated = comments.Max(x => x.CreatedAt);
                var firstCreated = comments.Min(x => x.CreatedAt);
                Highlight highlight = new Highlight();
                highlight.VOD_ID = twitchID;
                highlight.StartTime = firstCreated.UtcDateTime;
                highlight.EndTime = lastCreated.UtcDateTime;
                highlight.MessageCount = comments.Count();
                highlight.TimeFrame = new TimeSpan(lastCreated.Ticks - firstCreated.Ticks).TotalSeconds;
                highlight.TimeOffset = TimeSpan.FromSeconds(comments.Min(x => x.ContentOffsetSeconds)).ToString("hh\\hmm\\mss\\s");
                comments.ToList().ForEach(x => highlight.HighlightMessages.Add(new HighlightMessage() { Message = x.Message.Body, Username = x.Commenter.DisplayName, WriteTime = x.CreatedAt.UtcDateTime, EmoteCount = x.Message.Emoticons != null ? x.Message.Emoticons.Count : 0 }));
                result.Add(highlight);

                startDate = startDate.AddSeconds(timeframe);
            }
            if (result.Count() > 0)
            {
                double EmoteWeight = 0.25;
                double maxCount = result.Max(x => (x.MessageCount + (x.HighlightMessages.Select(y => y.EmoteCount).Sum() * EmoteWeight)));
                foreach (var highlight in result)
                    highlight.Fitness = Math.Round((double)(highlight.MessageCount + (highlight.EmoteCount * EmoteWeight)) / (double)maxCount * 100.0, 2);
            }
            var orderedResult = result.OrderByDescending(x => x.Fitness).ToList();
            return orderedResult;
        }
    }
}
