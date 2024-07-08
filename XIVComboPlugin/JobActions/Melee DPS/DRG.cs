using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class DRG
{
    const uint
       TrueThrust = 75,
       VorpalThrust = 78,
       Disembowel = 87,
       FullThrust = 84,
       HeavensThrust = 25771,
       ChaosThrust = 88,
       ChaoticSpring = 25772,
       FangAndClaw = 3554,
       WheelingThrust = 3556,
       RaidenThrust = 16479,
       DoomSpike = 86,
       DraconianFury = 25770,
       SonicThrust = 7397,
       CoerthanTorment = 16477,
       WyrmwindThrust = 25773,
       LanceBarrage = 36954,
       SpiralBlow = 36955;

    static class Levels
    {
        public const byte
            VorpalThrust = 4,
            Disembowel = 18,
            FullThrust = 26,
            ChaosThrust = 50,
            SonicThrust = 62,
            Drakesbane = 64,
            FangAndClaw = 56,
            CoerthanTorment = 72;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<DRGGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            // 1 branch single target combo
            if (actionId == DRG.FullThrust || actionId == DRG.HeavensThrust)
            {
                var gauge = GetJobGauge();
                if (gauge.FirstmindsFocusCount == 2)
                    return DRG.WyrmwindThrust;

                if ((lastMove == DRG.TrueThrust || lastMove == DRG.RaidenThrust) && level >= Levels.VorpalThrust)
                    return originalHook(DRG.VorpalThrust);

                if ((lastMove == DRG.VorpalThrust || lastMove == DRG.LanceBarrage) && level >= Levels.FullThrust)
                    return originalHook(DRG.FullThrust);

                if ((lastMove == DRG.FullThrust || lastMove == DRG.HeavensThrust) && level >= Levels.FangAndClaw)
                    return DRG.FangAndClaw;

                if (lastMove == DRG.FangAndClaw && level >= Levels.Drakesbane)
                    return originalHook(DRG.WheelingThrust);

                return originalHook(DRG.TrueThrust);
            }

            // 2 branch single target combo
            if (actionId == DRG.ChaosThrust || actionId == DRG.ChaoticSpring)
            {
                var gauge = GetJobGauge();
                if (gauge.FirstmindsFocusCount == 2)
                    return DRG.WyrmwindThrust;

                if ((lastMove == DRG.TrueThrust || lastMove == DRG.RaidenThrust) && level >= Levels.Disembowel)
                    return originalHook(DRG.Disembowel);

                if ((lastMove == DRG.Disembowel || lastMove == DRG.SpiralBlow) && level >= Levels.ChaosThrust)
                    return originalHook(DRG.ChaosThrust);

                if ((lastMove == DRG.ChaosThrust || lastMove == DRG.ChaoticSpring) && level >= Levels.FangAndClaw)
                    return DRG.WheelingThrust;

                if (lastMove == DRG.WheelingThrust && level >= Levels.Drakesbane)
                    return originalHook(DRG.FangAndClaw);

                return originalHook(DRG.TrueThrust);
            }

            // aoe combo
            if (actionId == DRG.CoerthanTorment)
            {
                var gauge = GetJobGauge();
                if (gauge.FirstmindsFocusCount == 2)
                    return DRG.WyrmwindThrust;

                if ((lastMove == DRG.DoomSpike || lastMove == DRG.DraconianFury) && level >= Levels.SonicThrust)
                    return DRG.SonicThrust;
                if (lastMove == DRG.SonicThrust && level >= Levels.CoerthanTorment)
                    return DRG.CoerthanTorment;

                return originalHook(DRG.DoomSpike);
            }

            return null;
        }

        public override byte ClassId => 4;
        public override byte JobId => 22;
    }
}
