using Core.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Core.Views
{
    /// <summary>
    /// Interaction logic for BatteryView.xaml
    /// </summary>
    public partial class BatteryView : IViewFor<BatteryViewModel>
    {
        public BatteryView()
        {
            InitializeComponent();
        }

        public BatteryViewModel ViewModel
        {
            get { return (BatteryViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel",typeof(BatteryViewModel), typeof(BatteryView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (BatteryViewModel)value;
        }
    }
}
