using Core.ViewModels;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
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
        private GMapMarker _boatMarker;
        public MapView()
        {
            InitializeComponent();

            Map.MapProvider = GMap.NET.MapProviders.GoogleHybridMapProvider.Instance;
            Map.Position = new GMap.NET.PointLatLng(-22.8833, -43.1034);
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 9;
            Map.DragButton = MouseButton.Left;
            Map.Manager.Mode = GMap.NET.AccessMode.ServerAndCache;

            AddBoatMarker(new PointLatLng(-22.8833, -43.1034));

        }
        ~MapView()
        {
            Map.Dispose(); //Necessary to prevent memory leaks that can cause the process to remain in memory after closing the application
        }

        private void AddBoatMarker(PointLatLng position)
        {
            _boatMarker = new GMapMarker(position)
            {
                Shape = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Stroke = Brushes.OrangeRed,
                    StrokeThickness = 1.5
                },
                Tag = "Boat"
            };

            Map.Markers.Add(_boatMarker);
            
        }


    }
}
