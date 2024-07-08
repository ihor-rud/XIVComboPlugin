using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class SCH
{
    const uint
       Physick = 190,
       Adloquium = 185,
       FeyBless = 16543,
       Consolation = 16546;

    static class Levels
    {
        public const byte
            Adloquium = 30;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<SCHGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == SCH.FeyBless)
            {
                if (GetJobGauge().SeraphTimer > 0)
                    return SCH.Consolation;
            }

            if (actionId == SCH.Physick || actionId == SCH.Adloquium)
            {
                if (level >= Levels.Adloquium)
                    return SCH.Adloquium;
                return SCH.Physick;
            }

            return null;
        }

        public override byte ClassId => 15;
        public override byte JobId => 28;
    }
}
