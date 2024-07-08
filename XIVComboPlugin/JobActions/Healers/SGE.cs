using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class SGE
{
    const uint
       Diagnosis = 24284,
       Druochole = 24296,
       Dyskrasia = 24297,
       Toxikon = 24304;

    static class Levels
    {
        public const byte
            Druochole = 45,
            Ixochole = 52,
            Toxikon = 66;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<SGEGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            var level = PlayerLevel();

            if (actionId == SGE.Diagnosis || actionId == SGE.Druochole)
            {
                var gauge = GetJobGauge();
                if (level > Levels.Druochole && gauge.Addersgall > 0 && !gauge.Eukrasia)
                    return SGE.Druochole;
            }

            if (actionId == SGE.Dyskrasia)
            {
                if (level >= Levels.Toxikon && GetJobGauge().Addersting > 0)
                    return originalHook(SGE.Toxikon);
            }

            return null;
        }

        public override byte ClassId => 0;
        public override byte JobId => 40;
    }
}
