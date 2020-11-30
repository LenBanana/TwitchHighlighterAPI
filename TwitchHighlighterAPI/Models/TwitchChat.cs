using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchHighlighterAPI.Models
{
    public static class TwitchChatExtension
    {
        public static TwitchChat FromJson(this TwitchChat chat, string json) => JsonConvert.DeserializeObject<TwitchChat>(json);
    }
    public class TwitchChat
    {
        public TwitchChat()
        {
            Next = "";
        }

        [JsonProperty("comments")]
        public List<Comment> Comments { get; set; }

        [JsonProperty("_next")]
        public string Next { get; set; }
    }


    public class Comment
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("channel_id")]
        public long ChannelId { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("content_id")]
        public long ContentId { get; set; }

        [JsonProperty("content_offset_seconds")]
        public double ContentOffsetSeconds { get; set; }

        [JsonProperty("commenter")]
        public Commenter Commenter { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }
    }

    public class Commenter
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("logo")]
        public Uri Logo { get; set; }
    }

    public class Message
    {
        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("fragments")]
        public List<Fragment> Fragments { get; set; }

        [JsonProperty("is_action")]
        public bool IsAction { get; set; }

        [JsonProperty("user_badges", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserBadge> UserBadges { get; set; }

        [JsonProperty("user_color", NullValueHandling = NullValueHandling.Ignore)]
        public string UserColor { get; set; }

        [JsonProperty("emoticons", NullValueHandling = NullValueHandling.Ignore)]
        public List<EmoticonElement> Emoticons { get; set; }
    }

    public class EmoticonElement
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("begin")]
        public long Begin { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }
    }

    public class Fragment
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("emoticon", NullValueHandling = NullValueHandling.Ignore)]
        public FragmentEmoticon Emoticon { get; set; }
    }

    public class FragmentEmoticon
    {
        [JsonProperty("emoticon_id")]
        public string EmoticonId { get; set; }

        [JsonProperty("emoticon_set_id")]
        public string EmoticonSetId { get; set; }
    }

    public class UserBadge
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }

}
