using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Settings";
        public IScreen HostScreen { get; }

        public SettingsViewModel(IScreen? screen)
        {
            HostScreen = screen ?? throw new ArgumentNullException(nameof(screen));
        }

    }
}
