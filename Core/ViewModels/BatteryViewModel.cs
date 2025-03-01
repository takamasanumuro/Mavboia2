using ReactiveUI;
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

        public BatteryViewModel(IScreen? screen)
        {
            HostScreen = screen ?? throw new ArgumentNullException(nameof(screen));
        }

    }
}
