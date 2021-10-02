using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using ReactBot.App.EventHandlers.MessageReceived;
using ReactBot.App.EventHandlers.ReactionAdded;
using ReactBot.App.EventHandlers.ReactionRemoved;
using ReactBot.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactBot.App
{
    class Program
    {
        private static DiscordSocketClient _client { get; }  = new DiscordSocketClient();
        static async Task Main(string[] args)
        {
            AddMessageReceivedEventHandlers();
            AddReactionAddedEventHandlers();
            AddReactionRemovedEventHandlers();

            await LoginClientAsync();
            await StartClientAsync();

            // Prevent Main from returning, otherwise the app would close as soon as it finished setting up.
            await Task.Delay(-1);
        }

        private static void AddMessageReceivedEventHandlers()
        {

            // TODO: push this setup into config. Once that's done we can iterate of the items which implement
            // IMessageReceivedEventHandler and configure the client using the results.
            var ghostEmote = "\U0001F47B";
            var emoteCache = new EmoteCache();
            var authorRoleHandler = new AddReactionToAuthorRoleEventHandler("Cheesey Pal", "peepoCheese", emoteCache);
            var userNameHandler = new AddReactionToUserNameEventHandler("GOING GHOST", ghostEmote, emoteCache);
            var keywordHandler = new AddReactionToKeywordMatchEventHandler(ghostEmote, new[] { "ghost" }, emoteCache);
            var mentionedUserHandler = new AddReactionToMentionedUsersEventHandler(ghostEmote, "GOING GHOST", emoteCache);

            _client.MessageReceived += authorRoleHandler.HandleReceivedMessage;
            _client.MessageReceived += userNameHandler.HandleReceivedMessage;
            _client.MessageReceived += keywordHandler.HandleReceivedMessage;
            _client.MessageReceived += mentionedUserHandler.HandleReceivedMessage;

        }

        private static void AddReactionAddedEventHandlers()
        {
            var duplicateReactionHandler = new DuplicateReactionEventHandler("shutupbitch");

            _client.ReactionAdded += duplicateReactionHandler.HandleReactionAdded;
        }

        private static void AddReactionRemovedEventHandlers()
        {
            var removeLastReactionHandler = new RemoveLastReactionEventHandler("shutupbitch", _client.CurrentUser);

            _client.ReactionRemoved += removeLastReactionHandler.HandleReactionRemoved;
        }

        private static async Task LoginClientAsync()
        {
            var tokenRetriever = new DiscordClientTokenRetriever();
            var token = await tokenRetriever.GetTokenAsync();
            await _client.LoginAsync(TokenType.Bot, token);
        }

        private static Task StartClientAsync() => _client.StartAsync();
    }
}
