using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game;
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

        public unsafe IconReplacer(SigScanner scanner, ClientState clientState, JobGauges jobGauges)
        {
            this.clientState = clientState;

            Address = new IconReplacerAddressResolver();
            Address.Setup(scanner);

            var actionmanager = (byte*)ActionManager.Instance();
            comboTimer = (IntPtr)(actionmanager + 0x60);
            lastComboMove = comboTimer + 0x4;

            iconHook = Hook<OnGetIconDelegate>.FromAddress(Address.GetIcon, GetIconDetour);
            checkerHook = Hook<OnCheckIsIconReplaceableDelegate>.FromAddress(Address.IsIconReplaceable, (_) => 1);

            CustomCombo[] comboList = {
                // Tanks
                new PLD.Combo(clientState, jobGauges),
                new WAR.Combo(clientState, jobGauges),
                new DRK.Combo(clientState, jobGauges),
                new GNB.Combo(clientState, jobGauges),

                // Melee DPS
                new MNK.Combo(clientState, jobGauges),
                new DRG.Combo(clientState, jobGauges),
                new NIN.Combo(clientState, jobGauges),
                new SAM.Combo(clientState, jobGauges),
                new RPR.Combo(clientState, jobGauges),

                // Physical Ranged DPS
                new BRD.Combo(clientState, jobGauges),
                new MCH.Combo(clientState, jobGauges),
                new DNC.Combo(clientState, jobGauges),

                // Magical Ranged DPS
                new BLM.Combo(clientState, jobGauges),
                new SMN.Combo(clientState, jobGauges),
                new RDM.Combo(clientState, jobGauges),

                // Healers
                new WHM.Combo(clientState, jobGauges),
                new SCH.Combo(clientState, jobGauges),
                new AST.Combo(clientState, jobGauges),
                new SGE.Combo(clientState, jobGauges),
            };

            foreach (var item in comboList)
            {
                combos[item.ClassID] = item;
                combos[item.JobID] = item;
            }
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
            if (clientState.LocalPlayer == null)
            {
                return iconHook.Original(self, actionID);
            }

            var lastMove = Marshal.ReadInt32(lastComboMove);
            var comboTime = Marshal.PtrToStructure<float>(comboTimer);
            var jobId = this.clientState.LocalPlayer.ClassJob.Id;

            var combo = combos[jobId];
            var action = combo.Invoke(actionID, (uint)lastMove, comboTime, (actionID) => iconHook.Original(self, actionID));
            return action ?? iconHook.Original(self, actionID);
        }
    }
}
