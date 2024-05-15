using System;
using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
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

        public class Combo : CustomCombo
        {
            public Combo(IClientState clientState, IJobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 0;
                this.JobID = 33;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == AST.Play)
                {
                    var gauge = GetJobGauge<ASTGauge>();
                    if (gauge.DrawnCard == CardType.NONE)
                        return originalHook(AST.Draw);
                }

                if (actionID == AST.CrownPlay)
                {
                    var gauge = GetJobGauge<ASTGauge>();
                    if (gauge.DrawnCrownCard == CardType.NONE)
                        return originalHook(AST.MinorArcana);
                }

                if (actionID == AST.Benefic || actionID == AST.Benefic2)
                {
                    if (level >= Levels.Benefic2)
                        return AST.Benefic2;

                    return AST.Benefic;
                }

                return null;
            }
        }
    }
}
