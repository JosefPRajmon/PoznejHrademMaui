using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using PoznejHrademMaui.Platforms.Android;

namespace PoznejHrademMaui
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class WidgetUpdateReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            AppWidgetManager appWidgetManager = AppWidgetManager.GetInstance(context);
            ComponentName widgetComponent = new ComponentName(context, Java.Lang.Class.FromType(typeof(MyAppWidgetProvider)));

            // Zavolejte aktualizační metodu widgetu
            int[] appWidgetIds = appWidgetManager.GetAppWidgetIds(widgetComponent);
            MyAppWidgetProvider widgetProvider = new MyAppWidgetProvider();
            widgetProvider.OnUpdate(context, appWidgetManager, appWidgetIds);
        }


    }

    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : MauiAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Define the intent for the QR code scan
            var scanQrCodeIntent = new Intent(this, typeof(MainActivity));
            scanQrCodeIntent.SetAction("cz.poznejmesto.scanqr");

            var scanQrCodeShortcut = new ShortcutInfo.Builder(this, "scan_qr")
                .SetShortLabel("Skenuj")
                .SetLongLabel("Skenuj do tajenky")
                .SetIcon(Icon.CreateWithResource(this, Resource.Drawable.photo_camera_svgrepo_com))
                .SetIntent(scanQrCodeIntent)
                .Build();

            var shortcutManager = (ShortcutManager)GetSystemService(ShortcutService);
            shortcutManager.SetDynamicShortcuts(new List<ShortcutInfo> { scanQrCodeShortcut });
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent.Action == "cz.poznejmesto.scanqr")
            {
                // Handle the QR scan action
                StartQRScanner();
            }
        }

        private void StartQRScanner()
        {
            // Your code to start the QR scanner
            var shell = (AppShell)App.Current.MainPage;
            shell.GoToAsync($"enigma?camera=true");
        }
    }
}
