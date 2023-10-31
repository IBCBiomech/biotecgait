using biotecgait.Services.Implementations;
using biotecgait.Services.Interfaces;
using biotecgait.ViewModels;
using biotecgait.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace biotecgait
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ServiceProvider = BuildServiceProvider();
            MainWindow = new MainWindow(new MainWindowVM());
            MainWindow.Show();
        }
        private IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IApiService, ApiService>();
            services.AddSingleton<DevicesView>();
            services.AddSingleton<DevicesVM>();
            return services.BuildServiceProvider();
        }
    }
}
