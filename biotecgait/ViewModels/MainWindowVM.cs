using biotecgait.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace biotecgait.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        [ObservableProperty]
        ContentControl currentView; 
        public MainWindowVM()
        {
            currentView = App.ServiceProvider.GetService<DevicesView>();
        }
    }
}
