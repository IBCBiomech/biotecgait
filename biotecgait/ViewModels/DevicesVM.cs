using biotecgait.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace biotecgait.ViewModels
{
    public partial class DevicesVM : ObservableObject
    {
        [RelayCommand]
        public void Scan()
        {
            App.ServiceProvider.GetService<IApiService>().Scan();
        }
    }
}
