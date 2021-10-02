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
        private IDiscordClient _discordClient { get; }
        public RemoveLastReactionEventHandler(string reactionToRemove, IDiscordClient discordClient) : this(new[] { reactionToRemove }, discordClient)
        { }

        public RemoveLastReactionEventHandler(IEnumerable<string> reactionsToRemove, IDiscordClient discordClient)
        {
            _reactionsToRemove = new HashSet<string>(reactionsToRemove, StringComparer.OrdinalIgnoreCase);
            _discordClient = discordClient;
        }

        public async Task HandleReactionRemoved(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message.Reactions.TryGetValue(reaction.Emote, out var reactionMetadata))
            {
                if (reactionMetadata.ReactionCount == 1 && reactionMetadata.IsMe && _reactionsToRemove.Contains(reaction.Emote.Name))
                {
                    await message.RemoveReactionAsync(reaction.Emote, _discordClient.CurrentUser);
                }
            }
        }
    }
}
