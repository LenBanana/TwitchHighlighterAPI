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
        // Test call /api/Highlight/GetVODHighlights?twitchID=816335899&timeframe=0.5
        [HttpGet, Route("Highlights")]
        public async Task<List<Highlight>> GetVODHighlights(string twitchID, double timeframe)
        {
            return await TwitchChatProcessing.GetChatHighlights(twitchID, timeframe);
        }
    }
}
