using Dalamud.Game;
using Dalamud.Plugin.Services;
using Dalamud.Plugin;

namespace XIVComboPlugin
{
    class XIVComboPlugin : IDalamudPlugin
    {
        public string Name => "XIV Combo Plugin";

        public ISigScanner TargetModuleScanner { get; init; }
        public IClientState ClientState { get; init; }
        public IJobGauges JobGauges { get; init; }
        public IGameInteropProvider HookProvider { get; init; }

        private readonly IconReplacer iconReplacer;

        public XIVComboPlugin()
        {
            this.iconReplacer = new IconReplacer(TargetModuleScanner, ClientState, JobGauges, HookProvider);
            this.iconReplacer.Enable();
        }

        public void Dispose()
        {
            this.iconReplacer.Dispose();
        }
    }
}
