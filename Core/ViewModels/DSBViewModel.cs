using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class DSBViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "DSB";
        public IScreen HostScreen { get; }

        public DSBViewModel(IScreen? screen)
        {
            HostScreen = screen ?? throw new ArgumentNullException(nameof(screen));
        }

    }
}
