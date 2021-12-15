using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    public static class RDM
    {
        public const uint
            Verthunder = 7505,
            Veraero = 7507,
            Veraero2 = 16525,
            Verthunder2 = 16524,
            Redoublement = 7516,
            Zwerchhau = 7512,
            Riposte = 7504,
            Scatter = 7509,
            Verstone = 7511,
            Verfire = 7510,
            Jolt = 7503,
            Verholy = 7526,
            Verflare = 7525,
            Scorch = 16530,
            Resolution = 25858;

        public static class Buffs
        {
            public const short
                Swiftcast = 167,
                VerfireReady = 1234,
                VerstoneReady = 1235,
                Dualcast = 1249;
        }

        public static class Levels
        {
            public const byte
                Zwerchhau = 35,
                Redoublement = 50,
                Scorch = 80,
                Resolution = 90;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 0;
                this.JobID = 35;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == RDM.Veraero2)
                {
                    if (HasEffect(Buffs.Swiftcast) || HasEffect(Buffs.Dualcast))
                        return originalHook(RDM.Scatter);
                }

                if (actionID == RDM.Verthunder2)
                {
                    if (HasEffect(Buffs.Swiftcast) || HasEffect(Buffs.Dualcast))
                        return originalHook(RDM.Scatter);
                }

                if (actionID == RDM.Redoublement)
                {
                    if (lastMove == RDM.Riposte && level >= Levels.Zwerchhau)
                        return originalHook(RDM.Zwerchhau);

                    if (lastMove == RDM.Zwerchhau && level >= Levels.Redoublement)
                        return originalHook(RDM.Redoublement);

                    return originalHook(RDM.Riposte);
                }

                if (actionID == RDM.Verthunder)
                {
                    if (level >= Levels.Scorch && (lastMove == RDM.Verflare || lastMove == RDM.Verholy))
                        return RDM.Scorch;

                    if (level >= Levels.Resolution && lastMove == RDM.Scorch)
                        return RDM.Resolution;

                    if (GetJobGauge<RDMGauge>().ManaStacks == 3)
                        return originalHook(RDM.Verthunder2);

                    if (HasEffect(Buffs.Swiftcast) || HasEffect(Buffs.Dualcast))
                        return originalHook(RDM.Verthunder);

                    if (HasEffect(Buffs.VerfireReady))
                        return RDM.Verfire;

                    return originalHook(RDM.Jolt);
                }

                if (actionID == RDM.Veraero)
                {
                    if (level >= Levels.Scorch && (lastMove == RDM.Verflare || lastMove == RDM.Verholy))
                        return RDM.Scorch;

                    if (level >= Levels.Resolution && lastMove == RDM.Scorch)
                        return RDM.Resolution;

                    if (GetJobGauge<RDMGauge>().ManaStacks == 3)
                        return originalHook(RDM.Veraero2);

                    if (HasEffect(Buffs.Swiftcast) || HasEffect(Buffs.Dualcast))
                        return originalHook(RDM.Veraero);

                    if (HasEffect(Buffs.VerstoneReady))
                        return RDM.Verstone;

                    return originalHook(RDM.Jolt);
                }

                return null;
            }
        }
    }
}
