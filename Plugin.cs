using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using PlayerStatsSystem;
using RemoteAdmin;
using YamlDotNet.Core.Tokens;
using PluginAPI.Core.Factories;
using static PlayerStatsSystem.DamageHandlerBase;

namespace OmegaWarhead
{
    public class Plugin
    {
        [PluginConfig]
        public Config PluginConfig;

        private static CoroutineHandle _coroutineHandle;
        private static bool _omegaWarhead;

        [PluginEntryPoint("OmegaWarhead", "1.1.0", "Destory the whole facility", "Misaka_ZeroTwo")]
        void LoadPlugin()
        {
            PluginAPI.Events.EventManager.RegisterEvents(this);
        }

        [PluginEvent(ServerEventType.RoundStart)]
        void OnRoundStart()
        {
            _coroutineHandle = Timing.RunCoroutine(enumerator());
        }

        [PluginEvent(ServerEventType.RoundEnd)]
        void OnRoundEnd()
        {
            Timing.KillCoroutines(_coroutineHandle);
        }

        [PluginEvent(ServerEventType.WarheadDetonation)]
        void OnWarheadDetonated()
        {
            if (_omegaWarhead)
            {
                foreach (Player hub in Player.GetPlayers<Player>())
                {
                    hub.ReferenceHub.playerStats.KillPlayer(new CustomReasonDamageHandler(PluginConfig.OmegaWarheadDeathReason, -1f));
                }
            }
        }

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        void OnWaitingForPlayers()
        {
            _omegaWarhead = false;
        }

        IEnumerator<float> enumerator()
        {
            yield return Timing.WaitForSeconds(PluginConfig.MinutesTillOmegaWarhead * 60);
            AlphaWarheadController.Singleton.InstantPrepare();
            AlphaWarheadController.Singleton.StartDetonation(false);
            AlphaWarheadController.Singleton.IsLocked = true;
            _omegaWarhead = true;
            Server.SendBroadcast(PluginConfig.OmegaWarheadAnnouncement, PluginConfig.OmegaWarheadAnnouncementDuration);
            yield break;
        }
    }
}
