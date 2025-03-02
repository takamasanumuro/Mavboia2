using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class TemplateViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Template";
        public IScreen HostScreen { get; }

        public TemplateViewModel(IScreen? screen)
        {
            HostScreen = screen ?? throw new ArgumentNullException(nameof(screen));
        }

    }
}
