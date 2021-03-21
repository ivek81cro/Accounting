using Prism.Commands;

namespace AccountingUI.Core.Commands
{
    public interface IApplicationCommands
    {
        CompositeCommand CloseAAllCommand { get; }
    }
}
