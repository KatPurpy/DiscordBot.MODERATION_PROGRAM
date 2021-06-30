using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.MODERATION_PROGRAM
{
    public abstract class Module
    {
        public abstract Task HandleMessage(SocketMessage message);
    }
}
