using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos;

static class AST
{
    const uint
       Benefic = 3594,
       Benefic2 = 3610,
       Play = 17055,
       Draw = 3590,
       MinorArcana = 7443,
       CrownPlay = 25869;

    static class Levels
    {
        public const byte
            Benefic2 = 26;
    }

    public class Combo(IClientState clientState, IJobGauges jobGauges) : ComboHelpers<ASTGauge>(clientState, jobGauges)
    {
        public override uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook)
        {
            // var level = PlayerLevel();

            // if (actionId == AST.Play)
            // {
            //     var gauge = GetJobGauge();
            //     if (gauge.DrawnCard == CardType.NONE)
            //         return originalHook(AST.Draw);
            // }

            // if (actionId == AST.CrownPlay)
            // {
            //     var gauge = GetJobGauge();
            //     if (gauge.DrawnCrownCard == CardType.NONE)
            //         return originalHook(AST.MinorArcana);
            // }

            // if (actionId == AST.Benefic || actionId == AST.Benefic2)
            // {
            //     if (level >= Levels.Benefic2)
            //         return AST.Benefic2;
            //     return AST.Benefic;
            // }

            return null;
        }

        public override byte ClassId => 0;
        public override byte JobId => 33;
    }
}
