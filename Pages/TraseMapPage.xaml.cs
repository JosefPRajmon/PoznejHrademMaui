using Microsoft.Maui.Maps;
using PoznejHrademMaui.DataManager;
using PoznejHrademMaui.Models;

namespace PoznejHrademMaui.Pages;

public partial class TraseMapPage : ContentPage
{
    PlacesArrayControl placesArrayControl { get; set; }
    private List<MapPin> _pins;
    public List<MapPin> Pins
    {
        get { return _pins; }
        set { _pins = value; OnPropertyChanged(); }
    }
    public TraseMapPage()
    {
        InitializeComponent();
        CentralizeMap();
        BindingContext = this;
        Pins = new List<MapPin>()
        {

        };
        List<MapPin> customPins = new List<MapPin>();
        placesArrayControl = new PlacesArrayControl();
        List<string> LeftPlaces = placesArrayControl.AllPlaces.Keys.Except(placesArrayControl.EnigmaPosition).ToList();
        for (int i = 0; i < LeftPlaces.Count; i++)
        {
            string placeS = LeftPlaces[i];
            PlacesFab place = placesArrayControl.AllPlaces[placeS];
            MapPin pin = new MapPin(MapPinClicked);

            pin.Position = place.Position;
            pin.Tittle = placeS;
            pin.IconSrc = $"pin";

            Pins.Add(pin);
        }
        for (int i = 0; i < placesArrayControl.EnigmaPosition.Count; i++)
        {
            try
            {
                string placeS = placesArrayControl.EnigmaPosition[i];
                PlacesFab place = placesArrayControl.AllPlaces[placeS];
                MapPin pin = new MapPin(MapPinClicked);

                pin.Position = place.Position;
                pin.Tittle = placeS;
                pin.IconSrc = $"pin{i+1}";

                Pins.Add(pin);
            }

            catch (Exception)
            {

            }
        }
        /*Polyline polyline = new Polyline
        {
            StrokeColor = Colors.DarkOrange,
            StrokeWidth = 10,
            Geopath = {
            new Location( 49.144214,  15.002943),
            new Location(49.144173, 15.002935),
            new Location(49.144049, 15.002889),
            new Location(49.143914, 15.002889),
            new Location(49.143491, 15.002952),
            new Location(49.143474, 15.002755),
            new Location(49.143356, 15.002700),
            new Location(49.143174, 15.002333),
            new Location(49.143139, 15.002224),
            new Location(49.142886, 15.001507),
            new Location(49.142862, 15.001344),
            new Location(49.142839, 15.001128),
            new Location(49.142851, 15.001065),
            new Location(49.142862, 15.001012),
            new Location(49.142821, 15.000958),
            new Location(49.142815, 15.000868),
            new Location(49.142798, 15.000698),
            new Location(49.142786, 15.000608),
            new Location(49.142774, 15.000599),
            new Location(49.142774, 15.000599),
            new Location(49.142615, 15.000455),
            new Location(49.142545, 15.000320),
            new Location(49.142545, 15.000320),
            new Location(49.142615, 15.000455),
            new Location(49.142480, 15.000715),
            new Location(49.142375, 15.001317),
            new Location(49.142186, 15.001327),
            new Location(49.142110, 15.001452),
            new Location(49.142057, 15.001560),
            new Location(49.141969, 15.001452),
            new Location(49.141916, 15.001398),
            new Location(49.141863, 15.001388),
            new Location(49.141810, 15.001155),
            new Location(49.141810, 15.001155),
            new Location(49.141805, 15.001138),
            new Location(49.141752, 15.001065),
            new Location(49.141693, 15.001002),
            new Location(49.141558, 15.000949),
            new Location(49.141493, 15.000868),
            new Location(49.141429, 15.000778),
            new Location(49.141358, 15.000698),
            new Location(49.141276, 15.000616),
            new Location(49.141223, 15.000536),
            new Location(49.141158, 15.000375),
            new Location(49.141052, 14.999988),
            new Location(49.141023, 14.999862),
            new Location(49.141023, 14.999754),
            new Location(49.141052, 14.999629),
            new Location(49.141099, 14.999503),
            new Location(49.141076, 14.999512),
            new Location(49.141076, 14.999512),
            new Location(49.141023, 14.999520),
            new Location(49.140970, 14.999574),
            new Location(49.140923, 14.999646),
            new Location(49.140887, 14.999736),
            new Location(49.140853, 15.000050),
            new Location(49.140829, 15.000149),
            new Location(49.140800, 15.000230),
            new Location(49.140659, 15.000572),
            new Location(49.140576, 15.000491),
            new Location(49.140482, 15.000428),
            new Location(49.140406, 15.000375),
            new Location(49.140324, 15.000329),
            new Location(49.140236, 15.000312),
            new Location(49.140136, 15.000302),
            new Location(49.140024, 15.000312),
            new Location(49.139918, 15.000346),
            new Location(49.139613, 15.000436),
            new Location(49.139565, 15.000383),
            new Location(49.139448, 15.000113),
            new Location(49.139407, 15.000042),
            new Location(49.139354, 14.999970),
            new Location(49.139460, 14.999826),
            new Location(49.139448, 14.999736),
            new Location(49.139424, 14.999656),
            new Location(49.139354, 14.999449),
            new Location(49.139336, 14.999367),
            new Location(49.139342, 14.999287),
            new Location(49.139389, 14.999054),
            new Location(49.139413, 14.998694),
            new Location(49.139530, 14.998694),
            new Location(49.139660, 14.998658),
            new Location(49.139754, 14.998587),
            new Location(49.139813, 14.998560),
            new Location(49.139853, 14.998551),
            new Location(49.140006, 14.998551),
            new Location(49.140071, 14.998532),
            new Location(49.140124, 14.998505),
            new Location(49.140171, 14.998461),
            new Location(49.140259, 14.998352),
            new Location(49.140324, 14.998262),
            new Location(49.140324, 14.998262),
            new Location(49.140324, 14.998262),
            new Location(49.140377, 14.998146),
            new Location(49.140406, 14.998191),
            new Location(49.140441, 14.998218),
            new Location(49.140576, 14.998262),
            new Location(49.140764, 14.998299),
            new Location(49.141017, 14.998344),
            new Location(49.141223, 14.998380),
            new Location(49.141429, 14.998461),
            new Location(49.141540, 14.998505),
            new Location(49.141616, 14.998532),
            new Location(49.141693, 14.998551),
            new Location(49.141787, 14.998532),
            new Location(49.141952, 14.998551),
            new Location(49.142105, 14.998604),
            new Location(49.142257, 14.998685),
            new Location(49.142380, 14.998793),
            new Location(49.142563, 14.998838),
            new Location(49.142733, 14.998838),
            new Location(49.142921, 14.998811),
            new Location(49.143226, 14.998721),
            new Location(49.143244, 14.998865),
            new Location(49.143274, 14.998973),
            new Location(49.143309, 14.999098),
            new Location(49.143374, 14.999243),
            new Location(49.143497, 14.999566),
            new Location(49.143626, 14.999862),
            new Location(49.143668, 14.999988),
            new Location(49.143702, 15.000113),
            new Location(49.143732, 15.000249),
            new Location(49.143761, 15.000409),
            new Location(49.143797, 15.000823),
            new Location(49.143609, 15.000832),
            new Location(49.143579, 15.000823),
            new Location(49.143579, 15.000823),
            new Location(49.143609, 15.000832),
            new Location(49.143797, 15.000823),
            new Location(49.143820, 15.001128),
            new Location(49.143832, 15.001264),
            new Location(49.143820, 15.001388),
            new Location(49.143802, 15.001478),
            new Location(49.143885, 15.001507),
            new Location(49.144120, 15.001452),
            new Location(49.144202, 15.001443),
            new Location(49.144272, 15.001452),
            new Location(49.144291, 15.001461),
            new Location(49.144291, 15.001461),
            new Location(49.144319, 15.001478),
            new Location(49.144355, 15.001533),
            new Location(49.144425, 15.001470),
            new Location(49.144602, 15.001128),
            new Location(49.144649, 15.000742),
            new Location(49.144596, 15.000383),
            new Location(49.144761, 15.000375),
            new Location(49.144937, 15.000275),
            new Location(49.145007, 15.000401),
            new Location(49.145207, 15.000312),
            new Location(49.145331, 15.000230),
            new Location(49.145331, 15.000239),
            new Location(49.145331, 15.000239),
            new Location(49.145407, 15.000436),
            new Location(49.145565, 15.000616),
            new Location(49.145930, 15.000409),
            new Location(49.145983, 15.000329),
            new Location(49.146053, 15.000275),
            new Location(49.146094, 15.000230),
            new Location(49.146094, 15.000230),
            new Location(49.146141, 15.000176),
            new Location(49.146194, 14.999916),
            new Location(49.146206, 14.999826),
            new Location(49.146218, 14.999727),
            new Location(49.146194, 14.999494),
            new Location(49.146177, 14.999243),
            new Location(49.146194, 14.999153),
            new Location(49.146224, 14.999080),
            new Location(49.146271, 14.999008),
            new Location(49.146347, 14.998946),
            new Location(49.146424, 14.998865),
            new Location(49.146441, 14.998811),
            new Location(49.146458, 14.998738),
            new Location(49.146477, 14.998488),
            new Location(49.146523, 14.998218),
            new Location(49.146658, 14.997966),
            new Location(49.146729, 14.997876),
            new Location(49.146729, 14.997876),
            new Location(49.146747, 14.997859),
            new Location(49.146847, 14.997778),
            new Location(49.146952, 14.997696),
            new Location(49.147040, 14.997643),
            new Location(49.147117, 14.997822),
            new Location(49.147270, 14.998218),
            new Location(49.147446, 14.998712),
            new Location(49.147340, 14.998694),
            new Location(49.147405, 14.999727),
            new Location(49.147411, 14.999907),
            new Location(49.147816, 14.999862),
            new Location(49.147845, 15.000375),
            new Location(49.147851, 15.000464),
            new Location(49.147863, 15.000545),
            new Location(49.147887, 15.000616),
            new Location(49.147922, 15.000688),
            new Location(49.147957, 15.000895),
            new Location(49.148016, 15.001057),
            new Location(49.148133, 15.001299),
            new Location(49.148034, 15.001470),
            new Location(49.147481, 15.001874),
            new Location(49.146788, 15.002396),
            new Location(49.146788, 15.002396),
            new Location(49.146194, 15.002836),
            new Location(49.146135, 15.002889),
            new Location(49.146088, 15.002952),
            new Location(49.146059, 15.003015),
            new Location(49.146035, 15.003114),
            new Location(49.146035, 15.003212),
            new Location(49.146059, 15.003482),
            new Location(49.146071, 15.003688),
            new Location(49.146100, 15.004004),
            new Location(49.146088, 15.004237),
            new Location(49.146053, 15.004470),
            new Location(49.145983, 15.004749),
            new Location(49.145871, 15.005090),
            new Location(49.145795, 15.005306),
            new Location(49.145701, 15.005530),
            new Location(49.145601, 15.005746),
            new Location(49.145378, 15.006168),
            new Location(49.145295, 15.006321),
            new Location(49.145207, 15.006447),
            new Location(49.145072, 15.006501),
            new Location(49.144966, 15.006510),
            new Location(49.144866, 15.006482),
            new Location(49.144778, 15.006428),
            new Location(49.144696, 15.006321),
            new Location(49.144643, 15.006212),
            new Location(49.144584, 15.006088),
            new Location(49.144490, 15.006115),
            new Location(49.144396, 15.005854),
            new Location(49.144337, 15.005701),
            new Location(49.144361, 15.005612),
            new Location(49.144372, 15.005566),
            new Location(49.144361, 15.005486),
            new Location(49.144344, 15.005413),
            new Location(49.144319, 15.005360),
            new Location(49.144319, 15.005360),
            new Location(49.144244, 15.005199),
            new Location(49.144372, 15.004757),
            new Location(49.144238, 15.004578),
            new Location(49.144126, 15.004371),
            new Location(49.144032, 15.004157),
            new Location(49.143908, 15.003815),
            new Location(49.143908, 15.003815),
            new Location(49.143855, 15.003671),
            new Location(49.143938, 15.003509),
            new Location(49.144049, 15.002889),
            new Location(49.144173, 15.002935),
            new Location(49.144214, 15.002943),
                }

};
myMap.MapElements.Add(polyline);*/

    }
    private async void CentralizeMap()
    {
        try
        {
            myMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(49.14351999778172, 15.01047867232925), Distance.FromKilometers(2.3)));
        }
        catch (Exception e)
        {
        }

    }
    private void MapPinClicked(MapPin pin)
    {
        // Handle pin click
        Navigation.PushAsync(new PlacePage(placesArrayControl.AllPlaces[pin.Tittle]));
    }
}