using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactBot.App.Services
{
    public interface IEmoteCache
    {
        IEmote GetEmote(SocketMessage message, string emoteName);
    }
    public class EmoteCache : IEmoteCache
    {
        static ConcurrentDictionary<string, IEmote> _emoteCache { get; } = new ConcurrentDictionary<string, IEmote>();
        public IEmote GetEmote(SocketMessage message, string emoteName)
        {
            return _emoteCache.GetOrAdd(emoteName, emote =>
            {
                SocketGuild guild = ((SocketGuildChannel)message.Channel).Guild;
                var guildEmote = guild.Emotes.FirstOrDefault(e => e.Name == emoteName);
                if (guildEmote != null)
                {
                    return guildEmote;
                }
                return new Emoji(emote);
            });

        }
    }
}
