using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    static class WHM
    {
        const uint
           Cure = 120,
           Medica = 124,
           Cure2 = 135,
           AfflatusSolace = 16531,
           AfflatusRapture = 16534;

        static class Levels
        {
            public const byte
                Cure2 = 30,
                AfflatusSolace = 52,
                AfflatusRapture = 76;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 6;
                this.JobID = 24;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == WHM.Cure || actionID == WHM.Cure2)
                {
                    if (level >= Levels.AfflatusSolace && GetJobGauge<WHMGauge>().Lily > 0)
                        return WHM.AfflatusSolace;

                    if (level >= Levels.Cure2)
                        return WHM.Cure2;

                    return WHM.Cure;
                }

                if (actionID == WHM.Medica)
                {
                    if (level >= Levels.AfflatusRapture && GetJobGauge<WHMGauge>().Lily > 0)
                        return WHM.AfflatusRapture;
                }

                return null;
            }
        }
    }
}
