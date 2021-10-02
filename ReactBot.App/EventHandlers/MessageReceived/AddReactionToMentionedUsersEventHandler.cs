using Discord.WebSocket;
using ReactBot.App.EventHandlers.Interfaces;
using ReactBot.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactBot.App.EventHandlers.MessageReceived
{
    public class AddReactionToMentionedUsersEventHandler : IMessageReceivedEventHandler
    {
        private string _reactEmoteName { get; }
        private HashSet<string> _mentionedUsersNames { get; }
        private IEmoteCache _emoteCache { get; }

        public AddReactionToMentionedUsersEventHandler(string reactEmoteName, string userName, IEmoteCache emoteCache)
            : this(reactEmoteName, new[] { userName }, emoteCache)
        { }

        public AddReactionToMentionedUsersEventHandler(string reactEmoteName, IEnumerable<string> userNames, IEmoteCache emoteCache)
        {
            _reactEmoteName = reactEmoteName;
            _mentionedUsersNames = new HashSet<string>(userNames);
            _emoteCache = emoteCache;
        }

        public async Task HandleReceivedMessage(SocketMessage message)
        {
            if (message.MentionedUsers.Select(x => x.Username).Intersect(_mentionedUsersNames, StringComparer.OrdinalIgnoreCase).Any())
            {
                var reactionEmote = _emoteCache.GetEmote(message, _reactEmoteName);
                if (reactionEmote != null && !message.Reactions[reactionEmote].IsMe)
                {
                    await message.AddReactionAsync(reactionEmote);
                }
            }
        }
    }
}
