using System;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.Combos;

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
       ForkedRaiju = 25777,
       FleetingRaiju = 25778;

    static class Buffs
    {
        public const short
            Mudra = 496,
            RaijuReady = 2690,
            ShadowWalker = 3848;
    }

    static class Levels
    {
        public const byte
            GustSlash = 4,
            AeolianEdge = 26,
            HakkeMujinsatsu = 52,
            ArmorCrush = 54,
            Meisui = 72,
            Raiju = 90;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<NINGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == NIN.ArmorCrush)
            {
                if (HasEffect(NIN.Buffs.Mudra))
                    return originalHook(NIN.Ninjutsu);

                if (level >= NIN.Levels.Raiju && HasEffect(NIN.Buffs.RaijuReady))
                    return NIN.ForkedRaiju;

                if (lastMove == NIN.SpinningEdge && level >= Levels.GustSlash)
                    return NIN.GustSlash;

                if (lastMove == NIN.GustSlash && level >= Levels.ArmorCrush)
                    return NIN.ArmorCrush;

                return NIN.SpinningEdge;
            }

            if (actionId == NIN.AeolianEdge)
            {
                if (HasEffect(NIN.Buffs.Mudra))
                    return originalHook(NIN.Ninjutsu);

                if (level >= NIN.Levels.Raiju && HasEffect(NIN.Buffs.RaijuReady))
                    return NIN.FleetingRaiju;

                if (lastMove == NIN.SpinningEdge && level >= Levels.GustSlash)
                    return NIN.GustSlash;

                if (lastMove == NIN.GustSlash && level >= Levels.AeolianEdge)
                    return NIN.AeolianEdge;

                return NIN.SpinningEdge;
            }

            if (actionId == NIN.HakkeMujinsatsu)
            {
                if (HasEffect(NIN.Buffs.Mudra))
                    return originalHook(NIN.Ninjutsu);

                if (lastMove == NIN.DeathBlossom && level >= Levels.HakkeMujinsatsu)
                    return NIN.HakkeMujinsatsu;

                return NIN.DeathBlossom;
            }

            if (actionId == NIN.TenChiJin || actionId == NIN.Meisui)
            {
                if (level >= Levels.Meisui && HasEffect(NIN.Buffs.ShadowWalker))
                    return NIN.Meisui;

                return originalHook(NIN.TenChiJin);
            }

            return null;
        }

        public override byte ClassId => 29;
        public override byte JobId => 30;
    }
}
