using Camera.MAUI;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PoznejHrademMaui.DataManager;

namespace PoznejHrademMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .UseMauiCommunityToolkit()
                        .ConfigureMauiHandlers(handlers =>
                        {
#if ANDROID
                            handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, PoznejHrademMaui.Platforms.Android.CustomMapHandler>();
#elif IOS
            handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, PoznejHrademMaui.Platforms.iOS.CustomMapHandler>();
#endif
                        })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<DatabaseService>();

            return builder.Build();
        }
    }
}
