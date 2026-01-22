using Microsoft.Extensions.Logging;
using RedoRepository_Sam_Paula.Services;
using RedoRepository_Sam_Paula.ViewModels;
using RedoRepository_Sam_Paula.Views;

namespace RedoRepository_Sam_Paula
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Services
            builder.Services.AddSingleton<ScenarioStore>();
            builder.Services.AddSingleton<MortgageCalculator>();

            // ViewModels
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<NewScenarioViewModel>();

            // Pages
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<NewScenarioPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
