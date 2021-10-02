using Discord.WebSocket;
using ReactBot.App.EventHandlers.Interfaces;
using ReactBot.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactBot.App.EventHandlers.MessageReceived
{
    public class AddReactionToKeywordMatchEventHandler : IMessageReceivedEventHandler
    {
        private IEnumerable<string> _keywords { get; }
        private IEmoteCache _emoteCache { get; }
        private string _reactionEmoteName { get; }
        public AddReactionToKeywordMatchEventHandler(string reactionEmoteName, IEnumerable<string> keywords, IEmoteCache emoteCache)
        {
            _reactionEmoteName = reactionEmoteName;
            _keywords = keywords;
            _emoteCache = emoteCache;
        }
        public async Task HandleReceivedMessage(SocketMessage message)
        {
            if (_keywords.Intersect(message.Content.Split(" ", StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase).Any())
            {
                var reactionEmote = _emoteCache.GetEmote(message, _reactionEmoteName);
                if (reactionEmote != null && BotReactionCalculator.ShouldBotReactToMessage(message, reactionEmote))
                {
                    await message.AddReactionAsync(reactionEmote);
                }
            }
        }
    }
}
