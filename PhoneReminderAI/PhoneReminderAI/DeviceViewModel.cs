using Plugin.BLE.Abstractions.Contracts;

namespace PhoneReminderAI
{
    public class DeviceViewModel    
    {
        public IDevice Device { get; set; }
        public string DeviceName { get; set; }
        public MyDevice DeviceType { get; set; }
    }
}