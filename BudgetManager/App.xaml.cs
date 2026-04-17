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
            services.AddSingleton<IEditorService, EditorService>();
            services.AddTransient<MainViewModel>();
            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }
    }

}
