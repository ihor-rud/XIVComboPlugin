using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
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

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 7;
                this.JobID = 25;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == BLM.Blizzard1 || actionID == BLM.Blizzard3)
                {
                    if (level >= Levels.Blizzard3)
                        return BLM.Blizzard3;
                    return BLM.Blizzard1;
                }

                if (actionID == BLM.Fire)
                {
                    var gauge = GetJobGauge<BLMGauge>();
                    if (level >= Levels.Fire3 && (!gauge.InAstralFire || HasEffect(Buffs.Firestarter)))
                        return BLM.Fire3;
                    return originalHook(BLM.Fire);
                }

                if (actionID == BLM.Blizzard4 || actionID == BLM.Fire4)
                {
                    var gauge = GetJobGauge<BLMGauge>();
                    if (gauge.InUmbralIce)
                        return BLM.Blizzard4;
                    return BLM.Fire4;
                }

                if (actionID == BLM.Freeze || actionID == BLM.Blizzard2)
                {
                    var gauge = GetJobGauge<BLMGauge>();
                    if (gauge.InUmbralIce && level >= Levels.Freeze)
                        return BLM.Freeze;
                    return originalHook(BLM.Blizzard2);
                }

                if (actionID == BLM.Transpose || actionID == BLM.UmbralSoul)
                {
                    var gauge = GetJobGauge<BLMGauge>();
                    if (gauge.InUmbralIce && level >= Levels.UmbralSoul)
                        return BLM.UmbralSoul;
                }

                if (actionID == BLM.LeyLines || actionID == BLM.BetweenTheLines)
                {
                    if (level >= Levels.BetweenTheLines && HasEffect(Buffs.LeyLines))
                        return BLM.BetweenTheLines;
                }

                if (actionID == BLM.Scathe)
                {
                    var gauge = GetJobGauge<BLMGauge>();
                    if (gauge.PolyglotStacks > 0 && level >= Levels.Xenoglossy)
                        return BLM.Xenoglossy;
                }

                return null;
            }
        }
    }
}
