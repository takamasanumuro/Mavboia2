using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI;
using Core.ViewModels;
using System.Reactive.Disposables;
using Splat;

namespace Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class MainWindowBase : ReactiveWindow<MainViewModel> { }
    public partial class MainWindow : MainWindowBase
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = Locator.Current.GetService<MainViewModel>();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, ViewModel => ViewModel.Router, View => View.RoutedViewHost.Router)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, ViewModel => ViewModel.NavigateToBattery, View => View.ButtonBattery)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, ViewModel => ViewModel.NavigateToDashboard, View => View.ButtonDashboard)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, ViewModel => ViewModel.NavigateToGraphical, View => View.ButtonGraphical)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, ViewModel => ViewModel.NavigateToMap, View => View.ButtonMap);
                this.BindCommand(ViewModel, ViewModel => ViewModel.NavigateToDSB, View => View.ButtonDSB)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, ViewModel => ViewModel.NavigateToSettings, View => View.ButtonSettings);
            });

            
        }
    }
}