using AvsCard.Wpf.Test.Views;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using System.Windows;

namespace AvsCard.Wpf.Test
{
    public partial class App : PrismApplication
    {
        #region Protected Methods

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AvsClientTestView>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate("ContentRegion", nameof(AvsClientTestView));
        }

        #endregion Protected Methods
    }
}