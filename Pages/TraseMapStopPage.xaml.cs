using PoznejHrademMaui.DataManager;
using PoznejHrademMaui.Models;

namespace PoznejHrademMaui.Pages;

public partial class TraseMapStopPage : ContentPage
{
    List<PlacesFab> placesFabs { get; set; }
    public TraseMapStopPage()
    {
        InitializeComponent();
        PlacesArrayControl placesArrayControl = new PlacesArrayControl();
        colection.ItemsSource = placesArrayControl.AllPlaces.Values;


    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Grid grid = (Grid)sender;
        PlacesFab placesFabLoc = (PlacesFab)grid.BindingContext;
        Navigation.PushAsync(new PlacePage(placesFabLoc));
    }
}