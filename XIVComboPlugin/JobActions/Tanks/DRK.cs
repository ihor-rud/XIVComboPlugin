using System;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Plugin.Services;

namespace XIVComboPlugin.Combos;

static class DRK
{
    const uint
       HardSlash = 3617,
       SyphonStrike = 3623,
       Souleater = 3632,
       Unleash = 3621,
       StalwartSoul = 16468;

    static class Levels
    {
        public const byte
            SyphonStrike = 2,
            Souleater = 26,
            StalwartSoul = 40;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<DRKGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();
            if (actionId == DRK.Souleater)
            {
                if (lastMove == DRK.HardSlash && level >= Levels.SyphonStrike)
                    return DRK.SyphonStrike;
                if (lastMove == DRK.SyphonStrike && level >= Levels.Souleater)
                    return DRK.Souleater;

                return DRK.HardSlash;
            }

            if (actionId == DRK.StalwartSoul)
            {
                if (lastMove == DRK.Unleash && level >= Levels.StalwartSoul)
                    return DRK.StalwartSoul;

                return DRK.Unleash;
            }

            return null;
        }

        public override byte ClassId => 0;
        public override byte JobId => 32;
    }
}
