using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    static class DRG
    {
        const uint
           TrueThrust = 75,
           VorpalThrust = 78,
           Disembowel = 87,
           FullThrust = 84,
           HeavensThrust = 25771,
           ChaosThrust = 88,
           ChaoticSpring = 25772,
           FangAndClaw = 3554,
           WheelingThrust = 3556,
           RaidenThrust = 16479,
           DoomSpike = 86,
           DraconianFury = 25770,
           SonicThrust = 7397,
           CoerthanTorment = 16477,
           WyrmwindThrust = 25773;

        static class Buffs
        {
            public const short
                SharperFangAndClaw = 802,
                EnhancedWheelingThrust = 803;
        }

        static class Levels
        {
            public const byte
                VorpalThrust = 4,
                Disembowel = 18,
                FullThrust = 26,
                ChaosThrust = 50,
                SonicThrust = 62,
                CoerthanTorment = 72;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 4;
                this.JobID = 22;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == DRG.CoerthanTorment)
                {
                    var gauge = GetJobGauge<DRGGauge>();
                    if (gauge.FirstmindsFocusCount == 2)
                        return DRG.WyrmwindThrust;

                    if (comboTime > 0)
                    {
                        if ((lastMove == DRG.DoomSpike || lastMove == DRG.DraconianFury) && level >= Levels.SonicThrust)
                            return DRG.SonicThrust;
                        if (lastMove == DRG.SonicThrust && level >= Levels.CoerthanTorment)
                            return DRG.CoerthanTorment;
                    }

                    return originalHook(DRG.DoomSpike);
                }

                if (actionID == DRG.ChaosThrust || actionID == DRG.ChaoticSpring)
                {
                    var gauge = GetJobGauge<DRGGauge>();
                    if (gauge.FirstmindsFocusCount == 2)
                        return DRG.WyrmwindThrust;

                    if (comboTime > 0)
                    {
                        if ((lastMove == DRG.TrueThrust || lastMove == DRG.RaidenThrust) && level >= Levels.Disembowel)
                            return DRG.Disembowel;
                        if (lastMove == DRG.Disembowel && level >= Levels.ChaosThrust)
                            return originalHook(DRG.ChaosThrust);
                    }

                    if (HasEffect(Buffs.SharperFangAndClaw))
                        return DRG.FangAndClaw;

                    if (HasEffect(Buffs.EnhancedWheelingThrust))
                        return DRG.WheelingThrust;

                    return originalHook(DRG.TrueThrust);
                }

                if (actionID == DRG.FullThrust || actionID == DRG.HeavensThrust)
                {
                    var gauge = GetJobGauge<DRGGauge>();
                    if (gauge.FirstmindsFocusCount == 2)
                        return DRG.WyrmwindThrust;

                    if (comboTime > 0)
                    {
                        if ((lastMove == DRG.TrueThrust || lastMove == DRG.RaidenThrust) && level >= Levels.VorpalThrust)
                            return DRG.VorpalThrust;
                        if (lastMove == DRG.VorpalThrust && level >= Levels.FullThrust)
                            return originalHook(DRG.FullThrust);
                    }

                    if (HasEffect(Buffs.SharperFangAndClaw))
                        return DRG.FangAndClaw;

                    if (HasEffect(Buffs.EnhancedWheelingThrust))
                        return DRG.WheelingThrust;

                    return originalHook(DRG.TrueThrust);
                }

                return null;
            }
        }
    }
}
