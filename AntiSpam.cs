using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.MODERATION_PROGRAM
{
    class AntiSpam : Module
    {
        class UserInfo
        {
            public DateTimeOffset lastMessage;
            public int postedMessages;
            public bool warned;
        }
        static TimeSpan timethreshold = TimeSpan.FromSeconds(3.5f);

        const int warn_threshold = 8;
        const int ban_threshold = 10;

        Dictionary<ulong, UserInfo> tr = new Dictionary<ulong, UserInfo>();

        public override async Task HandleMessage(SocketMessage message)
        {
            var id = message.Author.Id;
            var msgTime = message.Timestamp;
            if (tr.TryGetValue(id, out UserInfo info))
            {
                var time = (msgTime - info.lastMessage);
                if (time < timethreshold)
                {
                    info.lastMessage = msgTime;
                    info.postedMessages++;
                    
                    if(info.postedMessages >= warn_threshold)
                    {
                        if(info.postedMessages >= ban_threshold)
                        {
                            message.Channel.SendMessageAsync(message.Author.Mention + ", you are supposed to be banned");
                            return;
                        }
                        if (!info.warned)
                        {
                            message.Channel.SendMessageAsync(message.Author.Mention + ", stop spamming tx");
                            info.warned = true;
                        }
                        return;
                    }

                }
                else
                {
                    info.warned = false;
                    info.postedMessages = 0;
                    info.lastMessage = msgTime;
                }
            }
            else
            {
                tr.Add(id, new UserInfo()
                {
                    lastMessage = msgTime,
                    postedMessages = 1
                });
            }
        }
    }
}
