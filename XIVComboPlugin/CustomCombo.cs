using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Statuses;

namespace XIVComboPlugin.Combos
{
    public delegate ulong OnGetIconDelegate(byte param1, uint param2);

    public abstract partial class CustomCombo
    {
        protected CustomCombo(IClientState clientState, IJobGauges jobGauges)
        {
            this.clientState = clientState;
            this.jobGauges = jobGauges;
        }

        public abstract ulong? Invoke(uint actionID, uint lastComboActionID, float comboTime, Func<uint, ulong> originalHook);

        protected bool HasEffect(short effectID)
        {
            var localPlayer = this.clientState.LocalPlayer;
            if (localPlayer is null)
                return false;

            foreach (var status in localPlayer.StatusList)
            {
                if (status.StatusId == effectID)
                    return true;
            }

            return false;
        }

        protected Status FindEffect(short effectID)
        {
            var localPlayer = this.clientState.LocalPlayer;

            if (localPlayer is null)
                return null;

            foreach (var status in localPlayer.StatusList)
            {
                if (status.StatusId == effectID)
                    return status;
            }

            return null;
        }

        protected T GetJobGauge<T>() where T : JobGaugeBase => this.jobGauges.Get<T>();

        public byte ClassID;
        public byte JobID;
        protected IClientState clientState;
        protected IJobGauges jobGauges;
    }
}
