using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Threading;

namespace ConsoleApplication16
{
    class main
    {
        public static void Main(string[] args)
                => new main().MainAsync().GetAwaiter().GetResult();
        private DiscordSocketClient client;
        string token ="";
        string userId="";
        public async Task MainAsync()
        {
            try
            {
                client = new DiscordSocketClient();
                await getBotInfo();

                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
                if(client.LoginState == LoginState.LoggedIn)
                    Console.WriteLine("Bot Running!");
                client.MessageReceived += Bot_MessageReceived;
                // Block this task until the program is closed.
                await Task.Delay(-1);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(-1);
            }
        }
        private async Task Bot_MessageReceived(SocketMessage message)
        {
            var m = message as SocketUserMessage;
            if (m == null || m.Author.Id == Convert.ToUInt64(userId))
                return;
            string[] words = System.IO.File.ReadAllLines("configs/commands.txt");

            //help command displays help information for users, the information is read from the file commands.txt which is stored in the config folder for the bot
            if (message.Content.ToString().ToLower().Contains(":halp"))
            {
                help(message.Channel, words);
                Console.WriteLine("help called");
            }//end halp
            else if(message.Content.ToString().Contains(":"))
            {
                try
                {
                    //message in all lowercase
                    string lmessage = message.Content.ToLower();
                    string temp = "";
                    string tempF = "";
                    int index = 0;
                    int indexSpace = 0;
                    bool status = true;
                    //this is used to make sure inputting multiple images in one message will work
                    while (status)
                    {
                        //read in the list of gifs and their locations
                        string[] lines = System.IO.File.ReadAllLines("configs/commands.txt");

                        //find the index of the colon to be used in parsing
                        index = lmessage.IndexOf(":");

                        if (index >= 0)
                        {
                            //remove colon from the message
                            lmessage = lmessage.Substring(index + 1);
                            //locate if there is a space or not to see if there are multiple gif requests in one message
                            indexSpace = lmessage.IndexOf(" ");

                            //if a space was found set temp to the part up the space so if lmessage is "parrot :partyparrot"
                            //temp will become just "parrot"
                            if (indexSpace >= 0)
                            {
                                temp = lmessage.Substring(0, indexSpace);

                            }
                            //otherwise only one image was requested presumably and thus just set temp to lmessage
                            else
                            {
                                temp = lmessage;
                                status = false;
                            }

                            //parse the config file and look for the corresponding image requested
                            foreach (string line in lines)
                            {
                                //since the file is formatted as "imageName imageLocation" we need to parse the string so we only have "imageName"
                                tempF = line.Substring(1, line.IndexOf(" ") - 1);
                                //check to see if the requested image name matches the current string pulled from the file
                                if (temp == tempF)
                                {
                                    //if it matches parse the image location from the filestring
                                    string ifile = line.Substring(line.IndexOf(" ") + 1);//image file name is pulled from config.txt

                                    //then send the fill to the chat
                                    await message.Channel.SendFileAsync(ifile);
                                }
                            }//end of for loop
                            status = false;
                            lines = null;
                        }
                        else
                        {
                            status = false;
                            continue;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("not a command");//string parsing dun messed up
                }
            }
        }
        /*function essentially sends a message to the chat listing all the available images in the config file. Function takes a 
            ISocketMessageChannel which should be the channel the user sent the message from (given by message.Channel) and an array
            of strings which should be in the format of {":ultrafastparrot", ":parrot", ...} which should be parsed from the config file
        */
        public async void help(ISocketMessageChannel channel, string[] words)
        {

            string help = "```Available emojis (example usage-> :angelparrot) :\n";
            int counter = 0;
            
            foreach (string word in words)
            {
                counter++;
                string temp = word.Substring(1, word.IndexOf(" ") - 1) + ", ";
                help += temp;
                if (counter % 4 == 0)
                    help += "\n";
            }//end of for loop
            help = help.Remove(help.Length - 2, 1);
            help = help.Insert(help.Length - 1, ".```");
            System.Threading.Thread.Sleep(150);
            await channel.SendMessageAsync(help);
        }
        /*
         Function retrieves bot info from the bot_info.txt file in the configs folder, function takes no arguments and modifies two global variables
         token and userId. Program will exit if the file fails to find both values in the file or if the values are unmodified. 
         */
        public async Task getBotInfo()
        {
            string[] lines = System.IO.File.ReadAllLines("configs/bot_info.txt");
            try
            {

                foreach (string line in lines)
                {
                    if (line.Contains("#"))
                        continue;
                    else if (line.Contains("token:"))
                    {
                        int index = line.IndexOf(":") + 1;
                        token = line.Substring(index, 59);

                    }
                    else if (line.Contains("userId:"))
                    {
                        int index = line.IndexOf(":") + 1;
                        userId = line.Substring(index, 18);
                    }
                }
                if (token == "" || userId == "")
                {
                    System.Console.WriteLine("Failed to parse bot_info.txt please make sure it exist, is in the config folder, and follows the specified format");
                    Environment.Exit(-1);
                }
                else if (token.Contains("#") || userId.Contains("#"))
                {
                    System.Console.WriteLine("bot_info.txt seems to hold the incorrect info. Please go to configs folder and edit it");
                    Environment.Exit(-1);
                }
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}