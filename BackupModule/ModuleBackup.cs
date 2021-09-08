using BackupModule.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace BackupModule
{
    public class ModuleBackup : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BackupView>();
        }
    }
}
