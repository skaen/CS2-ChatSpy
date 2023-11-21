using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using System.Text.Json.Serialization;

namespace ChatSpy;
public class ConfigGen : BasePluginConfig
{
    [JsonPropertyName("OnlySpectators")]
    public bool OnlySpectators { get; set; } = false;
    [JsonPropertyName("AdminFlag")]
    public string AdminFlag { get; set; } = "@css/chat";
    [JsonPropertyName("ColorT")]
    public string ColorT { get; set; } = "{OLIVE}";
    [JsonPropertyName("ColorCT")]
    public string ColorCT { get; set; } = "{BLUE}";
    [JsonPropertyName("ColorSPEC")]
    public string ColorSPEC { get; set; } = "{GRAY}";
}
public class ChatSpy : BasePlugin, IPluginConfig<ConfigGen>
{
    public override string ModuleName => "Chat Spy";
    public override string ModuleVersion => "1.0.8";
    public override string ModuleAuthor => "skaen";
    public required ConfigGen Config { get; set; }
    public void OnConfigParsed(ConfigGen config)
    {
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventPlayerChat>(((@event, info) =>
        {
            var message = @event.Text.Trim();
            var player = Utilities.GetPlayerFromUserid(@event.Userid);

            if (player == null || !player.IsValid || player.IsBot || message == null)
                return HookResult.Continue;

            if (@event.Teamonly)
            {
                var isPlayerAlive = player.PawnIsAlive ? "" : "*DEAD*";
                var team = (CsTeam)player.TeamNum switch
                {
                    CsTeam.Terrorist => $"{Config.ColorT}(Terrorist)",
                    CsTeam.CounterTerrorist => $"{Config.ColorCT}(Counter-Terrorist)",
                    CsTeam.Spectator => $"{Config.ColorSPEC}(SPEC)",
                    _ => "NONE"
                };

                var playerEntities = Utilities.GetPlayers();
                foreach (var admin in playerEntities)
                {
                    if (AdminManager.PlayerHasPermissions(admin, Config.AdminFlag))
                    {
                        if (player == admin) continue;
                        if (Config.OnlySpectators && (CsTeam)player.TeamNum != CsTeam.Spectator) continue;
                        if (player.TeamNum != admin.TeamNum)
                        {
                            var messageTeamColor = ReplaceColorTags(team);
                            admin.PrintToChat($" {ChatColors.Grey}{isPlayerAlive} {messageTeamColor} {player.PlayerName} : {@event.Text}");
                        }
                    }
                }
                return HookResult.Continue;
            }
            return HookResult.Continue;
        }));
    }
    private string ReplaceColorTags(string input)
    {
        string[] colorPatterns =
        {
                "{DEFAULT}", "{RED}", "{LIGHTPURPLE}", "{GREEN}", "{LIME}", "{LIGHTGREEN}", "{LIGHTRED}", "{GRAY}",
                "{LIGHTOLIVE}", "{OLIVE}", "{LIGHTBLUE}", "{BLUE}", "{PURPLE}", "{GRAYBLUE}"
            };
        string[] colorReplacements =
        {
                "\x01", "\x02", "\x03", "\x04", "\x05", "\x06", "\x07", "\x08", "\x09", "\x10", "\x0B", "\x0C", "\x0E",
                "\x0A"
            };

        for (var i = 0; i < colorPatterns.Length; i++)
            input = input.Replace(colorPatterns[i], colorReplacements[i]);

        return input;
    }
}
