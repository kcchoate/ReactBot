using Discord.WebSocket;
using ReactBot.App.EventHandlers.Interfaces;
using ReactBot.App.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReactBot.App.EventHandlers.MessageReceived
{
    public class AddReactionToUserNameEventHandler : IMessageReceivedEventHandler
    {
        private string _userName { get; }
        private string _reactionEmoteName { get; }
        private IEmoteCache _emoteCache { get; }
        public AddReactionToUserNameEventHandler(string userName, string reactionEmoteName, IEmoteCache emoteCache)
        {
            _userName = userName;
            _reactionEmoteName = reactionEmoteName;
            _emoteCache = emoteCache;
        }
        public async Task HandleReceivedMessage(SocketMessage message)
        {
            var userName = ((SocketGuildUser)message.Author).Username;
            if (string.Equals(_userName, userName, StringComparison.OrdinalIgnoreCase))
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
