using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class RDM
{
    const uint
       Veraero = 7507,
       Veraero2 = 16525,
       Verthunder = 7505,
       Verthunder2 = 16524,
       Redoublement = 7516,
       Zwerchhau = 7512,
       Riposte = 7504,
       Scatter = 7509,
       Verstone = 7511,
       Verfire = 7510,
       Jolt = 7503,
       Verholy = 7526,
       Verflare = 7525,
       Scorch = 16530,
       Resolution = 25858;

    static class Buffs
    {
        public const short
            Swiftcast = 167,
            VerfireReady = 1234,
            VerstoneReady = 1235,
            Acceleration = 1238,
            Dualcast = 1249;
    }

    static class Levels
    {
        public const byte
            Zwerchhau = 35,
            Redoublement = 50,
            Scorch = 80,
            Resolution = 90;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<RDMGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == RDM.Veraero2)
            {
                if (HasEffect(Buffs.Swiftcast) || HasEffect(Buffs.Dualcast) || HasEffect(Buffs.Acceleration))
                    return originalHook(RDM.Scatter);
            }

            if (actionId == RDM.Verthunder2)
            {
                if (HasEffect(Buffs.Swiftcast) || HasEffect(Buffs.Dualcast) || HasEffect(Buffs.Acceleration))
                    return originalHook(RDM.Scatter);
            }

            if (actionId == RDM.Redoublement)
            {
                if (lastMove == RDM.Riposte && level >= Levels.Zwerchhau)
                    return originalHook(RDM.Zwerchhau);
                if (lastMove == RDM.Zwerchhau && level >= Levels.Redoublement)
                    return originalHook(RDM.Redoublement);
                return originalHook(RDM.Riposte);
            }

            if (actionId == RDM.Verthunder)
            {
                if (level >= Levels.Scorch && (lastMove == RDM.Verflare || lastMove == RDM.Verholy))
                    return RDM.Scorch;
                if (level >= Levels.Resolution && lastMove == RDM.Scorch)
                    return RDM.Resolution;
                if (GetJobGauge().ManaStacks >= 3)
                    return originalHook(RDM.Verthunder2);
                if (HasEffect(Buffs.Swiftcast) || HasEffect(Buffs.Dualcast) || HasEffect(Buffs.Acceleration))
                    return originalHook(RDM.Verthunder);
                if (HasEffect(Buffs.VerfireReady))
                    return RDM.Verfire;
                return originalHook(RDM.Jolt);
            }

            if (actionId == RDM.Veraero)
            {
                if (level >= Levels.Scorch && (lastMove == RDM.Verflare || lastMove == RDM.Verholy))
                    return RDM.Scorch;
                if (level >= Levels.Resolution && lastMove == RDM.Scorch)
                    return RDM.Resolution;
                if (GetJobGauge().ManaStacks >= 3)
                    return originalHook(RDM.Veraero2);
                if (HasEffect(Buffs.Swiftcast) || HasEffect(Buffs.Dualcast) || HasEffect(Buffs.Acceleration))
                    return originalHook(RDM.Veraero);
                if (HasEffect(Buffs.VerstoneReady))
                    return RDM.Verstone;
                return originalHook(RDM.Jolt);
            }

            return null;
        }

        public override byte ClassId => 0;
        public override byte JobId => 35;
    }
}
