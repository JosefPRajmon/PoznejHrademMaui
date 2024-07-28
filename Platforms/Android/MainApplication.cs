using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Runtime;
using PoznejHrademMaui.Platforms.Android;

namespace PoznejHrademMaui
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            Task.Run(async () =>
            {
                int i = 0;
                while (i<35)
                {
                    OnPauseForMe();
                    await Task.Delay(10000);
                    i++;
                }

            });

        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        public void OnPauseForMe()
        {

            // Získání kontextu aplikace

            SendWidgetUpdateBroadcast();
        }

        public void SendWidgetUpdateBroadcast()
        {
            var context = ApplicationContext;
            var appWidgetManager = AppWidgetManager.GetInstance(context);
            var widgetComponent = new ComponentName(context, Java.Lang.Class.FromType(typeof(MyAppWidgetProvider)));
            var appWidgetIds = appWidgetManager.GetAppWidgetIds(widgetComponent);

            var intent = new Intent(AppWidgetManager.ActionAppwidgetUpdate);
            intent.PutExtra(AppWidgetManager.ExtraAppwidgetIds, appWidgetIds);
            context.SendBroadcast(intent);
        }
    }
}
