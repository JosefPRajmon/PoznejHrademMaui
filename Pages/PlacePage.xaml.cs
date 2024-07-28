using PoznejHrademMaui.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PoznejHrademMaui.Pages;

public class Information
{
    public string Icon { get; set; }
    public string Info { get; set; }
}

public partial class PlacePage : ContentPage, INotifyPropertyChanged
{
    private ObservableCollection<Information> _informationn;
    public ObservableCollection<Information> Informationn
    {
        get => _informationn;
        set
        {
            _informationn = value;
            OnPropertyChanged();
        }
    }

    private PlacesFab _place;
    public PlacesFab Place
    {
        get => _place;
        set
        {
            _place = value;
            OnPropertyChanged();
        }
    }

    public PlacePage(PlacesFab placesFab)
    {
        InitializeComponent();
        Place = placesFab;
        BindingContext = this;
        Title = placesFab.Title;

        ObservableCollection<Information> list = new ObservableCollection<Information>();
        if (Place.Information.Length > 1)
        {
            string[] test = Place.Information.Split("--");
            foreach (string s in test)
            {
                string[] split = s.Split(":");
                Information inf = new Information();
                switch (split[0])
                {
                    case "map":
                        inf.Icon = "location_icon.svg";
                        break;
                    case "tel":
                        inf.Icon = "phone_icon.svg";
                        break;
                    case "mail":
                        inf.Icon = "email_icon.svg";
                        break;
                    case "web":
                        inf.Icon = "web_icon.svg";
                        break;
                }
                inf.Info = split[1].Trim();
                list.Add(inf);
            }
        }
        Informationn = list;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ScrollView_SizeChanged(object sender, EventArgs e)
    {
        ScrollView scrollView = (ScrollView)sender;
        PhotosGalery.HeightRequest = scrollView.Height;
        PhotosGalery.WidthRequest = scrollView.Width;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            Grid grid = (Grid)sender;
            Information datasouce = (Information)grid.BindingContext;
            try
            {

                if (datasouce.Icon =="location_icon.svg")
                {
                    IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(datasouce.Info);

                    Location location = locations?.FirstOrDefault();
                    var options = new MapLaunchOptions { Name = Title };
                    await Map.Default.OpenAsync(location, options);
                }
                else if (datasouce.Icon =="phone_icon.svg")
                    PhoneDialer.Default.Open(datasouce.Info);
                else if (datasouce.Icon =="email_icon.svg")
                    await Email.Default.ComposeAsync(new EmailMessage() { Subject = datasouce.Info });
                else if (datasouce.Icon =="web_icon.svg")
                    await Browser.Default.OpenAsync(datasouce.Info, BrowserLaunchMode.SystemPreferred);

            }
            catch (Exception)
            {
                bool copy = await DisplayAlert("chyba", "Nìco se nepovedlo, chcete to zkopírovat do schránky?", "ANO", "NE");
                if (copy)
                {
                    Clipboard.SetTextAsync(datasouce.Info);
                }
            }
        });

    }
}
