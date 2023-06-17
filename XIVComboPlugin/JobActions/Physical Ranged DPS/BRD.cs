using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    public static class BRD
    {
        public const uint
            QuickNock = 106,
            HeavyShot = 97,
            BurstShot = 16495,
            StraightShot = 98,
            ApexArrow = 16496;

        public static class Buffs
        {
            public const short
                StraightShotReady = 122;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 5;
                this.JobID = 23;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                if (actionID == BRD.HeavyShot || actionID == BRD.BurstShot)
                {
                    if (HasEffect(Buffs.StraightShotReady))
                        return originalHook(BRD.StraightShot);
                }

                if (actionID == BRD.QuickNock)
                {
                    var gauge = GetJobGauge<BRDGauge>();
                    var x = originalHook(BRD.ApexArrow);
                    if (gauge.SoulVoice == 100 || x != BRD.ApexArrow)
                        return x;
                }

                return null;
            }
        }
    }
}
