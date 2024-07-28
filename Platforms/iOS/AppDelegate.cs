using Foundation;
using UIKit;

namespace PoznejHrademMaui
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        {
            if (shortcutItem.Type == "cz.poznejmesto.scanqr")
            {
                // Handle the QR scan action
                StartQRScanner();
            }

            // Call the base method if needed, though it's not required in this case
            base.PerformActionForShortcutItem(application, shortcutItem, completionHandler);
        }

        private void StartQRScanner()
        {
            var shell = (AppShell)App.Current.MainPage;
            shell.GoToAsync($"enigma?camera=true");
        }
    }
}
