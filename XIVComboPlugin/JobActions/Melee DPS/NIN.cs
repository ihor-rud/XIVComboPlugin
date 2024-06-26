﻿using System;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.Combos
{
    static class NIN
    {
        const uint
           Ninjutsu = 2260,
           SpinningEdge = 2240,
           GustSlash = 2242,
           AeolianEdge = 2255,
           ArmorCrush = 3563,
           DeathBlossom = 2254,
           HakkeMujinsatsu = 16488,
           TenChiJin = 7403,
           Meisui = 16489,
           Bunshin = 16493,
           PhantomKamaitachi = 25774,
           FleetingRaiju = 25778;

        static class Buffs
        {
            public const short
                Mudra = 496,
                Suiton = 507,
                Bunshin = 1954,
                RaijuReady = 2690;
        }

        static class Levels
        {
            public const byte
                GustSlash = 4,
                AeolianEdge = 26,
                HakkeMujinsatsu = 52,
                ArmorCrush = 54,
                Meisui = 72,
                PhantomKamaitachi = 82,
                Raiju = 90;
        }

        public class Combo : CustomCombo
        {
            public Combo(IClientState clientState, IJobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 29;
                this.JobID = 30;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == NIN.ArmorCrush)
                {
                    if (HasEffect(NIN.Buffs.Mudra))
                        return originalHook(NIN.Ninjutsu);

                    if (level >= NIN.Levels.Raiju && HasEffect(NIN.Buffs.RaijuReady))
                    {
                        return NIN.FleetingRaiju;
                    }

                    if (comboTime > 0)
                    {
                        if (lastMove == NIN.SpinningEdge && level >= Levels.GustSlash)
                            return NIN.GustSlash;
                        if (lastMove == NIN.GustSlash && level >= Levels.ArmorCrush)
                            return NIN.ArmorCrush;
                        if (lastMove == NIN.GustSlash && level >= Levels.AeolianEdge)
                            return NIN.AeolianEdge;
                    }

                    return NIN.SpinningEdge;
                }

                if (actionID == NIN.HakkeMujinsatsu)
                {
                    if (HasEffect(NIN.Buffs.Mudra))
                        return originalHook(NIN.Ninjutsu);

                    if (comboTime > 0)
                    {
                        if (lastMove == NIN.DeathBlossom && level >= Levels.HakkeMujinsatsu)
                            return NIN.HakkeMujinsatsu;
                    }

                    return NIN.DeathBlossom;
                }

                if (actionID == NIN.TenChiJin)
                {
                    if (level >= Levels.Meisui && HasEffect(NIN.Buffs.Suiton))
                        return NIN.Meisui;

                    return NIN.TenChiJin;
                }

                if (actionID == NIN.Bunshin)
                {
                    if (level >= Levels.PhantomKamaitachi && HasEffect(Buffs.Bunshin))
                        return NIN.PhantomKamaitachi;

                    return NIN.Bunshin;
                }

                return null;
            }
        }
    }
}
