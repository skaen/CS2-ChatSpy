# CS2-ChatSpy
  The plugin catches the enemy team's message and sends them to the administrators with the @css/chat flag

# Installation
1. Install [CounterStrike Sharp](https://github.com/roflmuffin/CounterStrikeSharp) and [Metamod:Source](https://www.sourcemm.net/downloads.php/?branch=master)
3. Download [CS2-ChatSpy](https://github.com/skaen/CS2-ChatSpy/releases)
4. Unzip the archive and upload it to the game server

# Config
Grant `@css/chat` rights to Administrators who will see messages in the config:
```addons/counterstrikesharp/configs/admins.json```

Configure the config: `addons/counter strike sharp/configs/ChatSpy`
1. `OnlySpectators`: true/false - 
  `true` - outputs a message to Administrators who are in the spectators, `false` - outputs to Administrators in any command
2. `AdminFlag`: @cs/flag - Administrator flag for wiretapping
3. `ColorT`: {Color} - the color of messages from Terrorists
4. `ColorCT`: {Color} - color of messages from Counter-Terrorists
5. `ColorSPEC`: {Color} - the color of messages from the Specters
