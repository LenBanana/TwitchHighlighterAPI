using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchHighlighterAPI.Models
{
    public class HighlightQueueResult
    {
        public string Message { get; set; }

        public IEnumerable<Highlight> Highlights { get; set; } = new List<Highlight>();
    }
}
