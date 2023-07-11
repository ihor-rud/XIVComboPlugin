using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    static class SAM
    {
        const uint
           Hakaze = 7477,
           Jinpu = 7478,
           Gekko = 7481,
           Shifu = 7479,
           Kasha = 7482,
           Yukikaze = 7480,
           Fuga = 7483,
           Fuko = 25780,
           Mangetsu = 7484,
           Oka = 7485,
           Iaijutsu = 7867,
           TsubameGaeshi = 16483,
           Shoha = 16487,
           Kyuten = 7491,
           Shoha2 = 25779,
           Ikishoten = 16482,
           OgiNamikiri = 25781;

        static class Buffs
        {
            public const short
                MeikyoShisui = 1233,
                OgiNamikiriReady = 2959;
        }

        static class Levels
        {
            public const byte
                Jinpu = 4,
                Shifu = 18,
                Gekko = 30,
                Mangetsu = 35,
                Kasha = 40,
                Oka = 45,
                Yukikaze = 50,
                TsubameGaeshi = 76,
                Shoha = 80,
                Shoha2 = 82,
                OgiNamikiri = 90;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 0;
                this.JobID = 34;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == SAM.Oka)
                {
                    if (HasEffect(Buffs.MeikyoShisui))
                        return SAM.Oka;

                    if (comboTime > 0)
                    {
                        if ((lastMove == SAM.Fuga || lastMove == SAM.Fuko) && level >= Levels.Oka)
                            return SAM.Oka;
                    }

                    return originalHook(SAM.Fuga);
                }

                if (actionID == SAM.Mangetsu)
                {
                    if (HasEffect(Buffs.MeikyoShisui))
                        return SAM.Mangetsu;

                    if (comboTime > 0)
                    {
                        if ((lastMove == SAM.Fuga || lastMove == SAM.Fuko) && level >= Levels.Mangetsu)
                            return SAM.Mangetsu;
                    }

                    return originalHook(SAM.Fuga);
                }

                if (actionID == SAM.Gekko)
                {
                    if (HasEffect(Buffs.MeikyoShisui))
                        return SAM.Gekko;

                    if (comboTime > 0)
                    {
                        if (lastMove == SAM.Hakaze && level >= Levels.Jinpu)
                            return SAM.Jinpu;
                        if (lastMove == SAM.Jinpu && level >= Levels.Gekko)
                            return SAM.Gekko;
                    }

                    return SAM.Hakaze;
                }

                if (actionID == SAM.Kasha)
                {
                    if (HasEffect(Buffs.MeikyoShisui))
                        return SAM.Kasha;

                    if (comboTime > 0)
                    {
                        if (lastMove == SAM.Hakaze && level >= Levels.Shifu)
                            return SAM.Shifu;
                        if (lastMove == SAM.Shifu && level >= Levels.Kasha)
                            return SAM.Kasha;
                    }

                    return SAM.Hakaze;
                }

                if (actionID == SAM.Yukikaze)
                {
                    if (HasEffect(Buffs.MeikyoShisui))
                        return SAM.Yukikaze;

                    if (comboTime > 0)
                    {
                        if (lastMove == SAM.Hakaze && level >= Levels.Yukikaze)
                            return SAM.Yukikaze;
                    }

                    return SAM.Hakaze;
                }

                if (actionID == SAM.Iaijutsu)
                {
                    var gauge = GetJobGauge<SAMGauge>();

                    if (level >= Levels.Shoha && gauge.MeditationStacks >= 3)
                        return SAM.Shoha;

                    var originalTsubameGaeshi = originalHook(SAM.TsubameGaeshi);
                    if (originalTsubameGaeshi != SAM.TsubameGaeshi)
                        return originalTsubameGaeshi;
                }

                if (actionID == SAM.Kyuten)
                {
                    var gauge = GetJobGauge<SAMGauge>();
                    if (level >= Levels.Shoha2 && gauge.MeditationStacks >= 3)
                        return SAM.Shoha2;
                }

                if (actionID == SAM.Ikishoten)
                {
                    if (level >= Levels.OgiNamikiri)
                    {
                        if (HasEffect(Buffs.OgiNamikiriReady))
                            return SAM.OgiNamikiri;

                        var originalOgiNamikiri = originalHook(SAM.OgiNamikiri);
                        if (originalOgiNamikiri != SAM.OgiNamikiri)
                            return originalOgiNamikiri;
                    }

                    return SAM.Ikishoten;
                }

                return null;
            }
        }
    }
}
