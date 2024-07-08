using Dalamud.Game;
using Dalamud.Plugin.Services;
using Dalamud.Plugin;
using System.Collections.Generic;
using XIVComboPlugin.Combos;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace XIVComboPlugin;

public delegate uint OnCheckIsIconReplaceableDelegate(uint actionId);
public delegate uint OnGetIconDelegate(byte param1, uint param2);

class XIVComboPlugin : IDalamudPlugin
{
    public static string Name => "XIV Combo Plugin";

    private readonly IClientState clientState;
    private readonly Dictionary<uint, ICustomCombo> combos = [];

    private readonly Hook<OnCheckIsIconReplaceableDelegate> checkerHook;
    private readonly Hook<OnGetIconDelegate> iconHook;

    public XIVComboPlugin(ISigScanner scanner, IClientState clientState, IJobGauges jobGauges, IGameInteropProvider hookProvider, IPluginLog logger)
    {
        this.clientState = clientState;

        iconHook = hookProvider.HookFromAddress<OnGetIconDelegate>(ActionManager.Addresses.GetAdjustedActionId.Value, GetIconDetour);
        checkerHook = hookProvider.HookFromAddress<OnCheckIsIconReplaceableDelegate>(scanner.ScanText("40 53 48 83 EC 20 8B D9 48 8B 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 85 C0 74 1F"), (_) => 1);

        ICustomCombo[] comboList = [
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
            new PCT.Combo(clientState, jobGauges),

            // Healers
            new WHM.Combo(clientState, jobGauges),
            new SCH.Combo(clientState, jobGauges),
            new AST.Combo(clientState, jobGauges),
            new SGE.Combo(clientState, jobGauges),
        ];

        foreach (var item in comboList)
        {
            combos[item.ClassId] = item;
            combos[item.JobId] = item;
        }

        iconHook.Enable();
        checkerHook.Enable();
    }

    public void Dispose()
    {
        iconHook.Dispose();
        checkerHook.Dispose();
    }

    private unsafe uint GetIconDetour(byte self, uint actionId)
    {
        if (clientState.LocalPlayer == null)
        {
            return iconHook.Original(self, actionId);
        }

        var lastMove = ActionManager.Instance()->Combo.Action;
        var jobId = clientState.LocalPlayer.ClassJob.Id;

        if (!combos.ContainsKey(jobId))
        {
            return iconHook.Original(self, actionId);
        }

        var combo = combos[jobId];
        var action = combo.Invoke(actionId, lastMove, (actionId) => iconHook.Original(self, actionId));
        return action ?? iconHook.Original(self, actionId);
    }
}
