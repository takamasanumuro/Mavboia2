using Core.ViewModels;
using Core.Views;
using ReactiveUI;
using Splat;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RegisterDependencies();
        }

        private void RegisterDependencies()
        {
            var MainViewModel = new MainViewModel();
            Locator.CurrentMutable.RegisterConstant(MainViewModel, typeof(MainViewModel));
            Locator.CurrentMutable.RegisterConstant(MainViewModel, typeof(IScreen));
            Locator.CurrentMutable.RegisterLazySingleton(() => new DashboardViewModel(MainViewModel), typeof(DashboardViewModel));


            Locator.CurrentMutable.Register(() => new DashboardView(), typeof(IViewFor<DashboardViewModel>));
            Locator.CurrentMutable.Register(() => new BatteryView(), typeof(IViewFor<BatteryViewModel>));
        }
    }

}
