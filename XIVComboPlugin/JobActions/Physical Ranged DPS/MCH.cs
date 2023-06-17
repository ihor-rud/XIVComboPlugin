using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    public static class MCH
    {
        public const uint
            CleanShot = 2873,
            HeatedCleanShot = 7413,
            SplitShot = 2866,
            SlugShot = 2868,
            HeatBlast = 7410,
            SpreadShot = 2870,
            Scattergun = 25786,
            AutoCrossbow = 16497;

        public static class Levels
        {
            public const byte
                SlugShot = 2,
                CleanShot = 26,
                HeatBlast = 35,
                AutoCrossbow = 52;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 0;
                this.JobID = 31;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == MCH.CleanShot || actionID == MCH.HeatedCleanShot)
                {
                    if (GetJobGauge<MCHGauge>().IsOverheated && level >= Levels.HeatBlast)
                        return MCH.HeatBlast;

                    if (comboTime > 0)
                    {
                        if (lastMove == MCH.SplitShot && level >= Levels.SlugShot)
                        {
                            return originalHook(MCH.SlugShot);
                        }

                        if (lastMove == MCH.SlugShot && level >= Levels.CleanShot)
                        {
                            return originalHook(MCH.CleanShot);
                        }
                    }

                    return originalHook(MCH.SplitShot);
                }

                if (actionID == MCH.SpreadShot || actionID == MCH.Scattergun)
                {
                    if (GetJobGauge<MCHGauge>().IsOverheated && level >= Levels.AutoCrossbow)
                        return MCH.AutoCrossbow;
                }

                return null;
            }
        }
    }
}
