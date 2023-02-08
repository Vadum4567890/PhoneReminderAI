using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;

namespace PhoneReminderAI
{
    public class MyDevice : IDevice
    {
        public Guid Id { get; set; }
        public DeviceState State { get; set; }
        public string Name { get; set; }
        public int Rssi => throw new NotImplementedException();
        public object NativeDevice => throw new NotImplementedException();
        public IList<AdvertisementRecord> AdvertisementRecords => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IService> GetServiceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<IService>> GetServicesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> RequestMtuAsync(int requestValue)
        {
            throw new NotImplementedException();
        }

        public bool UpdateConnectionInterval(ConnectionInterval interval)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRssiAsync()
        {
            throw new NotImplementedException();
        }
    }
}
