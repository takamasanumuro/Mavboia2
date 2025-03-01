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
    public partial class DashboardView : IViewFor<DashboardViewModel>
    {
        public DashboardView()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                disposables(this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext));
            });
        }

        public DashboardViewModel ViewModel
        {
            get { return (DashboardViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (DashboardViewModel)value;
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(DashboardViewModel), typeof(DashboardView), new PropertyMetadata(null));
    }
}
