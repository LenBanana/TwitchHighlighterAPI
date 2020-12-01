using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchHighlighterAPI.Models
{
    public class Highlight
    {
        public Highlight()
        {
            HighlightMessages = new List<HighlightMessage>();
        }
        public string VOD_ID { get; set; }
        public string VOD_URL { get { return "https://www.twitch.tv/videos/" + VOD_ID + "?t=" + TimeOffset; } }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TimeFrame { get; set; }
        public int MessageCount { get { return HighlightMessages.Where(x => !(x.Message.Count(char.IsWhiteSpace)==(x.EmoteCount-1))).Count();  } }
        public int EmoteCount { get { return HighlightMessages.Select(x => x.EmoteCount).Sum(); } }
        public double Fitness { get; set; }
        public string TimeOffset { get; set; }
        public List<HighlightMessage> HighlightMessages { get; set; }
    }
}
