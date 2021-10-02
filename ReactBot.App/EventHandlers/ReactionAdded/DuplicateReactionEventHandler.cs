using Discord;
using Discord.WebSocket;
using ReactBot.App.EventHandlers.Interfaces;
using ReactBot.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactBot.App.EventHandlers.ReactionAdded
{
    public class DuplicateReactionEventHandler : IReactionAddedEventHandler
    {
        private HashSet<string> _emotesToDupliate { get; }
        public DuplicateReactionEventHandler(string emoteToDuplicate) : this(new[] { emoteToDuplicate })
        { }

        public DuplicateReactionEventHandler(IEnumerable<string> emotesToDuplicate)
        {
            _emotesToDupliate = new HashSet<string>(emotesToDuplicate, StringComparer.OrdinalIgnoreCase);
        }

        public async Task HandleReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if (BotReactionCalculator.ShouldBotReactToMessage(message, reaction.Emote))
            {
                await message.AddReactionAsync(reaction.Emote);
            }
        }
    }
}
