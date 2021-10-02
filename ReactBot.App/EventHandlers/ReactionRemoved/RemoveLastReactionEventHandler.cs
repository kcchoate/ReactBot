using Discord;
using Discord.WebSocket;
using ReactBot.App.EventHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactBot.App.EventHandlers.ReactionRemoved
{
    public class RemoveLastReactionEventHandler : IReactionRemovedEventHandler
    {
        private HashSet<string> _reactionsToRemove { get; }
        private IUser _targetUser { get; }
        public RemoveLastReactionEventHandler(string reactionToRemove, IUser targetUser) : this(new[] { reactionToRemove }, targetUser)
        { }

        public RemoveLastReactionEventHandler(IEnumerable<string> reactionsToRemove, IUser targetUser)
        {
            _reactionsToRemove = new HashSet<string>(reactionsToRemove, StringComparer.OrdinalIgnoreCase);
            _targetUser = targetUser;
        }

        public async Task HandleReactionRemoved(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message.Reactions.TryGetValue(reaction.Emote, out var reactionMetadata))
            {
                if (reactionMetadata.ReactionCount == 1 && reactionMetadata.IsMe && _reactionsToRemove.Contains(reaction.Emote.Name))
                {
                    await message.RemoveReactionAsync(reaction.Emote, _targetUser);
                }
            }
        }
    }
}
