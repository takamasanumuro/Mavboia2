using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class DashboardViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Dashboard";
        public IScreen HostScreen { get; }

        public DashboardViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
