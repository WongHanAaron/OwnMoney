using OwnMoney.Ui.Services.Environment;
using OwnMoney.Ui.Views.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnMoney.Ui.Services
{
    public static class ServiceRegistration
    {   
        public static void AddOwnMoneyUi(this IServiceCollection collection)
        {
            collection.AddSingleton<DashboardPage>();
            collection.AddSingleton<IWindowSizeListener, WindowSizeListener>();
        }
    }
}
