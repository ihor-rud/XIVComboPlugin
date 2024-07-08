using System;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.Combos;

static class RPR
{
    const uint
       Slice = 24373,
       WaxingSlice = 24374,
       InfernalSlice = 24375,
       Gibbet = 24382,
       Gallows = 24383,
       SpinningScythe = 24376,
       NightmareScythe = 24377,
       Guillotine = 24384;

    static class Buffs
    {
        public const short
            Enshrouded = 2593,
            SoulReaver = 2587,
            ImmortalSacrifice = 2592,
            EnhancedGibbet = 2588,
            EnhancedGallows = 2589,
            EnhancedVoidReaping = 2590,
            EnhancedCrossReaping = 2591,
            Threshold = 2595;
    }

    static class Levels
    {
        public const byte
            WaxingSlice = 5,
            SpinningScythe = 25,
            InfernalSlice = 30,
            NightmareScythe = 45,
            Gibbet = 70,
            Enshroud = 80,
            PlentifulHarvest = 88,
            Communio = 90;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<RPRGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == RPR.InfernalSlice)
            {
                if (level >= Levels.Gibbet && HasEffect(Buffs.SoulReaver))
                {
                    if (HasEffect(Buffs.EnhancedGibbet))
                        return originalHook(RPR.Gibbet);
                    return originalHook(RPR.Gallows);
                }

                if (level >= Levels.Enshroud && HasEffect(Buffs.Enshrouded))
                {
                    if (HasEffect(Buffs.EnhancedVoidReaping))
                        return originalHook(RPR.Gibbet);
                    return originalHook(RPR.Gallows);
                }

                if (lastMove == RPR.Slice && level >= Levels.WaxingSlice)
                    return RPR.WaxingSlice;

                if (lastMove == RPR.WaxingSlice && level >= Levels.InfernalSlice)
                    return RPR.InfernalSlice;

                return RPR.Slice;
            }

            if (actionId == RPR.NightmareScythe)
            {
                if (HasEffect(Buffs.SoulReaver) || HasEffect(Buffs.Enshrouded))
                {
                    return originalHook(RPR.Guillotine);
                }

                if (lastMove == RPR.SpinningScythe && level >= Levels.NightmareScythe)
                    return RPR.NightmareScythe;

                return RPR.SpinningScythe;
            }

            return null;
        }

        public override byte ClassId => 0;
        public override byte JobId => 39;
    }
}
