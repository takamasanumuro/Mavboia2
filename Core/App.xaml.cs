using Core.ViewModels;
using Core.Views;
using ReactiveUI;
using Splat;
using System.Configuration;
using System.Data;
using System.Windows;
using Application = System.Windows.Application;

namespace Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private Core.Services.MavlinkCommand mavlinkCommand;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RegisterDependencies();
            var mavlinkCommand = new Core.Services.MavlinkCommand();
            mavlinkCommand.RunMavlink();
        }

        private void RegisterDependencies()
        {
            var mainViewModel = new MainViewModel();
            Locator.CurrentMutable.RegisterConstant(mainViewModel, typeof(MainViewModel));
            Locator.CurrentMutable.RegisterConstant(mainViewModel, typeof(IScreen));
            Locator.CurrentMutable.RegisterLazySingleton(() => new DashboardViewModel(mainViewModel), typeof(DashboardViewModel));


            Locator.CurrentMutable.Register(() => new DashboardView(), typeof(IViewFor<DashboardViewModel>));
            Locator.CurrentMutable.Register(() => new BatteryView(), typeof(IViewFor<BatteryViewModel>));
            Locator.CurrentMutable.Register(() => new GraphicalView(), typeof(IViewFor<GraphicalViewModel>));
            Locator.CurrentMutable.Register(() => new MapView(), typeof(IViewFor<MapViewModel>));
            Locator.CurrentMutable.Register(() => new DSBView(), typeof(IViewFor<DSBViewModel>));
            Locator.CurrentMutable.Register(() => new SettingsView(), typeof(IViewFor<SettingsViewModel>));
        }
    }

}
