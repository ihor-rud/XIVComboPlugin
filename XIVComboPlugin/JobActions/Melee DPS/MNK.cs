using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;

namespace XIVComboPlugin.Combos
{
    public static class MNK
    {
        public const uint
            Bootshine = 53,
            DragonKick = 74,
            ArmOfTheDestroyer = 62,
            Rockbreaker = 70,
            FourPointFury = 16473;

        public static class Buffs
        {
            public const short
                OpoOpoForm = 107,
                RaptorForm = 108,
                CoerlForm = 109,
                PerfectBalance = 110,
                LeadenFist = 1861,
                FormlessFist = 2513;
        }

        public static class Levels
        {
            public const byte
                Rockbreaker = 30,
                FourPointFury = 45,
                DragonKick = 50,
                ShadowOfTheDestroyer = 82;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 2;
                this.JobID = 20;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;
                if (actionID == MNK.Rockbreaker)
                {
                    if (HasEffect(Buffs.FormlessFist))
                    {
                        return MNK.FourPointFury;
                    }

                    if (HasEffect(Buffs.PerfectBalance))
                    {
                        if (level >= Levels.ShadowOfTheDestroyer)
                            return originalHook(MNK.ArmOfTheDestroyer);

                        return MNK.Rockbreaker;
                    }

                    if (HasEffect(Buffs.OpoOpoForm))
                        return originalHook(MNK.ArmOfTheDestroyer);

                    if (HasEffect(Buffs.RaptorForm) && level >= Levels.FourPointFury)
                        return MNK.FourPointFury;

                    if (HasEffect(Buffs.CoerlForm) && level >= Levels.Rockbreaker)
                        return MNK.Rockbreaker;

                    return originalHook(MNK.ArmOfTheDestroyer);
                }

                if (actionID == MNK.DragonKick)
                {
                    if (HasEffect(Buffs.LeadenFist))
                        return MNK.Bootshine;

                    if ((HasEffect(Buffs.OpoOpoForm) || HasEffect(Buffs.PerfectBalance) || HasEffect(Buffs.FormlessFist)) && level >= Levels.DragonKick)
                        return MNK.DragonKick;

                    return MNK.Bootshine;
                }

                return null;
            }
        }
    }
}
