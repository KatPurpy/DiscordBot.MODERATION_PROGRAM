using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
namespace DiscordBot.MODERATION_PROGRAM
{
    class Cow : Module
    {
        static string[] purp = 
            string.Join('\n',new string[]
        {
                "WBWWWWWWWWWWWWWWWWWWWBW",
                "WBBWWWWWWWWWWWWWWWWWBBW",
                "WBPBWWWWWWWWWWWWWWWBPBW",
                "WBPPBWWWWWWWWWWWWWBPPBW",
                "WBPPPBWWWWWWWWWWWBPPPBW",
                "WBPRPPBWWWWWWWWWBPPRPBW",
                "WBPRRPPBWWWWWWWBPPRRPBW",
                "WBPPPBBBBBBBBBBBBBPPPBW",
                "WBPPBPPPPPPPPPPPPPBPPBW",
                "WBPBPPPPPPPPPPPPPPPBPBW",
                "WBBPPPBBPPPPPPPBBPPPBBW",
                "WBPPPBPPBPPPPPBPPBPPPBW",
                "WBPPBPPPPBPPPBPPPPBPPBW",
                "BBPPPPPPPPPPPPPPPPPPPBB",
                "BPPPPPPPPPPPPPPPPPPPPPB",
                "BPPPPPPPPPPPPPPPPPPPPPB",
                "BPPPPPPPPPRRRPPPPPPPPPB",
                "BBPPPPPPPPRRRPPPPPPPPBB",
                "WBPPBBPPPPPBPPPPPBBPPBW",
                "WBPPPPBBPPPBPPPBBPPPPBW",
                "WWBPPPPPBBPBPBBPPPPPBWW",
                "WWWBPPPPPPBBBPPPPPPBWWW",
                "WWWWBPPPPPPPPPPPPPBWWWW",
                "WWWWWBBBPPPPPPPBBBWWWWW",
                "WWWWWWWWBBBBBBBWWWWWWWW"
        })
            .Replace("W", "<:p_w:557568572096905227>")
            .Replace("R", "<:p_r:557568572491169821>")
            .Replace("P", "<:p_p:557568571950104609>")
            .Replace("B", "<:p_b:557568571744583682>")
            .Split('\n');

        static Dictionary<string, string> nuts = new Dictionary<string, string>() {
            { "pistachio", "<:pistachio:715538957542227999>" },
            { "cashew", "<:cashew:715538957185712151>" },
            { "almond", "<:almond:715538958225768461>" }
        };
        static Cow()
        {

        }
        
        public override async Task HandleMessage(SocketMessage message)
        {
            string content = message.Content;
            string contentLowerCase = content.ToLowerInvariant();
            
            bool byOwner = message.Author.Id == 403960686847459329;
             if (contentLowerCase.Contains("cow") ||
                contentLowerCase.Contains("meow"))
            {
#pragma warning disable CS4014 
                message.Channel.SendMessageAsync("Meow meow I'm a cow");
#pragma warning restore CS4014 
                return;
            }

            if (contentLowerCase.Contains("beep"))
            {
#pragma warning disable CS4014 
                message.Channel.SendMessageAsync("Beep beep I'm a sheep");
#pragma warning restore CS4014 
                return;
            }

            if(contentLowerCase.Contains("deez nuts"))
            {
#pragma warning disable CS4014
                message.Channel.SendMessageAsync("*dear god*");
#pragma warning restore CS4014 
                return;
            }
            
            if (contentLowerCase.Contains("there") || contentLowerCase.Contains("more"))
            {
#pragma warning disable CS4014
                message.Channel.SendMessageAsync("***no***");
#pragma warning restore CS4014 
                return;
            }

            if (contentLowerCase.StartsWith("print me a frik pls"))
            {
#pragma warning disable CS4014 
                message.Channel.SendMessageAsync("<:frik:726892636769746984>");
#pragma warning restore CS4014 
                return;
            }
            if (byOwner) {
                if (contentLowerCase.Contains("print me cat pls"))
                {
                    print_cat(message, "{0}");
                    return;
                }

                if (contentLowerCase.Contains("print me smol cat pls"))
                {
                    print_cat(message, ".{0}.");
                    return;
                }
            }

            if (contentLowerCase.StartsWith("!give"))
            {
                if (byOwner)
                {
                    string[] args = contentLowerCase.Split(' ');
                    if (nuts.TryGetValue(args[1], out string nut)) {
#pragma warning disable CS4014 
                        Task.Run(async () =>
                        {

                            await message.Channel.SendMessageAsync("Please wait. Brewing nuts...");
                            var typestate = message.Channel.EnterTypingState();
                            await Task.Delay(5500);
                            {
                                int required_nuts = Math.Min(int.Parse(args[2]),100);
                                int nutperline = 12;

                                int linesOfNuts = required_nuts / nutperline;
                                int leftover = required_nuts % nutperline;

                                for (int i = 0; i < linesOfNuts; i++)
                                {
                                    postNuts(nut, nutperline);
                                }

                                postNuts(nut, leftover);

                                void postNuts(string nut, int amount)
                                {
                                    StringBuilder sb = new();
                                    sb.Insert(0, nut, amount);
                                    message.Channel.SendMessageAsync(sb.ToString());
                                }
                            }
                            typestate.Dispose();
                        });
#pragma warning restore CS4014 
                    }
                    else
                    {
#pragma warning disable CS4014 
                        message.Channel.SendMessageAsync("invalid nut type");
#pragma warning restore CS4014 
                    }
                }
                else
                {
#pragma warning disable CS4014 
                    message.Channel.SendFileAsync(message.Author.Mention + ", no.");
#pragma warning restore CS4014 
                }
            }
        }

        static void print_cat(SocketMessage message, string format)
        {
            Task.Run(async () =>
            {
                foreach (var s in purp)
                {
                    await message.Channel.SendMessageAsync(string.Format(format,s));
                }
            });
        }
    }
}
