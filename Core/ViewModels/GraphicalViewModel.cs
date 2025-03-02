using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class GraphicalViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Graphical";
        public IScreen HostScreen { get; }
        public GraphicalViewModel(IScreen? screen)
        {
            HostScreen = screen ?? throw new ArgumentNullException(nameof(screen));
        }
    }
}
