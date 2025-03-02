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
    public class MapViewBase : ReactiveUserControl<MapViewModel> { }
    public partial class MapView : MapViewBase
    {
        public MapView()
        {
            InitializeComponent();
        }
    }
}
