# BirdBot
This is a stripped version of my discord bot solely for posting gifs/images in the chat, more specifically bird gifs (though any gifs/images work)
Follow these steps to get bot setup
1)Create a bot @ https://discordapp.com/developers/applications/me
  -Click "New App"
  -Enter a bot Name and you can add a description and image if you want, otherwise hit create app
2) Create a bot user to represent your bot in the server
  -Scroll down to bot and click create bot user
  -click reveal token, this will be stored in your configs/bot_info.txt next to token
3)Add the bot to your discrord server
  -get the client id of your bot next to Client id: on the same site, add this to the configs/bot_info.txt file 
  -go to https://discordapp.com/oauth2/authorize?client_id=CLIENTID&scope=bot where client id is the client id you just got
  -in the drop down menu select the server(s) you wanna add the bot to
4) Run the bot 
  - To add images just place the image in the images folder and then add the command to the commands.txt file in the format
  -of :commandName images/imageName.extension 
