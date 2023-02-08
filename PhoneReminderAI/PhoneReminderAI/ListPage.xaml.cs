using System;
using System.Collections.ObjectModel;
using Plugin.BLE;
using System.ComponentModel;
using Xamarin.Forms;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Forms.Xaml;
using System.Runtime.CompilerServices;
using IDevice = Plugin.BLE.Abstractions.Contracts.IDevice;
using System.Threading;
using Plugin.BLE.Abstractions;
using Plugin.LocalNotification;
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
        public IDevice CurDevice;
      
        public ListPage()
        {

            InitializeComponent();
            
            ble = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;

            deviceList = new ObservableCollection<DeviceViewModel>();
            DevicesList.ItemsSource = deviceList;
  
            this.BindingContext = this;
            StartCheckingDeviceState();
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

                deviceList.Clear();
                Adapter.ScanMode = ScanMode.Balanced;
                IsScanning = true;
                if (!ble.Adapter.IsScanning)
                {
                    var cts = new CancellationTokenSource();
                    cts.CancelAfter(5000);
                    Adapter.DeviceDiscovered += (s, a) =>
                    {
                        deviceList.Add(new DeviceViewModel() { Device = a.Device, DeviceName = a.Device.Name, DeviceType = (MyDevice)a.Device});
                    };
                    await Adapter.StartScanningForDevicesAsync(cancellationToken: cts.Token);
                }
            }
            catch (Exception ex)
            {
                IsScanning = false;
                await DisplayAlert("Notice", ex.Message.ToString(), "Error !");
            }
            finally
            {
                if (ble.Adapter.IsScanning)
                {
                    await Adapter.StopScanningForDevicesAsync();
                    
                }
            }

            IsScanning = false; 
            
            var device = new DeviceViewModel() {DeviceName = "BMW X5", DeviceType = new MyDevice() { State = DeviceState.Disconnected, Name = "BMW X5" }};
        
            deviceList.Add(device);
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


        private async void DevicesList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            if (DevicesList.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select a device from the list first.", "OK");
                return;
            }
            CurDevice = ((DeviceViewModel)DevicesList.SelectedItem).DeviceType;

            if (CurDevice != null)
            {
                if (CurDevice.State == DeviceState.Disconnected)
                {
                    var result = await DisplayAlert("Message", "Do you want to connect to this device?", "Connect", "Cancel");
                    if (result)
                    {
                        try
                        {
                            StartCheckingDeviceState();
                            var crf = new CancellationToken();
                            await Adapter.ConnectToDeviceAsync(CurDevice,ConnectParameters.None, cancellationToken: crf);
                            await DisplayAlert("Message", "Connected to device with Id: " + CurDevice.Id, "OK");
                           
                        }
                        catch (DeviceConnectionException ex)
                        {
                            await DisplayAlert("Error", ex.Message, "OK");
                            DevicesList.SelectedItem = null;
                        }
                        catch (InvalidCastException ex)
                        {
                            await DisplayAlert("Error", "The cast is not valid. Error: " + ex.Message, "OK");
                            DevicesList.SelectedItem = null;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Try again", "OK");
                        DevicesList.SelectedItem = null;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Device is already connected or being connected", "OK");
                    DevicesList.SelectedItem = null;
                }
            }
        }
    }
}

