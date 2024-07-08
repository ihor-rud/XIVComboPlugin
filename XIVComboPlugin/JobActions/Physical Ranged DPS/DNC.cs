using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class DNC
{
    const uint
       Bladeshower = 15994,
       Bloodshower = 15996,
       Windmill = 15993,
       RisingWindmill = 15995,
       Cascade = 15989,
       Fountain = 15990,
       ReverseCascade = 15991,
       Fountainfall = 15992,
       FanDance1 = 16007,
       FanDance2 = 16008,
       FanDance3 = 16009,
       FanDance4 = 25791,
       Flourish = 16013,
       Devilment = 16011,
       StarfallDance = 25792;

    static class Buffs
    {
        public const short
            FlourishingSymmetry = 3017,
            FlourishingFlow = 3018,
            ThreefoldFanDance = 1820,
            FourfoldFanDance = 2699,
            FlourishingStarfall = 2700,
            SilkenSymmetry = 2693,
            SilkenFlow = 2694;
    }

    static class Levels
    {
        public const byte
            Fountain = 2,
            ReverseCascade = 20,
            Bladeshower = 25,
            RisingWindmill = 35,
            Fountainfall = 40,
            Bloodshower = 45,
            FanDance4 = 86,
            StarfallDance = 90;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<DNCGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == DNC.Windmill)
            {
                if (level >= Levels.Bloodshower && (HasEffect(Buffs.SilkenFlow) || HasEffect(Buffs.FlourishingFlow)))
                    return DNC.Bloodshower;

                if (level >= Levels.RisingWindmill && (HasEffect(Buffs.SilkenSymmetry) || HasEffect(Buffs.FlourishingSymmetry)))
                    return DNC.RisingWindmill;

                if (lastMove == DNC.Windmill && level >= Levels.Bladeshower)
                    return DNC.Bladeshower;
            }

            if (actionId == DNC.Cascade)
            {
                if (GetJobGauge().IsDancing)
                    return originalHook(DNC.Cascade);

                if (level >= Levels.Fountainfall && (HasEffect(Buffs.SilkenFlow) || HasEffect(Buffs.FlourishingFlow)))
                    return DNC.Fountainfall;

                if (level >= Levels.ReverseCascade && (HasEffect(Buffs.SilkenSymmetry) || HasEffect(Buffs.FlourishingSymmetry)))
                    return DNC.ReverseCascade;

                if (lastMove == DNC.Cascade && level >= Levels.Fountain)
                    return DNC.Fountain;
            }

            if (actionId == DNC.FanDance1 || actionId == DNC.FanDance2)
            {
                if (HasEffect(Buffs.ThreefoldFanDance))
                    return DNC.FanDance3;
            }

            if (actionId == DNC.Flourish)
            {
                if (level >= Levels.FanDance4 && HasEffect(Buffs.FourfoldFanDance))
                    return DNC.FanDance4;
            }

            if (actionId == DNC.Devilment)
            {
                if (level >= Levels.StarfallDance && HasEffect(Buffs.FlourishingStarfall))
                    return DNC.StarfallDance;
            }

            return null;
        }

        public override byte ClassId => 0;
        public override byte JobId => 38;
    }
}
