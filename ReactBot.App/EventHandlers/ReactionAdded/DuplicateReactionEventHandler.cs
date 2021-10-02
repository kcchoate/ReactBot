using Discord;
using Discord.WebSocket;
using ReactBot.App.EventHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactBot.App.EventHandlers.ReactionAdded
{
    public class DuplicateReactionEventHandler : IReactionAddedEventHandler
    {
        private HashSet<string> _sourceReactionNames { get; }
        public DuplicateReactionEventHandler(string sourceReactionName) : this(new[] { sourceReactionName })
        { }

        public DuplicateReactionEventHandler(IEnumerable<string> sourceReactionNames)
        {
            _sourceReactionNames = new HashSet<string>(sourceReactionNames, StringComparer.OrdinalIgnoreCase);
        }

        public async Task HandleReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.Message.IsSpecified 
                && !reaction.Message.Value.Author.IsBot
                && _sourceReactionNames.Contains(reaction.Emote.Name))
            {
                await reaction.Message.Value.AddReactionAsync(reaction.Emote);
            }
        }
    }
}
