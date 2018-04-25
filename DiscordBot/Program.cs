using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot {
    internal static class Program {
        private const string Path = "token.txt";
        private static void Main(string[] args) { MainAsync(args).GetAwaiter().GetResult();} //The true main, but we want it run async, so we only call OUR version of main

        private static async Task MainAsync(string[] args) {
            var client = new DiscordSocketClient();
            SocketTextChannel defaultChannel = null;

            Random randomQuote = new Random();
            string[] loginQuotes = { "It's time to kick ass and chew bubble gum", "Your face, your ass - what's the difference?", "Hail to The King Baby", "Let's Rock", "Shake it baby!" };
            //End setup for stupid one liner login quotes

            client.Log += message => { //Logging ready status to whoever is running the client
                Console.WriteLine(message);
                return Task.CompletedTask;
            };
            //Setup stuff
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var commandService = new CommandService();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());

            client.MessageReceived += async rawMessage => {//MessageReceived..so we need to go in here and figure out what to do
                if (!(rawMessage is SocketUserMessage message))  return;//Message not from user, or some form of error
                SocketGuild guild = ((SocketGuildChannel)message.Channel).Guild; //Gets the server
                defaultChannel = guild.DefaultChannel; //Default Channel

                //Listen for any praising
                if (message.Content.Contains("praisesun") || message.Content.Contains("<:praisesun:372813946052280341>") && !(message.Content.Contains("STOP"))) { //Listen for praise sun stuff
                    await message.Channel.SendMessageAsync("<:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341>\n" +
                        "<:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341>\n" +
                        "<:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341>\n");
                }

                var argPos = 0; //Set argPos for use
                if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;

                var context = new CommandContext(client, message);
                var result = await commandService.ExecuteAsync(context, argPos, serviceProvider);
                if (!result.IsSuccess) //Error Logging
                    Console.WriteLine("Failed to run command: {0}", result.ErrorReason);
            };
           
            var token = File.ReadAllText(Path); //Reads the bots token so it can connect
            await client.LoginAsync(TokenType.Bot, token); //Logs bot in
            await client.StartAsync(); //Starts bot

            client.Ready += async () => { //Bot is ready, send duke nukem quote Random quote between 0-4
                String messageToAnnoy = loginQuotes[randomQuote.Next(0, 4)];
                await defaultChannel.SendMessageAsync(messageToAnnoy);
                //await message.Channel.SendMessageAsync(messageToAnnoy);
            };
            await Task.Delay(-1); //Keeps bot running
        }
    }
}