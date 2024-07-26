using PoznejHrademMaui.Models;

namespace PoznejHrademMaui.Pages
{
    public partial class MainPage : ContentPage
    {
        private List<string> Items = new List<string>() {
            (string)Application.Current.Resources["firstMPItem"],
            (string)Application.Current.Resources["secondMPItem"],
            (string)Application.Current.Resources["threeMPItem"],
            (string)Application.Current.Resources["fourMPItem"],
            (string)Application.Current.Resources["fiveMPItem"],
                    };
        private List<string> ItemsHistory = new List<string>()
        {
            (string)Application.Current.Resources["HistoryMPFirst"],
            (string)Application.Current.Resources["HistoryMPSecond"],
            (string)Application.Current.Resources["HistoryMPThree"],
            (string)Application.Current.Resources["HistoryMPFour"],
            (string)Application.Current.Resources["HistoryMPFive"],
            (string)Application.Current.Resources["HistoryMPSix"],
            (string)Application.Current.Resources["HistoryMPSeven"],
            (string)Application.Current.Resources["HistoryMPEight"],
        };
        public MainPage()
        {
            InitializeComponent();
            BindingContext=this;
            ScreenHelper screenHelper = new ScreenHelper();
            aboutCityColection.ItemsSource = Items;
            HistoryCityColection.ItemsSource= ItemsHistory;

            //   testing1.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 3.6;
            infoColize.Text = "Počet obyvatel 20 475 (k 31.12.2022) \nRozloha: 74, 27 km2 \nNadmořská výška: 478 m n. m.";


        }

        private void scroll_SizeChanged(object sender, EventArgs e)
        {
            double a = ((ScrollView)sender).Height;
            Absolutelay.HeightRequest = a;
            testing.HeightRequest = a;
            imageTit.HeightRequest = a;
            testing.WidthRequest = ((ScrollView)sender).Width;
            imageTit.WidthRequest = ((ScrollView)sender).Width;
        }
    }

}
