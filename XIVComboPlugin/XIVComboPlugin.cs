using Dalamud.Game;
using Dalamud.Plugin.Services;
using Dalamud.Plugin;

namespace XIVComboPlugin
{
    class XIVComboPlugin : IDalamudPlugin
    {
        public string Name => "XIV Combo Plugin";
        private readonly IconReplacer iconReplacer;

        public XIVComboPlugin(ISigScanner scanner, IClientState clientState, IJobGauges jobGauges, IGameInteropProvider hookProvider)
        {
            this.iconReplacer = new IconReplacer(scanner, clientState, jobGauges, hookProvider);
            this.iconReplacer.Enable();
        }

        public void Dispose()
        {
            this.iconReplacer.Dispose();
        }
    }
}
