using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchHighlighterAPI.Models;

namespace TwitchHighlighterAPI.Extensions
{
    public static class HighlightExtension
    {
        public static IEnumerable<HighlightMessage> FilterPartner(this IEnumerable<HighlightMessage> messages, bool include) => messages.Where(x => x.Badges.Any(y => y == "partner") == include);
        public static IEnumerable<HighlightMessage> FilterEmotes(this IEnumerable<HighlightMessage> messages, bool include) => messages.Where(x => !(x.Message.Count(char.IsWhiteSpace) == (x.EmoteCount - 1)) == include);
    }
}
