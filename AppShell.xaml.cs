

namespace PoznejHrademMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("enigma", typeof(PoznejHrademMaui.Pages.EnigmaPage));
        }

        public Task GoToEnigmaPage(bool camera)
        {
            return GoToAsync($"enigma?camera={camera}");
        }
    }
}
