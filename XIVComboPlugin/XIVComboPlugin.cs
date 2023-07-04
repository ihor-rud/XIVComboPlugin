using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace XIVComboPlugin
{
    class XIVComboPlugin : IDalamudPlugin
    {
        public string Name => "XIV Combo Plugin";

        [PluginService] public static SigScanner TargetModuleScanner { get; private set; } = null!;
        [PluginService] public static ClientState ClientState { get; private set; } = null!;
        [PluginService] public static JobGauges JobGauges { get; private set; } = null!;

        private readonly IconReplacer iconReplacer;

        public XIVComboPlugin()
        {
            this.iconReplacer = new IconReplacer(TargetModuleScanner, ClientState, JobGauges);
            this.iconReplacer.Enable();
        }

        public void Dispose()
        {
            this.iconReplacer.Dispose();
        }
    }
}
