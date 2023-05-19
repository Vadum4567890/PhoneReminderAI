using System;
using System.Collections.ObjectModel;
using Plugin.BLE;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms.Xaml;
using System.Runtime.CompilerServices;
using IDevice = Plugin.BLE.Abstractions.Contracts.IDevice;
using System.Threading;
using Plugin.BLE.Abstractions;
using Plugin.LocalNotification;
using Xamarin.Essentials;
using Exception = System.Exception;
using ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode;


namespace PhoneReminderAI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage, INotifyPropertyChanged
    {
        public IBluetoothLE ble;
        public IAdapter Adapter;
        public ObservableCollection<DeviceViewModel> deviceList;
        public ObservableCollection<IDevice> _deviceList;
        public IDevice CurDevice;

        public ListPage()
        {

            InitializeComponent();

            ble = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;
            Adapter.ScanMode = ScanMode.LowLatency;
            ble.StateChanged += (s, e) =>
            {
                Debug.WriteLine("StateChanged: {0}", e.NewState);
            };
            deviceList = new ObservableCollection<DeviceViewModel>();
            _deviceList = new ObservableCollection<IDevice>();

            Adapter.DeviceConnected += async (s, a) =>
            {
                Debug.WriteLine("Device Connected");
                await DisplayAlert("Device Connected", a.Device.Name.ToString(), "Cancel");
            };
            Adapter.DeviceAdvertised += (s, a) =>
            {
                Debug.WriteLine("Device Advertised");
            };
            Adapter.DeviceConnectionLost += async (s, a) =>
            {
                Debug.WriteLine("Device ConnectionLost");
                await DisplayAlert("Device ConnectionLost", a.Device.Name.ToString(), "Cancel");
            };
            Adapter.DeviceDisconnected += (s, a) =>
            {
                Debug.WriteLine("Device Disconnected");

            };
            //Adapter.ScanTimeoutElapsed += async (s, a) =>
            //{

            //    await DisplayAlert("Device ScanTimeout", "", "Cancel");
            //};
            Adapter.DeviceDiscovered += (s, a) =>
            {
                Debug.WriteLine("DebugDiscovered");

                _deviceList.Add(a.Device);
            };
            DevicesList.ItemsSource = _deviceList;

            // this.BindingContext = this;
            //   StartCheckingDeviceState();
        }



        private bool _isScanning = false;
        public bool IsScanning
        {
            get { return _isScanning; }
            set
            {
                _isScanning = value;
                OnPropertyChanged();
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (ble.State == BluetoothState.Off)
                {
                    await DisplayAlert("Message", "Bluetooth is not available. Please turn on your Bluetooth", "OK");
                    return;
                }
                _deviceList.Clear();
                deviceList.Clear();
                Adapter.ScanMode = ScanMode.Balanced;
                // Adapter.ScanTimeout = 10000;
                IsScanning = true;
                //var scanfilterobj = new ScanFilterOptions { };
                await Adapter.StartScanningForDevicesAsync(allowDuplicatesKey: true);
            }
            catch (Exception ex)
            {
                IsScanning = false;
                await DisplayAlert("Notice", ex.Message.ToString(), "Error !");
            }


            IsScanning = false;


            //var _device = new MyDevice() { Id = Guid.NewGuid(), Name = "Iphone 14", State = DeviceState.Disconnected };
            //_deviceList.Add(_device);
        }



        public Timer Timer;

        private void StartCheckingDeviceState()
        {
            if (CurDevice == null)
            {

            }
            else
            {
                //Timer = new Timer(CheckDeviceState, null, 0, 1000);
                CheckDeviceState();
            }
        }

        private void CheckDeviceState()
        {
            if (CurDevice.State == DeviceState.Disconnected)
            {
                SendNotificationDis();
            }
            else if (CurDevice.State == DeviceState.Connected)
            {
                SendNotificationCon();
            }
        }

        private async void SendNotificationCon()
        {
            var notification = new NotificationRequest()
            {
                BadgeNumber = 1,
                //Description = "Your Device is connected",
                Title = "Your Device is connected",
                ReturningData = "Some Data",
            };
            await LocalNotificationCenter.Current.Show(notification);
        }

        private async void SendNotificationDis()
        {
            var notification = new NotificationRequest()
            {
                BadgeNumber = 1,
                Description = "Trip was ended!",
                Title = "Take your phone!",
                ReturningData = "Some Data",
            };
            await LocalNotificationCenter.Current.Show(notification);
        }

        private IDevice device;
        private async void DevicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            device = DevicesList.SelectedItem as IDevice;
            var result = await DisplayAlert("Message", "Do you want to connect to this device?", "Connect", "Cancel");
            if (!result)
            {
                return;
            }

            await Adapter.StopScanningForDevicesAsync();
            try
            {
                MainThread.BeginInvokeOnMainThread(async () => await Adapter.ConnectToDeviceAsync(device, new ConnectParameters(true, true)));
                await DisplayAlert("Connect", $"Status: {device.State}", "Ok");
            }
            catch (InvalidCastException ex)
            {
                await DisplayAlert("Error", ex.Message, "Cancel");
            }
            catch (DeviceConnectionException exception)
            {
                await DisplayAlert("Error", exception.Message, "Cancel");
            }

        }
    }
}

    