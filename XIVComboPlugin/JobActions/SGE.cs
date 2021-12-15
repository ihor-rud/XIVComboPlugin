﻿using System;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboPlugin.Combos
{
    public static class SGE
    {
        public const uint
            Diagnosis = 24284,
            Druochole = 24296,
            Dyskrasia = 24297,
            Toxikon = 24304;

        public static class Levels
        {
            public const byte
                Druochole = 45,
                Toxikon = 66;
        }

        public class Combo : CustomCombo
        {
            public Combo(ClientState clientState, JobGauges jobGauges) : base(clientState, jobGauges)
            {
                this.ClassID = 0;
                this.JobID = 40;
            }

            public override ulong? Invoke(uint actionID, uint lastMove, float comboTime, Func<uint, ulong> originalHook)
            {
                var level = this.clientState.LocalPlayer.Level;

                if (actionID == SGE.Diagnosis)
                {
                    var gauge = GetJobGauge<SGEGauge>();
                    if (gauge.Addersgall > 0 && !gauge.Eukrasia)
                        return SGE.Druochole;
                }

                if (actionID == SGE.Dyskrasia)
                {
                    if (level >= Levels.Toxikon && GetJobGauge<SGEGauge>().Addersting > 0)
                        return SGE.Toxikon;
                }

                return null;
            }
        }
    }
}
