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
            var viewModelInstance = new MainViewModel();
            ViewModel = viewModelInstance;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(viewModelInstance, ViewModel => ViewModel.Router, View => View.RoutedViewHost.Router)
                    .DisposeWith(disposables);
                this.BindCommand(viewModelInstance, ViewModel => ViewModel.NavigateToBattery, View => View.ButtonBattery)
                    .DisposeWith(disposables);
                this.BindCommand(viewModelInstance, ViewModel => ViewModel.NavigateToDashboard, View => View.ButtonDashboard)
                    .DisposeWith(disposables);
            });

            
        }
    }
}