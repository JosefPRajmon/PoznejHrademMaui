using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;
using PoznejHrademMaui.Components;
using PoznejHrademMaui.DataManager;

namespace PoznejHrademMaui.Platforms.Android
{
    [BroadcastReceiver(Label = "Tajenka")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/appwidgetprovider")]
    public class MyAppWidgetProvider : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            base.OnUpdate(context, appWidgetManager, appWidgetIds);

            // Načtěte data widgetu zde
            Task.Run(async () =>
            {
                var databaseService = new DatabaseService(); // Inicializujte svou databázovou službu
                var scannedCodes = await databaseService.GetScannedCodesAsync();

                for (int i = 0; i < appWidgetIds.Length; i++)
                {
                    var widgetId = appWidgetIds[i];
                    var remoteViews = new RemoteViews(context.PackageName, Resource.Layout.widget_layout);

                    // Aktualizujte obsah widgetu s daty
                    if (scannedCodes.Any())
                    {
                        string textLine = "Tajenka: ";
                        foreach (ScannedCode item in scannedCodes)
                        {
                            if (item.Enigma.Length>1)
                            {
                                textLine += $" {item.Enigma}";
                            }
                            else
                            {
                                textLine += $" xxx";
                            }
                        }

                        remoteViews.SetTextViewText(Resource.Id.widgetTextView, textLine);
                    }
                    else
                    {
                        remoteViews.SetTextViewText(Resource.Id.widgetTextView, "Žádná data");
                    }

                    appWidgetManager.UpdateAppWidget(widgetId, remoteViews);
                }
            });
        }
    }
}
