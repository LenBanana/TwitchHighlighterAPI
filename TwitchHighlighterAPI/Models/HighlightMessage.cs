using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchHighlighterAPI.Models
{
    public class HighlightMessage
    {
        public DateTime WriteTime { get; set; }
        public int EmoteCount { get; set; }
        public List<string> Badges { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
