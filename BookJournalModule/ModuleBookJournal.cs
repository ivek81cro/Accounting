using BookJournalModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace BookJournalModule
{
    public class ModuleBookJournal : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<JournalView>();
        }
    }
}
