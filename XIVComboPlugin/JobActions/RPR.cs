﻿using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;

namespace XIVComboPlugin.Combos
{
    public static class RPR
    {
        public const uint
            Slice = 24373,
            WaxingSlice = 24374,
            InfernalSlice = 24375,
            Gibbet = 24382,
            Gallows = 24383,
            ShadowOfDeath = 24378,
            SpinningScythe = 24376,
            NightmareScythe = 24377,
            Guillotine = 24384,
            Enshroud = 24394,
            Communio = 24398,
            ArcaneCircle = 24405,
            PlentifulHarvest = 24385,
            HellsIngress = 24401,
            HellsEgress = 24402,
            Regress = 24403;

        public static class Buffs
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

        public static class Levels
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

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 0;
                this.JobID = 39;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == RPR.InfernalSlice)
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

                    if (comboTime > 0)
                    {
                        if (lastMove == RPR.Slice && level >= Levels.WaxingSlice)
                            return RPR.WaxingSlice;

                        if (lastMove == RPR.WaxingSlice && level >= Levels.InfernalSlice)
                            return RPR.InfernalSlice;
                    }

                    return RPR.Slice;
                }

                if (actionID == RPR.NightmareScythe)
                {
                    if (HasEffect(Buffs.SoulReaver) || HasEffect(Buffs.Enshrouded))
                    {
                        return originalHook(RPR.Guillotine);
                    }

                    if (comboTime > 0)
                    {
                        if (lastMove == RPR.SpinningScythe && level >= Levels.NightmareScythe)
                            return RPR.NightmareScythe;
                    }

                    return RPR.SpinningScythe;
                }

                return null;
            }
        }
    }
}
