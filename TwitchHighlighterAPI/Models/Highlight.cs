using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchHighlighterAPI.Extensions;

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
        public double MessageCount { get { return HighlightMessages.FilterEmotes(false).FilterPartner(false).Count();  } }
        public double EmoteCount { get { return HighlightMessages.FilterEmotes(true).Count(); } }
        public double PartnerCount { get { return HighlightMessages.FilterPartner(true).Count(); } }
        public double Fitness { get; set; }
        public string TimeOffset { get; set; }
        public List<HighlightMessage> HighlightMessages { get; set; }
    }
}
