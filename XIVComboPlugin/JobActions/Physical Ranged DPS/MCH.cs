using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class MCH
{
    const uint
       CleanShot = 2873,
       HeatedCleanShot = 7413,
       SplitShot = 2866,
       HeatedSplitShot = 7411,
       SlugShot = 2868,
       HeatedSlugShot = 7412,

       HeatBlast = 7410,
       SpreadShot = 2870,
       Scattergun = 25786,
       AutoCrossbow = 16497;

    static class Levels
    {
        public const byte
            SlugShot = 2,
            CleanShot = 26,
            HeatBlast = 35,
            AutoCrossbow = 52;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<MCHGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == MCH.CleanShot || actionId == MCH.HeatedCleanShot)
            {
                if (GetJobGauge().IsOverheated && level >= Levels.HeatBlast)
                    return originalHook(MCH.HeatBlast);

                if ((lastMove == MCH.SplitShot || lastMove == MCH.HeatedSplitShot) && level >= Levels.SlugShot)
                    return originalHook(MCH.SlugShot);

                if ((lastMove == MCH.SlugShot || lastMove == MCH.HeatedSlugShot) && level >= Levels.CleanShot)
                    return originalHook(MCH.CleanShot);

                return originalHook(MCH.SplitShot);
            }

            if (actionId == MCH.SpreadShot || actionId == MCH.Scattergun)
            {
                if (GetJobGauge().IsOverheated && level >= Levels.AutoCrossbow)
                    return MCH.AutoCrossbow;
            }

            return null;
        }

        public override byte ClassId => 0;
        public override byte JobId => 31;
    }
}
