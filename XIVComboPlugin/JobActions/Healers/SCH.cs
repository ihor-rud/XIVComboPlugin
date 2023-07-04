using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    static class SCH
    {
        const uint
           Physick = 190,
           Adloquium = 185,
           FeyBless = 16543,
           Consolation = 16546;

        static class Levels
        {
            public const byte
                Adloquium = 30;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 15;
                this.JobID = 28;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == SCH.FeyBless)
                {
                    if (GetJobGauge<SCHGauge>().SeraphTimer > 0)
                        return SCH.Consolation;
                }

                if (actionID == SCH.Physick || actionID == SCH.Adloquium)
                {
                    if (level >= Levels.Adloquium)
                        return SCH.Adloquium;

                    return SCH.Physick;
                }

                return null;
            }
        }
    }
}
