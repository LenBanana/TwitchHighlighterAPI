using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitchHighlighterAPI.Models;
using TwitchHighlighterAPI.Twitch;

namespace TwitchHighlighterAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HighlightController : ControllerBase
    {
        // Test call /api/Highlight/GetVODHighlights/Highlights?twitchID=816335899&timeframe=0.5
        [HttpGet]
        public HighlightQueueResult GetVODHighlights(string twitchID, double timeframe)
        {
            return TwitchChatProcessing.QueueRequest(twitchID, timeframe);
        }

        [HttpGet]
        public List<string> GetQueuedRequests()
        {
            return TwitchChatProcessing.QueuedRequests.Where(x => !x.Value).Select(x => x.Key).ToList();
        }
    }
}
