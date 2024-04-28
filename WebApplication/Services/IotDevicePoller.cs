using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HomeDev.DbEntities;
using Newtonsoft.Json;

namespace HomeDev.Services
{
    public class IotDevicePoller : IHostedService
    {
        private Timer _timer = null;
        private readonly ILogger<IotDevicePoller> _logger;
        readonly IConfiguration _configuration;
        private readonly IotDevice iotDevice;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly int _pollSeconds;


        public IotDevicePoller(
            ILogger<IotDevicePoller> logger,
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._scopeFactory = scopeFactory;

            iotDevice = new IotDevice(this._logger, _configuration.GetValue<string>("IodDevice:Url"));
            _pollSeconds = _configuration.GetValue<int>("IodDevice:PollSeconds");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ActionToBePerformedAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(this._pollSeconds));
            return Task.CompletedTask;
        }

        async void ActionToBePerformedAsync(object state)
        {
            // ottengo lo stato dell'aria condizionata e dell'ambiente
            AirConditioner acStatus = await iotDevice.GetAcStatus();
            ClimaHistory clima = await iotDevice.GetClima();

            if (acStatus == null || clima == null)
            {
                _logger.LogError($"Unable to connect to iotdevice at {iotDevice._url}");
                return;
            }

            // salvo i valori a db
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await _context.CreateOrUpdate_ClimaHistory(clima);
                await _context.CreateOrUpdate_AirConditioner(acStatus);
                _context.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}