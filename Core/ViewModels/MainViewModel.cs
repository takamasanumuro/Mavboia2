using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Splat;
using System.Reactive.Linq;
using ReactiveUI.Fody.Helpers;


namespace Core.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToDashboard { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToBattery { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToGraphical { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToMap { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToDSB { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToSettings { get;}


        public MainViewModel()
        {

            NavigateToDashboard = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new DashboardViewModel(this)));

            NavigateToBattery = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new BatteryViewModel(this)));

            NavigateToGraphical = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new GraphicalViewModel(this)));

            NavigateToMap = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new MapViewModel(this)));

            NavigateToDSB = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new DSBViewModel(this)));

            NavigateToSettings = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new SettingsViewModel(this)));

            Router.Navigate.Execute(new DashboardViewModel(this));
        }
    }
}
