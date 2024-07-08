using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

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

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<BRDGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            if (actionId == BRD.HeavyShot || actionId == BRD.BurstShot)
            {
                if (HasEffect(Buffs.StraightShotReady))
                    return originalHook(BRD.StraightShot);
            }

            if (actionId == BRD.QuickNock)
            {
                var gauge = GetJobGauge();
                var originalApexArrow = originalHook(BRD.ApexArrow);
                if (gauge.SoulVoice == 100 || originalApexArrow != BRD.ApexArrow)
                    return originalApexArrow;
            }

            return null;
        }

        public override byte ClassId => 5;
        public override byte JobId => 23;
    }
}
