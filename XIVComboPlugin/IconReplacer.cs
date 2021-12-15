using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Hooking;
using XIVComboPlugin.Combos;

namespace XIVComboPlugin
{
    public class IconReplacer
    {
        public delegate ulong OnCheckIsIconReplaceableDelegate(uint actionID);

        public delegate ulong OnGetIconDelegate(byte param1, uint param2);

        private readonly IconReplacerAddressResolver Address;
        private readonly Hook<OnCheckIsIconReplaceableDelegate> checkerHook;

        private readonly ClientState clientState;
        private readonly IntPtr comboTimer;

        private readonly Dictionary<uint, CustomCombo> combos = new();
        private readonly Hook<OnGetIconDelegate> iconHook;
        private readonly IntPtr lastComboMove;

        public IconReplacer(SigScanner scanner, ClientState clientState, JobGauges jobGauges)
        {
            this.clientState = clientState;

            Address = new IconReplacerAddressResolver();
            Address.Setup(scanner);

            comboTimer = scanner.GetStaticAddressFromSig("48 89 2D ?? ?? ?? ?? 85 C0 74 0F");
            lastComboMove = comboTimer + 0x4;
            iconHook = new Hook<OnGetIconDelegate>(Address.GetIcon, GetIconDetour);
            checkerHook = new Hook<OnCheckIsIconReplaceableDelegate>(Address.IsIconReplaceable, (_) => 1);

            // tank
            CustomCombo combo = new PLD.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new WAR.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new DRK.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new GNB.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            // male dps
            combo = new MNK.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new DRG.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new NIN.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new SAM.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new RPR.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            // range p dps
            combo = new BRD.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new MCH.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new DNC.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            // range m dps
            combo = new BLM.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new SMN.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new RDM.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            // healer
            combo = new WHM.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new SCH.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new AST.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;

            combo = new SGE.Combo(clientState, jobGauges);
            combos[combo.ClassID] = combo;
            combos[combo.JobID] = combo;
        }

        public void Enable()
        {
            iconHook.Enable();
            checkerHook.Enable();
        }

        public void Dispose()
        {
            iconHook.Dispose();
            checkerHook.Dispose();
        }

        private ulong GetIconDetour(byte self, uint actionID)
        {
            if (clientState.LocalPlayer == null) return iconHook.Original(self, actionID);

            var lastMove = Marshal.ReadInt32(lastComboMove);
            var comboTime = Marshal.PtrToStructure<float>(comboTimer);
            var jobId = this.clientState.LocalPlayer.ClassJob.Id;

            var combo = combos.GetValueOrDefault(jobId);
            if (combo != null)
            {
                var action = combo.Invoke(actionID, (uint)lastMove, comboTime, (actionID) => iconHook.Original(self, actionID));
                if (action.HasValue)
                {
                    return action.Value;
                }
            }

            return iconHook.Original(self, actionID);
        }
    }
}
