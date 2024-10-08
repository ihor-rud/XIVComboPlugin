using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class PCT
{
    const uint
        FireRed = 34650,
        Fire2Red = 34656,
        BlizzardCyan = 34653,
        Blizzard2Cyan = 34659,
        CreatureMotif = 34689,
        LivingMuse = 35347,
        WeaponMotif = 34690,
        SteelMuse = 35348,
        HammerStamp = 34678,
        LandscapeMotif = 34691,
        ScenicMuse = 35349,
        RainbowDrip = 34688,
        HolyInWhite = 34662,
        CometInBlack = 34663;

    static class Buffs
    {
        public const short
            SubstractivePalette = 3674,
            HammerTime = 3680,
            RainbowBright = 3679,
            MonochromeTones = 3691;
    }

    static class Levels
    {

    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<PCTGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == PCT.BlizzardCyan || actionId == PCT.FireRed)
            {
                if (HasEffect(Buffs.RainbowBright))
                    return PCT.RainbowDrip;

                if (HasEffect(Buffs.SubstractivePalette))
                    return originalHook(PCT.BlizzardCyan);

                return originalHook(PCT.FireRed);
            }

            if (actionId == PCT.Blizzard2Cyan || actionId == PCT.Fire2Red)
            {
                if (HasEffect(Buffs.RainbowBright))
                    return PCT.RainbowDrip;

                if (HasEffect(Buffs.SubstractivePalette))
                    return originalHook(PCT.Blizzard2Cyan);

                return originalHook(PCT.Fire2Red);
            }

            if (actionId == PCT.CreatureMotif)
            {
                if (GetJobGauge().CreatureMotifDrawn)
                    return originalHook(PCT.LivingMuse);

                return originalHook(PCT.CreatureMotif);
            }

            if (actionId == PCT.WeaponMotif)
            {
                if (HasEffect(Buffs.HammerTime))
                    return originalHook(PCT.HammerStamp);

                if (GetJobGauge().WeaponMotifDrawn)
                    return originalHook(PCT.SteelMuse);

                return originalHook(PCT.WeaponMotif);
            }

            if (actionId == PCT.LandscapeMotif)
            {
                if (GetJobGauge().LandscapeMotifDrawn)
                    return originalHook(PCT.ScenicMuse);

                return originalHook(PCT.LandscapeMotif);
            }

            if (actionId == PCT.HolyInWhite || actionId == PCT.CometInBlack)
            {
                if (HasEffect(Buffs.MonochromeTones))
                    return PCT.CometInBlack;

                return PCT.HolyInWhite;
            }

            return null;
        }

        public override byte ClassId => 0;
        public override byte JobId => 42;
    }
}
