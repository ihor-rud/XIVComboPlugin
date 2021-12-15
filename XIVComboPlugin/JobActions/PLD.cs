using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.Statuses;

namespace XIVComboPlugin.Combos
{
    public static class PLD
    {
        public const uint
            FastBlade = 9,
            RiotBlade = 15,
            RageOfHalone = 21,
            RoyalAuthority = 3539,
            Atonement = 16460,
            TotalEclipse = 7381,
            Prominence = 16457,
            HolySpirit = 7384,
            HolyCircle = 16458,
            Confiteor = 16459;

        public static class Buffs
        {
            public const short
                Requiescat = 1368,
                SwordOath = 1902;
        }

        public static class Levels
        {
            public const byte
                RiotBlade = 4,
                RageOfHalone = 26,
                Prominence = 40,
                RoyalAuthority = 60,
                Confiteor = 80;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 1;
                this.JobID = 19;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;
                if (actionID == PLD.RoyalAuthority || actionID == PLD.RageOfHalone)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == PLD.FastBlade && level >= Levels.RiotBlade)
                            return PLD.RiotBlade;

                        if (lastMove == PLD.RiotBlade && level >= Levels.RageOfHalone)
                            return originalHook(PLD.RageOfHalone);
                    }

                    if (HasEffect(Buffs.SwordOath))
                    {
                        return PLD.Atonement;
                    }

                    return PLD.FastBlade;
                }

                if (actionID == PLD.Prominence)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == PLD.TotalEclipse && level >= Levels.Prominence)
                            return PLD.Prominence;
                    }

                    return PLD.TotalEclipse;
                }

                if (actionID == PLD.HolySpirit || actionID == PLD.HolyCircle)
                {
                    var x = originalHook(PLD.Confiteor);
                    if (x != PLD.Confiteor)
                        return x;

                    Status requiescat = FindEffect(Buffs.Requiescat);

                    if (requiescat != null)
                    {
                        if (requiescat.StackCount <= 1 && level >= PLD.Levels.Confiteor)
                        {
                            return originalHook(PLD.Confiteor);
                        }
                    }
                }

                return null;
            }
        }
    }
}
