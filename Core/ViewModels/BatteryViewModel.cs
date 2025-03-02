using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class BatteryViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Battery";
        public IScreen HostScreen { get; }

        [Reactive]
        public BatteryModel Battery { get; set; }

        public BatteryViewModel(IScreen? screen)
        {
            HostScreen = screen ?? throw new ArgumentNullException(nameof(screen));

            Battery = new BatteryModel
            {
                Voltage = 48.5,
                Current = 0.5,
                SOC = 0.5,
                Temperature1 = 25.0,
                Temperature2 = 0.5,
                MaxCellVoltage = 4.2,
                MinCellVoltage = 3.2,
                ChargeMOSState = true,
                DischargeMOSState = false
            };

        }
    }
}
