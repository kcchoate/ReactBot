using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactBot.App.Services
{
    public class BotReactionCalculator
    {
        public static bool ShouldBotReactToMessage(IUserMessage message, IEmote emote)
        {
            var doesReactionAlreadyExist = message.Reactions.TryGetValue(emote, out var reactionMetadata);
            return !doesReactionAlreadyExist || (doesReactionAlreadyExist && !reactionMetadata.IsMe);
        }
        public static bool ShouldBotReactToMessage(SocketMessage message, IEmote emote)
        {
            var doesReactionAlreadyExist = message.Reactions.TryGetValue(emote, out var reactionMetadata);
            return !doesReactionAlreadyExist || (doesReactionAlreadyExist && !reactionMetadata.IsMe);
        }
    }
}
