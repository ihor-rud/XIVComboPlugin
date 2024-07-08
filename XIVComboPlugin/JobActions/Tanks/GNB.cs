using System;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.Combos;

static class GNB
{
    const uint
       KeenEdge = 16137,
       BrutalShell = 16139,
       SolidBarrel = 16145,
       DemonSlice = 16141,
       DemonSlaughter = 16149,
       GnashingFang = 16146,
       Continuation = 16155;

    static class Buffs
    {
        public const short
            ReadyToRip = 1842,
            ReadyToTear = 1843,
            ReadyToGouge = 1844;
    }

    static class Levels
    {
        public const byte
            BrutalShell = 4,
            SolidBarrel = 26,
            DemonSlaughter = 40;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<GNBGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == GNB.SolidBarrel)
            {
                if (lastMove == GNB.KeenEdge && level >= Levels.BrutalShell)
                    return GNB.BrutalShell;
                if (lastMove == GNB.BrutalShell && level >= Levels.SolidBarrel)
                    return GNB.SolidBarrel;

                return GNB.KeenEdge;
            }

            if (actionId == GNB.GnashingFang)
            {
                if (HasEffect(Buffs.ReadyToRip) || HasEffect(Buffs.ReadyToTear) || HasEffect(Buffs.ReadyToGouge))
                    return originalHook(GNB.Continuation);
            }

            if (actionId == GNB.DemonSlaughter)
            {
                if (lastMove == GNB.DemonSlice && level >= Levels.DemonSlaughter)
                    return GNB.DemonSlaughter;

                return GNB.DemonSlice;
            }

            return null;
        }

        public override byte ClassId => 0;
        public override byte JobId => 37;
    }
}
