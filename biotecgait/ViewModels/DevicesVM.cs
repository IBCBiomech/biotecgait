using biotecgait.Messages.Api;
using biotecgait.Models;
using biotecgait.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace biotecgait.ViewModels
{
    public partial class DevicesVM : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<InsoleModel> insoles;
        public DevicesVM() 
        {
            WeakReferenceMessenger.Default.Register<InsolesScanMessage>(this, HandleInsolesScanMessage);
            WeakReferenceMessenger.Default.Register<InsoleConnectedMessage>(this, HandleInsoleConnectedMessage);
            WeakReferenceMessenger.Default.Register<InsoleDisconnectedMessage>(this, HandleInsoleDisconnectedMessage);
            WeakReferenceMessenger.Default.Register<InsoleHeaderInfoMessage>(this, HandleInsoleHeaderInfoMessage);
        }
        private void HandleInsolesScanMessage(object recipient, InsolesScanMessage message)
        {
            Insoles = new ObservableCollection<InsoleModel>();
            foreach(InsoleScanData insole in message.Insoles)
            {
                Insoles.Add(new InsoleModel(insole.name, insole.MAC));
            }
        }
        private void HandleInsoleConnectedMessage(object recipient, InsoleConnectedMessage message)
        {
            InsoleModel insole = Insoles.First((InsoleModel insole) => insole.Mac == message.MAC);
            insole.Connected = true;
        }
        private void HandleInsoleDisconnectedMessage(object recipient, InsoleDisconnectedMessage message)
        {
            InsoleModel insole = Insoles.First((InsoleModel insole) => insole.Mac == message.MAC);
            insole.Connected = false;
        }
        private void HandleInsoleHeaderInfoMessage(object recipient, InsoleHeaderInfoMessage message)
        {
            InsoleModel insole = Insoles.First((InsoleModel insole) => insole.Mac == message.MAC);
            insole.Fw = message.fwVersion;
            insole.Battery = message.battery;
        }
        [RelayCommand]
        public void Scan()
        {
            App.ServiceProvider.GetService<IApiService>().Scan();
        }
        [RelayCommand]
        public void Connect()
        {
            List<string> macs = new List<string>();
            App.ServiceProvider.GetService<IApiService>().Connect(macs);
        }
        [RelayCommand]
        public void Disconnect()
        {
            List<string> macs = new List<string>();
            App.ServiceProvider.GetService<IApiService>().Disconnect(macs);
        }
        [RelayCommand]
        public void Capture()
        {
            App.ServiceProvider.GetService<IApiService>().Capture();
        }
        [RelayCommand]
        public void Stop()
        {
            App.ServiceProvider.GetService<IApiService>().Stop();
        }
        [RelayCommand]
        public void Pause()
        {
            App.ServiceProvider.GetService<IApiService>().Pause();
        }
        [RelayCommand]
        public void Resume()
        {
            App.ServiceProvider.GetService<IApiService>().Resume();
        }
    }
}
