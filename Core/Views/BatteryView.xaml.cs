using Core.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// 

    public class BatteryLevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int batteryLevel)
            {
                if (batteryLevel <= 20)
                    return Brushes.Red;
                else if (batteryLevel <= 60)
                    return Brushes.Orange;
                else
                    return Brushes.Green;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BatteryViewBase : ReactiveUserControl<BatteryViewModel> { }

    public partial class BatteryView : BatteryViewBase
    {
        public BatteryView()
        {
            InitializeComponent();
        }
    }
}
