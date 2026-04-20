using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using BudgetManager.Services;
using BudgetManager.ViewModels;

namespace BudgetManager
{
    public partial class App : Application
    {
        public App()
        {
            var services = new ServiceCollection();
            // Dependency Injection beállítás / Singleton, hogy az egész programban csak egy példány legyen
            services.AddSingleton<IEditorService, EditorService>();
            services.AddTransient<MainViewModel>(); // Transient, minden lekéréskor új példány legyen, habár főablaknál egy is elég
            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }
    }

}
