using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.Statuses;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class PLD
{
    const uint
       FastBlade = 9,
       RiotBlade = 15,
       RageOfHalone = 21,
       RoyalAuthority = 3539,
       Atonement = 16460,
       TotalEclipse = 7381,
       Prominence = 16457,
       HolySpirit = 7384,
       HolyCircle = 16458,
       Confiteor = 16459;

    static class Buffs
    {
        public const short
            Requiescat = 1368,
            AtonementReady = 1902,
            BladeOfFaithReady = 3019;
    }

    static class Levels
    {
        public const byte
            RiotBlade = 4,
            RageOfHalone = 26,
            Prominence = 40,
            Confiteor = 80;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<PLDGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();
            if (actionId == PLD.RoyalAuthority || actionId == PLD.RageOfHalone)
            {
                if (lastMove == PLD.FastBlade && level >= Levels.RiotBlade)
                    return PLD.RiotBlade;

                if (lastMove == PLD.RiotBlade && level >= Levels.RageOfHalone)
                    return originalHook(PLD.RageOfHalone);

                if (HasEffect(Buffs.AtonementReady))
                    return PLD.Atonement;

                return PLD.FastBlade;
            }

            if (actionId == PLD.Prominence)
            {
                if (lastMove == PLD.TotalEclipse && level >= Levels.Prominence)
                    return PLD.Prominence;

                return PLD.TotalEclipse;
            }

            if (actionId == PLD.HolySpirit || actionId == PLD.HolyCircle)
            {
                var originalConfiteor = originalHook(PLD.Confiteor);
                if (originalConfiteor != PLD.Confiteor)
                    return originalConfiteor;

                Status requiescat = FindEffect(Buffs.Requiescat);

                if (requiescat != null)
                {
                    if (requiescat.StackCount <= 1 && level >= PLD.Levels.Confiteor)
                    {
                        return originalHook(PLD.Confiteor);
                    }
                }
            }

            return null;
        }

        public override byte ClassId => 1;
        public override byte JobId => 19;
    }
}
