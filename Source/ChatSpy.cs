using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace ChatSpy;

public class ChatSpy : BasePlugin
{
    public override string ModuleName => "Chat Spy";
    public override string ModuleVersion => "1.0.7";
    public override string ModuleAuthor => "skaen";
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
                    CsTeam.Terrorist => "Terrorist",
                    CsTeam.CounterTerrorist => "Counter-Terrorist",
                    CsTeam.Spectator => "SPEC",
                    _ => "NONE"
                };

                var playerEntities = Utilities.GetPlayers();
                foreach (var admin in playerEntities)
                {
                    if (AdminManager.PlayerHasPermissions(admin, "@css/chat"))
                    {
                        if (player == admin) continue;
                        if (player.TeamNum != admin.TeamNum)
                        {
                            admin.PrintToChat($" {ChatColors.Grey}{isPlayerAlive} ({team}) {player.PlayerName} : {@event.Text}");
                        }
                    }
                }
                return HookResult.Continue;
            }
            return HookResult.Continue;
        }));
    }
}
