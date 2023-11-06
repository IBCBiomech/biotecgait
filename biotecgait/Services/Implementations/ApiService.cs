using biotecgait.Services.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Messaging;
using WisewalkSDK;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using static WisewalkSDK.Wisewalk;
using System.Linq;
using System;
using biotecgait.Messages.Api;

namespace biotecgait.Services.Implementations
{
    public class ApiService : IApiService
    {
        private Wisewalk api;
        private List<Dev> scanDevices;
        private Dictionary<string, Device> devicesConnected;
        public string? port_selected;
        public string? error;
        private List<float> latencies = new();
        private const int N_LATENCIES_AVG = 100;
        private float latency = 0.0f;
        private Stopwatch stopwatch = new();
        public ApiService()
        {
            api = new Wisewalk();
            api.scanFinished += scanFinishedCallback;
            api.deviceConnected += deviceConnectedCallback;
            api.deviceDisconnected += deviceDisconnectedCallback;
            api.updateDeviceConfiguration += updateDeviceConfigurationCallback;
            api.updateDeviceRTC += updateDeviceRTCCallback;
            api.dataReceived += dataReceivedCallback;
            devicesConnected = new Dictionary<string, Device>();
        }
        #region SCAN
        public async void Scan()
        {
            Trace.WriteLine("Scan from ApiService");
            await Task.Run(() => ScanDevices());
        }
        public void ScanDevices()
        {
            ShowPorts();
            api.Open(port_selected, out error);
            if (!api.ScanDevices(out error))
            {
                // Error
                Trace.WriteLine("", "Error to scan devices - " + error);
            }
            else
            {
                Thread.Sleep(2000);
            }
        }
        public void ShowPorts()
        {
            var ports = api.GetUsbDongles();
            foreach (ComPort port in ports)
            {
                Match match1 = Regex.Match(port.description, "nRF52 USB CDC BLE*", RegexOptions.IgnoreCase);
                if (match1.Success)
                {
                    port_selected = port.name;
                    Trace.WriteLine(port.description);
                }
            }
        }
        private void scanFinishedCallback(List<Dev> devices)
        {
            scanDevices = devices;
            Trace.WriteLine("# of devices: " + devices.Count);
            ShowScanList(scanDevices);
            List<InsoleScanData> insoles = new();
            for (int i = 0; i < scanDevices.Count; i++)
            {
                string name = "Wisewalk";
                InsoleScanData insole = new InsoleScanData(name, GetMacAddress(scanDevices[i]));
                insoles.Add(insole);
            }
            WeakReferenceMessenger.Default.Send(new InsolesScanMessage(insoles));
        }
        private string GetMacAddress(Dev device)
        {
            string mac = "";

            mac = device.mac[5].ToString("X2") + ":" + device.mac[4].ToString("X2") + ":" + device.mac[3].ToString("X2") + ":" +
                                    device.mac[2].ToString("X2") + ":" + device.mac[1].ToString("X2") + ":" + device.mac[0].ToString("X2");

            return mac;
        }
        private void ShowScanList(List<Dev> devices)
        {
            for (int idx = 0; idx < devices.Count; idx++)
            {
                string macAddress = devices[idx].mac[5].ToString("X2") + ":" + devices[idx].mac[4].ToString("X2") + ":" + devices[idx].mac[3].ToString("X2") + ":" +
                                    devices[idx].mac[2].ToString("X2") + ":" + devices[idx].mac[1].ToString("X2") + ":" + devices[idx].mac[0].ToString("X2");


                Trace.WriteLine("MacAddress: ", " * " + macAddress);
            }
        }
        #endregion SCAN
        #region CONNECT
        public void Connect(List<string> macs)
        {
            Trace.WriteLine("onConnectMessageReceived");
            List<Dev> conn_list_dev = new List<Dev>();
            foreach (string mac in macs)
            {
                Trace.WriteLine(mac);
                conn_list_dev.Add(findInsole(mac));
            }
            if (!api.Connect(conn_list_dev, out error))
            {
                Trace.WriteLine("Connect error " + error);
            }
        }
        private Dev findInsole(string mac)
        {
            return scanDevices.FirstOrDefault(de => GetMacAddress(de) == mac);
        }
        private void deviceConnectedCallback(byte handler, Device dev)
        {
            WeakReferenceMessenger.Default.Send(new InsoleConnectedMessage(dev.Id));
            devicesConnected[handler.ToString()] = dev;
            api.SetDeviceConfiguration(handler, 100, 3, out error);
        }
        #endregion
        #region DISCONNECT
        public async void Disconnect(List<string> macs)
        {
            List<int> device_handlers = new List<int>();
            foreach (string mac in macs)
            {
                device_handlers.Add(findHandler(mac));
            }
            if (!api.Disconnect(device_handlers, out error))
            {
                Trace.WriteLine("Disconnect error " + error);
            }
            await Task.Delay(2000);
            foreach (string mac in macs)
            {
                WeakReferenceMessenger.Default.Send(new InsoleDisconnectedMessage(mac));
            }
        }
        private byte findHandler(string mac)
        {
            string handler = devicesConnected.Where(d => d.Value.Id == mac).FirstOrDefault().Key;
            return byte.Parse(handler);
        }
        private void deviceDisconnectedCallback(byte handler)
        {
            Device device = devicesConnected[handler.ToString()];
            WeakReferenceMessenger.Default.Send(new InsoleDisconnectedMessage(device.Id));
            devicesConnected.Remove(handler.ToString());
        }
        #endregion DISCONNECT
        #region CONFIGURATION
        private void updateDeviceConfigurationCallback(byte handler, byte sambleRate, byte packetType)
        {
            api.SetRTCDevice(handler, GetDateTime(), out error);
        }
        private DateTime GetDateTime()
        {
            DateTime dateTime = new DateTime(2022, 11, 8, 13, 0, 0, 0);
            return dateTime;
        }
        private void updateDeviceRTCCallback(byte handler, DateTime dateTime)
        {
            Device device = api.GetDevicesConnected()[handler.ToString()];
            WeakReferenceMessenger.Default.Send(new InsoleHeaderInfoMessage(device.Id, device.HeaderInfo.fwVersion, device.HeaderInfo.battery));
        }
        #endregion CONFIGURATION
        #region DATA RECEIVED
        private void dataReceivedCallback(byte deviceHandler, WisewalkData data)
        {
            // Stopwatch comienza en StartStream en el método Capture (ApiService)

            if (stopwatch.IsRunning)
            {
                // Después de 100 paquetes que se tienen que capturar 2 segundos.


                latencies.Add((float)stopwatch.Elapsed.TotalSeconds);
                if (latencies.Count >= N_LATENCIES_AVG)
                {
                    for (int i = 0; i < latencies.Count; i++)
                    {
                        latency += latencies[i] - 0.04f * (i / 2); // Cada grupo paquete de cuatro datos recibidos son 0.04s e i/2 por las plantillas L y R
                    }
                    latency /= latencies.Count;  // Calculas la media.

                    Trace.WriteLine("latency = " + latency);
                    stopwatch.Reset();
                }
            }

            List<InsoleCaptureData> measures = new List<InsoleCaptureData>();
            foreach (var sole in data.Sole)
            {
                measures.Add(new InsoleCaptureData(sole));
            }
            WeakReferenceMessenger.Default.Send(new InsoleCapturePacketData(deviceHandler, measures));
        }
        #endregion DATA RECEIVED
        public async void Capture()
        {
            Trace.WriteLine("Start from ApiService");
            api.SetDeviceConfiguration(0, 100, 3, out error);
            api.SetDeviceConfiguration(1, 100, 3, out error);
            await Task.Delay(2000);
            api.StartStream(out error);
            stopwatch.Restart();
        }
        public void Stop()
        {
            if (!api.StopStream(out error))
            {
                Trace.WriteLine(error);
            }
        }
        public void Pause()
        {
            if (!api.StopStream(out error))
            {
                Trace.WriteLine(error);
            }
        }
        public void Resume()
        {
            if (!api.StartStream(out error))
            {
                Trace.WriteLine(error);
            }
        }
        public float Latency()
        {
            return latency;
        }
    }
}
