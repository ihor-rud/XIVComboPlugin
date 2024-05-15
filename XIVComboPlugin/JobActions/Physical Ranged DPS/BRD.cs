using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    static class BRD
    {
        const uint
           QuickNock = 106,
           HeavyShot = 97,
           BurstShot = 16495,
           StraightShot = 98,
           ApexArrow = 16496;

        static class Buffs
        {
            public const short
                StraightShotReady = 122;
        }

        public class Combo : CustomCombo
        {
            public Combo(IClientState clientState, IJobGauges jobGauges) : base(clientState, jobGauges)
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
                    var originalApexArrow = originalHook(BRD.ApexArrow);
                    if (gauge.SoulVoice == 100 || originalApexArrow != BRD.ApexArrow)
                        return originalApexArrow;
                }

                return null;
            }
        }
    }
}
