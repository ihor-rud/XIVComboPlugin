using System;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.Combos;

static class WAR
{
    const uint
       StormsPath = 42,
       HeavySwing = 31,
       Maim = 37,
       StormsEye = 45,
       MythrilTempest = 16462,
       Overpower = 41,
       InnerBeast = 49,
       SteelCyclone = 51,
       FellCleave = 3549,
       Decimate = 3550,
       PrimalRend = 25753,
       PrimalWrath = 36924;

    static class Levels
    {
        public const byte
            Maim = 4,
            StormsPath = 26,
            MythrilTempest = 40,
            StormsEye = 50;
    }

    static class Buffs
    {
        public const short
            PrimalRendReady = 2624,
            Wrathful = 3901;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<WARGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();
            if (actionId == WAR.StormsPath)
            {
                if (lastMove == WAR.HeavySwing && level >= Levels.Maim)
                    return WAR.Maim;

                if (lastMove == WAR.Maim && level >= Levels.StormsPath)
                    return WAR.StormsPath;

                return WAR.HeavySwing;
            }

            if (actionId == WAR.StormsEye)
            {
                if (lastMove == WAR.HeavySwing && level >= Levels.Maim)
                    return WAR.Maim;

                if (lastMove == WAR.Maim && level >= Levels.StormsEye)
                    return WAR.StormsEye;

                return WAR.HeavySwing;
            }

            if (actionId == WAR.MythrilTempest)
            {
                if (lastMove == WAR.Overpower && level >= Levels.MythrilTempest)
                    return WAR.MythrilTempest;

                return WAR.Overpower;
            }

            if (actionId == WAR.InnerBeast || actionId == WAR.SteelCyclone || actionId == WAR.FellCleave || actionId == WAR.Decimate)
            {
                if (HasEffect(Buffs.PrimalRendReady))
                    return WAR.PrimalRend;

                if (HasEffect(Buffs.Wrathful))
                    return WAR.PrimalWrath;
            }

            return null;
        }

        public override byte ClassId => 3;
        public override byte JobId => 21;
    }
}
