namespace PoznejHrademMaui.Pages;

public partial class HowAndWhyPlayPage : ContentPage
{
    private List<string> tutorialsInformation = new List<string>()
    {
        (string)Application.Current.Resources["tutorialInformationLineOne"],
        (string)Application.Current.Resources["tutorialInformationLineTwo"],
    };
    private List<string> tutorials = new List<string>()
    {
        (string)Application.Current.Resources["tutorialLineOne"],
        (string)Application.Current.Resources["tutorialLineTwo"],
        (string)Application.Current.Resources["tutorialLineThree"],
        (string)Application.Current.Resources["tutorialLineFour"],
        (string)Application.Current.Resources["tutorialLineFive"],
        (string)Application.Current.Resources["tutorialLineSix"],
        (string)Application.Current.Resources["tutorialLineSeven"],
        (string)Application.Current.Resources["tutorialLineEight"],
        (string)Application.Current.Resources["tutorialLineNine"],
    };
    public HowAndWhyPlayPage()
    {
        InitializeComponent();
        tutorialInformationColection.ItemsSource = tutorialsInformation;
        tutorialColection.ItemsSource = tutorials;
    }
}