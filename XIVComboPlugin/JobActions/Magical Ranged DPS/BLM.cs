using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class BLM
{
    const uint
       Scathe = 156,
       Fire = 141,
       Fire3 = 152,
       Fire4 = 3577,
       Blizzard1 = 142,
       Blizzard2 = 25793,
       Blizzard3 = 154,
       Blizzard4 = 3576,
       Freeze = 159,
       Transpose = 149,
       UmbralSoul = 16506,
       LeyLines = 3573,
       BetweenTheLines = 7419,
       Xenoglossy = 16507;

    static class Buffs
    {
        public const short
            LeyLines = 737,
            Firestarter = 165;
    }

    static class Levels
    {
        public const byte
            Blizzard3 = 35,
            Fire3 = 35,
            Freeze = 40,
            BetweenTheLines = 62,
            UmbralSoul = 76,
            Xenoglossy = 80;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<BLMGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == BLM.Blizzard1 || actionId == BLM.Blizzard3)
            {
                var gauge = GetJobGauge();
                if (level >= Levels.Blizzard3 && !gauge.IsParadoxActive)
                    return BLM.Blizzard3;
                return originalHook(BLM.Blizzard1);
            }

            if (actionId == BLM.Fire)
            {
                var gauge = GetJobGauge();
                if (gauge.IsParadoxActive)
                    return originalHook(BLM.Fire);
                if (level >= Levels.Fire3 && (!gauge.InAstralFire || HasEffect(Buffs.Firestarter)))
                    return BLM.Fire3;
            }

            if (actionId == BLM.Blizzard4 || actionId == BLM.Fire4)
            {
                var gauge = GetJobGauge();
                if (gauge.InUmbralIce)
                    return BLM.Blizzard4;
                return BLM.Fire4;
            }

            if (actionId == BLM.Freeze || actionId == BLM.Blizzard2)
            {
                var gauge = GetJobGauge();
                if (gauge.InUmbralIce && level >= Levels.Freeze)
                    return BLM.Freeze;
                return originalHook(BLM.Blizzard2);
            }

            if (actionId == BLM.Transpose || actionId == BLM.UmbralSoul)
            {
                var gauge = GetJobGauge();
                if (gauge.InUmbralIce && level >= Levels.UmbralSoul)
                    return BLM.UmbralSoul;
            }

            if (actionId == BLM.LeyLines || actionId == BLM.BetweenTheLines)
            {
                if (level >= Levels.BetweenTheLines && HasEffect(Buffs.LeyLines))
                    return BLM.BetweenTheLines;
            }

            if (actionId == BLM.Scathe)
            {
                var gauge = GetJobGauge();
                if (gauge.PolyglotStacks > 0 && level >= Levels.Xenoglossy)
                    return BLM.Xenoglossy;
            }

            return null;
        }

        public override byte ClassId => 7;
        public override byte JobId => 25;
    }
}
