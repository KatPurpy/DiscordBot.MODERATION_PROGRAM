using System;
using System.IO;
using System.Threading.Tasks;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.MODERATION_PROGRAM
{
    class Program
    {
        static DiscordSocketClient client;
        static CommandService commands;

        static Module[] handles = new Module[]
        {
            new Cow(),
            new AntiSpam()
        };

        static void Main(string[] args)
        {
            new Program().MainTask().GetAwaiter
                ().GetResult();
        }

        public async Task MainTask()
        {
            client = new DiscordSocketClient();
            Console.WriteLine("START");

            string token = System.Text.Json.JsonDocument.Parse(
                    File.ReadAllText("creds.json")).RootElement.GetProperty("token").GetString();
            
            client.Log += Client_Log;

            client.Ready += () =>
            {
                Console.WriteLine("Successfully logged in!");
                return Task.CompletedTask;
            };

            client.MessageReceived += Client_MessageReceived;

            await client.LoginAsync(Discord.TokenType.Bot,
                token
                );
            await client.StartAsync();

            

           

            await spin();
            await client.LogoutAsync();
            await Task.Delay(-1);
        }

        async Task spin()
        {
            var anim = new[]{ '-', '\\', '|', '/' };
            var statuses = new[]
            {
                Discord.UserStatus.Online,
                Discord.UserStatus.Idle,
                Discord.UserStatus.DoNotDisturb
            };
            int a=0,b = 0;
            while (true)
            {
                await client.SetGameAsync(anim[a++%anim.Length].ToString(), null, Discord.ActivityType.Watching);
                await client.SetStatusAsync(statuses[b++%statuses.Length]);
                await Task.Delay(6000);
            }
        }

        private async Task Client_Log(Discord.LogMessage arg)
        {
            Console.WriteLine(arg);
            await Task.CompletedTask;
        }

        private static async Task Client_MessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null) return;

            Console.WriteLine(message.Content);
            bool msg_deleted = false;
            if (message.Author.Id != client.CurrentUser.Id)
            {
                foreach (var module in handles)
                {
                    if (msg_deleted) break;
                    await module.HandleMessage(arg);
                }
            }
        }
    }
}
