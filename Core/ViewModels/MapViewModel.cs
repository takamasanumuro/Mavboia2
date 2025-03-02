using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class MapViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Map";
        public IScreen HostScreen { get; }

        public MapViewModel(IScreen? screen)
        {
            HostScreen = screen ?? throw new ArgumentNullException(nameof(screen));
        }

    }
}
