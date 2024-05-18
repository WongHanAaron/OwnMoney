using Microsoft.Extensions.Logging;
using OwnMoney.Ui.Services;
using OwnMoney.Ui.Services.Environment;
using OwnMoney.Ui.Views.Dashboard;
using OwnMoney.Ui.Views.Transactions;
using Prism;
using Prism.DryIoc;
using Syncfusion.Maui.Core.Hosting;

namespace OwnMoney.Ui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder()
            .ConfigureSyncfusionCore()
            .UseMauiApp<App>()
            .UsePrism(prism =>
                prism.ConfigureModuleCatalog(moduleCatalog =>
                {
                    //moduleCatalog.AddModule<MauiAppModule>();
                    //moduleCatalog.AddModule<MauiTestRegionsModule>();
                })
                .RegisterTypes(containerRegistry =>
                {
                    containerRegistry.AddOwnMoneyViews();

                    var collection = new ContainerRegistryAdapter(containerRegistry);
                    collection.AddOwnMoneyUi();

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
                    .AddSegment<TransactionsViewModel>()
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
