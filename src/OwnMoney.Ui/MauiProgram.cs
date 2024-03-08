using Microsoft.Extensions.Logging;
using OwnMoney.Ui.Views.Dashboard;
using Prism;

namespace OwnMoney.Ui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .UsePrism(prism =>
                prism.ConfigureModuleCatalog(moduleCatalog =>
                {
                    //moduleCatalog.AddModule<MauiAppModule>();
                    //moduleCatalog.AddModule<MauiTestRegionsModule>();
                })
                .RegisterTypes(containerRegistry =>
                {
                    containerRegistry.RegisterGlobalNavigationObserver();
                    containerRegistry.RegisterForNavigation<DashboardPage>();
                    //containerRegistry.RegisterForNavigation<RootPage>();
                    //containerRegistry.RegisterForNavigation<SamplePage>();
                    //containerRegistry.RegisterForNavigation<SplashPage>();
                })
                .AddGlobalNavigationObserver(context => context.Subscribe(x =>
                {
                    if (x.Type == NavigationRequestType.Navigate)
                        Console.WriteLine($"Navigation: {x.Uri}");
                    else
                        Console.WriteLine($"Navigation: {x.Type}");

                    var status = x.Cancelled ? "Cancelled" : x.Result.Success ? "Success" : "Failed";
                    Console.WriteLine($"Result: {status}");

                    if (status == "Failed" && !string.IsNullOrEmpty(x.Result?.Exception?.Message))
                        Console.Error.WriteLine(x.Result.Exception.Message);
                }))

                .OnAppStart(navigationService => navigationService.CreateBuilder()
                    .AddSegment<DashboardViewModel>()
                    .Navigate(HandleNavigationError))
            )
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void HandleNavigationError(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
