using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;

namespace XIVComboPlugin.Combos
{
    static class WAR
    {
        const uint
           StormsPath = 42,
           HeavySwing = 31,
           Maim = 37,
           StormsEye = 45,
           MythrilTempest = 16462,
           Overpower = 41,
           InnerBeast = 49,
           SteelCyclone = 51,
           FellCleave = 3549,
           Decimate = 3550,
           PrimalRend = 25753;

        static class Levels
        {
            public const byte
                Maim = 4,
                StormsPath = 26,
                MythrilTempest = 40,
                StormsEye = 50;
        }

        static class Buffs
        {
            public const short
                PrimalRendReady = 2624;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 3;
                this.JobID = 21;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;
                if (actionID == WAR.StormsPath)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == WAR.HeavySwing && level >= Levels.Maim)
                            return WAR.Maim;

                        if (lastMove == WAR.Maim && level >= Levels.StormsPath)
                            return WAR.StormsPath;
                    }

                    return WAR.HeavySwing;
                }

                if (actionID == WAR.StormsEye)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == WAR.HeavySwing && level >= Levels.Maim)
                            return WAR.Maim;

                        if (lastMove == WAR.Maim && level >= Levels.StormsEye)
                            return WAR.StormsEye;
                    }

                    return WAR.HeavySwing;
                }

                if (actionID == WAR.MythrilTempest)
                {
                    if (comboTime > 0)
                    {
                        if (lastMove == WAR.Overpower && level >= Levels.MythrilTempest)
                            return WAR.MythrilTempest;
                    }

                    return WAR.Overpower;
                }

                if (actionID == WAR.InnerBeast || actionID == WAR.SteelCyclone || actionID == WAR.FellCleave || actionID == WAR.Decimate)
                {
                    if (HasEffect(Buffs.PrimalRendReady))
                        return WAR.PrimalRend;
                }

                return null;
            }
        }
    }
}
