using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;

namespace XIVComboPlugin.Combos
{
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

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 0;
                this.JobID = 32;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;
                if (actionID == DRK.Souleater)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == DRK.HardSlash && level >= Levels.SyphonStrike)
                            return DRK.SyphonStrike;
                        if (lastMove == DRK.SyphonStrike && level >= Levels.Souleater)
                            return DRK.Souleater;
                    }

                    return DRK.HardSlash;
                }

                if (actionID == DRK.StalwartSoul)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == DRK.Unleash && level >= Levels.StalwartSoul)
                            return DRK.StalwartSoul;
                    }

                    return DRK.Unleash;
                }

                return null;
            }
        }
    }
}
