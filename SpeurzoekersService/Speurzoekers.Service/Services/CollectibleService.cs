using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Speurzoekers.Common.Business;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Service.Hubs;
using Speurzoekers.Service.Hubs.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Speurzoekers.Service.Services
{
#warning Replace with Azure Task or something
    public class CollectibleService : IHostedService, IDisposable
    {
        private Timer _timer;
        private int? oldStartTime, currentStartTime;
        private int counter;

        private readonly IHubContext<CollectibleHub, ICollectibleClient> _collectibleHub;
        private readonly IServiceScope _serviceScope;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMapGenerator _mapGenerator;

        public CollectibleService(
            IHubContext<CollectibleHub, ICollectibleClient> collectibleHub,
            IServiceScopeFactory scopeFactory)
        {
            _collectibleHub = collectibleHub;
            _serviceScope = scopeFactory.CreateScope();

            _mapGenerator = _serviceScope.ServiceProvider.GetRequiredService<IMapGenerator>();
            _configurationRepository = _serviceScope.ServiceProvider.GetRequiredService<IConfigurationRepository>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            counter = await _configurationRepository.GetMapRefreshRate();
            _timer = new Timer(Count, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private async void Count(object state)
        {
            int? start = await _configurationRepository.GetMapRefreshStart();

            if (start != oldStartTime)
            {
                currentStartTime = oldStartTime = start;
            }

            if (currentStartTime.HasValue)
            {
                if (DateTime.Now.Hour * 60 + DateTime.Now.Minute != currentStartTime)
                {
                    return;
                }
                else
                {
                    currentStartTime = null;
                }
            }

            int min = await _configurationRepository.GetMapRefreshRate();
            counter++;

            if (counter >= min)
            {
                counter = 0;

                try
                {
                    await _mapGenerator.RefreshMap(DateTime.Now);
                    await _collectibleHub.Clients.All.RefreshMapAlert();
                }
                catch (SynchronizationLockException) // already refreshing
                { }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _serviceScope.Dispose();
        }
    }

}
