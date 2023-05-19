using System;
using System.Collections.Generic;
using System.Linq;
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
        public int Rssi => 1;

        public object NativeDevice => null;
        public IList<AdvertisementRecord> AdvertisementRecords => Enumerable.Empty<AdvertisementRecord>().ToList();

        public void Dispose()
        {
            
        }

        public Task<IService> GetServiceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IService>(default);
        }

        public Task<IReadOnlyList<IService>> GetServicesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<IService>>(default);
        }

        public Task<int> RequestMtuAsync(int requestValue)
        {
            return Task.FromResult<int>(default);
        }

        public bool UpdateConnectionInterval(ConnectionInterval interval)
        {
            return interval == ConnectionInterval.Normal;
        }

        public Task<bool> UpdateRssiAsync()
        {
            return Task.FromResult<bool>(default);
        }
    }
}
