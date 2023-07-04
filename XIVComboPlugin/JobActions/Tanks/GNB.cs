using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;

namespace XIVComboPlugin.Combos
{
    static class GNB
    {
        const uint
           KeenEdge = 16137,
           BrutalShell = 16139,
           SolidBarrel = 16145,
           DemonSlice = 16141,
           DemonSlaughter = 16149,
           GnashingFang = 16146,
           Continuation = 16155;

        static class Buffs
        {
            public const short
                ReadyToRip = 1842,
                ReadyToTear = 1843,
                ReadyToGouge = 1844;
        }

        static class Levels
        {
            public const byte
                BrutalShell = 4,
                SolidBarrel = 26,
                DemonSlaughter = 40;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 0;
                this.JobID = 37;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == GNB.SolidBarrel)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == GNB.KeenEdge && level >= Levels.BrutalShell)
                            return GNB.BrutalShell;
                        if (lastMove == GNB.BrutalShell && level >= Levels.SolidBarrel)
                            return GNB.SolidBarrel;
                    }

                    return GNB.KeenEdge;
                }

                if (actionID == GNB.GnashingFang)
                {
                    if (HasEffect(Buffs.ReadyToRip) || HasEffect(Buffs.ReadyToTear) || HasEffect(Buffs.ReadyToGouge))
                        return originalHook(GNB.Continuation);
                }

                if (actionID == GNB.DemonSlaughter)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == GNB.DemonSlice && level >= Levels.DemonSlaughter)
                            return GNB.DemonSlaughter;
                    }

                    return GNB.DemonSlice;
                }

                return null;
            }
        }
    }
}
