using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using peasysdemo.Models;
using peasysdemo.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;


namespace peasysdemo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("SplineSansMono-VariableFont_wght.ttf", "SplineSansMono");
                    // fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    // fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Adding connection service
            builder.Services.AddSingleton<IDBConnectionService, DBConnexionService>();

            // Register pages with DI
            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<Sales>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<Configuration>();

            return builder.Build();
        }
    }
}
