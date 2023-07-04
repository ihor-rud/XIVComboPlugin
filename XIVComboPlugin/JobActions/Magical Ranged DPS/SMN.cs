using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    static class SMN
    {
        const uint
           Ruin1 = 163,
           Ruin2 = 172,
           Ruin3 = 3579,
           Outburst = 16511,
           TriDisaster = 25826,
           EnergyDrain = 16508,
           EnergySiphon = 16510,
           Fester = 181,
           Painflare = 3578,
           Gemshine = 25883,
           PreciousBrilliance = 25884,
           SummonBahamut = 7427,
           SummonPhoenix = 25800,
           AstralImpulse = 25820,
           FountainOfFire = 16514,
           EnkindleBahamut = 7429;

        static class Levels
        {
            public const byte
                PreciousBrilliance = 26,
                Painflare = 40,
                SummonBahamut = 70;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 26;
                this.JobID = 27;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == SMN.Ruin1 || actionID == SMN.Ruin2 || actionID == SMN.Ruin3)
                {
                    if (GetJobGauge<SMNGauge>().Attunement != 0)
                    {
                        return originalHook(Gemshine);
                    }
                }

                if (actionID == SMN.Outburst || actionID == SMN.TriDisaster)
                {
                    if (level >= Levels.PreciousBrilliance && GetJobGauge<SMNGauge>().Attunement != 0)
                    {
                        return originalHook(PreciousBrilliance);
                    }
                }

                if (actionID == SMN.SummonBahamut || actionID == SMN.SummonPhoenix)
                {
                    var original = originalHook(SMN.Ruin1);
                    if (original == SMN.AstralImpulse && level >= Levels.SummonBahamut)
                        return originalHook(SMN.EnkindleBahamut);
                    if (original == SMN.FountainOfFire)
                        return originalHook(SMN.EnkindleBahamut);
                }

                if (actionID == SMN.Fester)
                {
                    if (!GetJobGauge<SMNGauge>().HasAetherflowStacks)
                        return SMN.EnergyDrain;
                }

                if (actionID == SMN.Painflare)
                {
                    if (!GetJobGauge<SMNGauge>().HasAetherflowStacks)
                        return SMN.EnergySiphon;

                    if (level >= Levels.Painflare)
                        return SMN.Painflare;

                    return SMN.EnergySiphon;
                }

                return null;
            }
        }
    }
}
