using Prism.Commands;

namespace AccountingUI.Core.Commands
{
    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand CloseAAllCommand { get; } = new CompositeCommand();
    }
}
