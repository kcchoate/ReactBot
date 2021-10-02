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
    public class AddReactionToAuthorRoleEventHandler : IMessageReceivedEventHandler
    {
        private string _authorRoleName { get; }
        private string _reactionEmoteName { get; }
        private IEmoteCache _emoteCache { get; }
        public AddReactionToAuthorRoleEventHandler(string authorRoleName, string reactionEmoteName, IEmoteCache emoteCache)
        {
            _authorRoleName = authorRoleName;
            _reactionEmoteName = reactionEmoteName;
            _emoteCache = emoteCache;
        }

        public async Task HandleReceivedMessage(SocketMessage message)
        {
            var userRoles = ((SocketGuildUser)message.Author).Roles;
            if (userRoles.Any(x => string.Equals(_authorRoleName, x.Name, StringComparison.OrdinalIgnoreCase)))
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
