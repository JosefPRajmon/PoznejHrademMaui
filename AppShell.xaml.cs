

namespace PoznejHrademMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Infocenter.Command = new Command(async () => await Browser.OpenAsync("https://infocentrum.jh.cz/"));
            Routing.RegisterRoute("enigma", typeof(PoznejHrademMaui.Pages.EnigmaPage));
            Application.Current.Resources["EnigmaPage"] = "Formulář";
        }

        public Task GoToEnigmaPage(bool camera)
        {
            return GoToAsync($"enigma?camera={camera}");
        }
    }
}
