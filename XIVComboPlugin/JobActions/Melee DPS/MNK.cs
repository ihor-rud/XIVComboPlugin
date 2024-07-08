using System;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.Combos;

static class MNK
{
    const uint
       Bootshine = 53,
       DragonKick = 74,
       ArmOfTheDestroyer = 62,
       Rockbreaker = 70,
       FourPointFury = 16473;

    static class Buffs
    {
        public const short
            OpoOpoForm = 107,
            RaptorForm = 108,
            CoerlForm = 109,
            PerfectBalance = 110,
            LeadenFist = 1861,
            FormlessFist = 2513;
    }

    static class Levels
    {
        public const byte
            Rockbreaker = 30,
            FourPointFury = 45,
            DragonKick = 50,
            ShadowOfTheDestroyer = 82;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<MNKGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();
            if (actionId == MNK.Rockbreaker)
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

            if (actionId == MNK.DragonKick)
            {
                if (HasEffect(Buffs.LeadenFist))
                    return MNK.Bootshine;

                if ((HasEffect(Buffs.OpoOpoForm) || HasEffect(Buffs.PerfectBalance) || HasEffect(Buffs.FormlessFist)) && level >= Levels.DragonKick)
                    return MNK.DragonKick;

                return MNK.Bootshine;
            }

            return null;
        }

        public override byte ClassId => 2;
        public override byte JobId => 20;
    }
}
