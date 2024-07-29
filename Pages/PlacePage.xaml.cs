using PoznejHrademMaui.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PoznejHrademMaui.Pages;

public class Information : INotifyPropertyChanged
{
    private List<string> icons;
    public List<string> Icons
    {
        get
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
            {
                List<string> newIcons = icons;
                newIcons.Reverse();
                return newIcons;
            }
            return icons;
        }
        set
        {
            icons = value;
            OnPropertyChanged("Icons");
        }
    }
    public string Info { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
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
                        inf.Icons = new List<string>() { "location_icon.svg", "light_location_icon.svg" };
                        break;
                    case "tel":
                        inf.Icons = new List<string>() { "phone_icon.svg", "light_phone_icon.svg" };
                        break;
                    case "mail":
                        inf.Icons = new List<string>() { "email_icon.svg", "light_email_icon.svg" };
                        break;
                    case "web":
                        inf.Icons = new List<string>() { "web_icon.svg", "light_web_icon.svg" };
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

                if (datasouce.Icons[0] =="location_icon.svg")
                {
                    IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(datasouce.Info);

                    Location location = locations?.FirstOrDefault();
                    var options = new MapLaunchOptions { Name = Title };
                    await Map.Default.OpenAsync(location, options);
                }
                else if (datasouce.Icons[0].Contains("phone_icon.svg"))
                    PhoneDialer.Default.Open(datasouce.Info);
                else if (datasouce.Icons[0].Contains("email_icon.svg"))
                    await Email.Default.ComposeAsync(new EmailMessage() { Subject = datasouce.Info });
                else if (datasouce.Icons[0].Contains("web_icon.svg"))
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
