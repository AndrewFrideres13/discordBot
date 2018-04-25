using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot {
    public class InfoModule : ModuleBase {
        const String atAllString = "<@438507190349987843>";
        SocketMessage message = null;    

        [Command("command"), Summary("Let there be Commandments")]
        public async Task Command(SocketMessage msg) {
            String message;
            message = msg.ToString();
            //Delete old message
            await msg.DeleteAsync();
            //Call out everyone and send message via our bot
            await ReplyAsync(atAllString + " " + message);
        }

        //Basic structure of what a command looks like, no parameters here
        [Command("test1234"), Summary("Test Command")]
        public async Task Test() {
            await ReplyAsync("It's a test to put the fear of the lord into ya filthy heathens, waddya expect?");
        }

        [Command("pts"), Summary("Praises the sun")]
        [RequireContext(ContextType.Guild)]
        public async Task Praise(){
            //If only I could be so grossly incandescent
            await ReplyAsync("<:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341>\n" +
                "<:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341>\n" +
                "<:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341><:praisesun:372813946052280341>\n");
        }
    } //End class
} //End namespace