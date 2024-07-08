using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

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
        Aethercharge = 25800,
        DreadwyrmTrance = 3581,
        SummonBahamut = 7427,
        AstralImpulse = 25820,
        FountainOfFire = 16514,
        UmbralImpulse = 36994,
        EnkindleBahamut = 7429,
        Necrotize = 36990;

    static class Levels
    {
        public const byte
            EnkindleBahamut = 70;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<SMNGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == SMN.Gemshine || actionId == SMN.Ruin1 || actionId == SMN.Ruin2 || actionId == SMN.Ruin3)
            {
                if (GetJobGauge().Attunement != 0)
                    return originalHook(SMN.Gemshine);
            }

            if (actionId == SMN.PreciousBrilliance || actionId == SMN.Outburst || actionId == SMN.TriDisaster)
            {
                if (GetJobGauge().Attunement != 0)
                    return originalHook(SMN.PreciousBrilliance);
            }

            if (actionId == SMN.Aethercharge || actionId == SMN.DreadwyrmTrance || actionId == SMN.SummonBahamut)
            {
                var originalRuin = originalHook(SMN.Ruin1);
                if (originalRuin == SMN.AstralImpulse && level >= Levels.EnkindleBahamut)
                    return originalHook(SMN.EnkindleBahamut);
                if (originalRuin == SMN.FountainOfFire)
                    return originalHook(SMN.EnkindleBahamut);
                if (originalRuin == SMN.UmbralImpulse)
                    return originalHook(SMN.EnkindleBahamut);
            }

            if (actionId == SMN.EnergyDrain || actionId == SMN.Fester || actionId == SMN.Necrotize)
            {
                if (!GetJobGauge().HasAetherflowStacks)
                    return SMN.EnergyDrain;

                return originalHook(SMN.Fester);
            }

            if (actionId == SMN.EnergySiphon || actionId == SMN.Painflare)
            {
                if (!GetJobGauge().HasAetherflowStacks)
                    return SMN.EnergySiphon;

                return SMN.Painflare;
            }

            return null;
        }

        public override byte ClassId => 26;
        public override byte JobId => 27;
    }
}
