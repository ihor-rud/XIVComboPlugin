using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

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

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<WHMGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == WHM.Cure || actionId == WHM.Cure2 || actionId == WHM.AfflatusSolace)
            {
                if (level >= Levels.AfflatusSolace && GetJobGauge().Lily > 0)
                    return WHM.AfflatusSolace;

                if (level >= Levels.Cure2)
                    return WHM.Cure2;

                return WHM.Cure;
            }

            if (actionId == WHM.Medica || actionId == WHM.AfflatusRapture)
            {
                if (level >= Levels.AfflatusRapture && GetJobGauge().Lily > 0)
                    return WHM.AfflatusRapture;
            }

            return null;
        }

        public override byte ClassId => 6;
        public override byte JobId => 24;
    }
}
