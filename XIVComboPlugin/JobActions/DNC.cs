﻿using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    public static class DNC
    {
        public const uint
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

        public static class Buffs
        {
            public const short
                FlourishingSymmetry = 2693,
                FlourishingFlow = 2694,
                FlourishingFanDance = 1820,
                FlourishingStarfall = 2700,
                FourfoldFanDance = 2699;
        }

        public static class Levels
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

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 0;
                this.JobID = 38;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == DNC.Windmill)
                {
                    if (level >= Levels.Bloodshower && HasEffect(Buffs.FlourishingFlow))
                        return DNC.Bloodshower;

                    if (level >= Levels.RisingWindmill && HasEffect(Buffs.FlourishingSymmetry))
                        return DNC.RisingWindmill;

                    if (comboTime > 0)
                    {
                        if (lastMove == DNC.Windmill && level >= Levels.Bladeshower)
                            return DNC.Bladeshower;
                    }
                }

                if (actionID == DNC.Cascade)
                {
                    if (GetJobGauge<DNCGauge>().IsDancing)
                        return originalHook(DNC.Cascade);

                    if (level >= Levels.Fountainfall && HasEffect(Buffs.FlourishingFlow))
                        return DNC.Fountainfall;

                    if (level >= Levels.ReverseCascade && HasEffect(Buffs.FlourishingSymmetry))
                        return DNC.ReverseCascade;

                    if (lastMove == DNC.Cascade && level >= Levels.Fountain)
                        return DNC.Fountain;
                }

                if (actionID == DNC.FanDance1 || actionID == DNC.FanDance2)
                {
                    if (HasEffect(Buffs.FlourishingFanDance))
                        return DNC.FanDance3;
                }

                if (actionID == DNC.Flourish)
                {
                    if (level >= Levels.FanDance4 && HasEffect(Buffs.FourfoldFanDance))
                        return DNC.FanDance4;
                }

                if (actionID == DNC.Devilment)
                {
                    if (level >= Levels.StarfallDance && HasEffect(Buffs.FlourishingStarfall))
                        return DNC.StarfallDance;
                }

                return null;
            }
        }
    }
}
