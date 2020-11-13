using Iot.Device.DHTxx;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Blazor.Pi.Server.Hubs;
using Blazor.Pi.Shared;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Pi.Server.Services
{
    internal class DHTService : IHostedService, IDisposable
    {
        private readonly Dht11 _dht;
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IHubContext<GPIOHub> _hubContext;

        public DHTService(
            ILogger<DHTService> logger,
            Dht11 dhtController,
            IHubContext<GPIOHub> hubContext
            )
        {
            _hubContext = hubContext;
            _dht = dhtController;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_logger.LogDebug("DHT Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(2));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            //_logger.LogDebug($"DHT Service is working: {DateTime.Now}");
            var temperature = new TemperatureData {
                Temperature = _dht.Temperature.Celsius,
                Humidity = _dht.Humidity,
                IsLastReadSuccessful = _dht.IsLastReadSuccessful,
            };
            //_logger.LogDebug($"Read Success: {temperature.IsLastReadSuccessful}");
            if (temperature.IsLastReadSuccessful)
            {
                _hubContext.Clients.All.SendAsync("ReceiveDhtStatus",$"Temperature: {_dht.Temperature.Celsius.ToString("0.0")} °C, Humidity: {_dht.Humidity.ToString("0.0")} %");
            }
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //_logger.LogDebug("DHT Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
