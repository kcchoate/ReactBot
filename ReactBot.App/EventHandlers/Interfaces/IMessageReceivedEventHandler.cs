using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReactBot.App.EventHandlers.Interfaces
{
    public interface IMessageReceivedEventHandler
    {
        Task HandleReceivedMessage(SocketMessage message);
    }
}
