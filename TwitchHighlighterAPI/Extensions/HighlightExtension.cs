using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchHighlighterAPI.Models;

namespace TwitchHighlighterAPI.Extensions
{
    public static class HighlightExtension
    {
        static double EmoteWeight = 0.75;
        static double PartnerWeight = 0.25;
        public static IEnumerable<HighlightMessage> FilterPartner(this IEnumerable<HighlightMessage> messages, bool include) => messages.Where(x => x.Badges==null||x.Badges.Any(y => y == "partner") == include);
        public static IEnumerable<HighlightMessage> FilterEmotes(this IEnumerable<HighlightMessage> messages, bool include) => messages.Where(x => !(x.Message.Count(char.IsWhiteSpace) == (x.EmoteCount - 1)) == include);
        public static double TotalMessageCount(this Highlight highlight) => (highlight.MessageCount + (highlight.EmoteCount * EmoteWeight) + (highlight.PartnerCount * PartnerWeight));
    }
}
