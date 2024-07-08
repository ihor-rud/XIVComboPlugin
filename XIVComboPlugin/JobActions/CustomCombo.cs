using System;
using System.Linq;

using Dalamud.Plugin.Services;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Game.ClientState.Statuses;

namespace XIVComboPlugin.Combos;

public interface ICustomCombo
{
    public uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook);
    public byte ClassId { get; }
    public byte JobId { get; }
}

public abstract class ComboHelpers<T>(IClientState clientState, IJobGauges jobGauges) : ICustomCombo
    where T : JobGaugeBase
{
    protected Status FindEffect(short effectId) => clientState
        .LocalPlayer?
        .StatusList
        .FirstOrDefault(status => status.StatusId == effectId);

    protected bool HasEffect(short effectId) => FindEffect(effectId) != null;
    protected byte PlayerLevel() => clientState.LocalPlayer.Level;
    protected T GetJobGauge() => jobGauges.Get<T>();

    public abstract uint? Invoke(uint actionId, uint lastMove, Func<uint, uint> originalHook);
    public abstract byte ClassId { get; }
    public abstract byte JobId { get; }
}
