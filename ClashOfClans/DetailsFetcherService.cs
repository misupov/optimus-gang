using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClashOfClans.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClashOfClans
{
    public class DetailsFetcherService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;
        private Timer _timer;

        public DetailsFetcherService(IServiceScopeFactory scopeFactory, ILogger<DetailsFetcherService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DetailsFetcherService is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ClashContext>();
                var tag = "#Q8GRU2LR";
                var clanSnapshot = await ApiClient.GetClanSnapshot(tag);
                var existingClanSnapshot = await dbContext.ClanSnapshots.Where(s => s.Tag == tag).OrderByDescending(s => s.SnapshotTime).Include(s => s.MemberList).FirstOrDefaultAsync();
                if (existingClanSnapshot == null)
                {
                    await dbContext.ClanSnapshots.AddAsync(clanSnapshot);
                    await dbContext.SaveChangesAsync();
                }
                else if (!ClanSnapshot.ClanSnapshotComparer.Equals(clanSnapshot, existingClanSnapshot))
                {
                    var playersDict = existingClanSnapshot.MemberList.ToDictionary(s => s.Tag);
                    for (var index = 0; index < clanSnapshot.MemberList.Count; index++)
                    {
                        var playerSnapshot = clanSnapshot.MemberList[index];
                        if (playersDict.TryGetValue(playerSnapshot.Tag, out var existingPlayerSnapshot))
                        {
                            if (PlayerSnapshot.PlayerSnapshotComparer.Equals(playerSnapshot, existingPlayerSnapshot))
                            {
                                clanSnapshot.MemberList[index] = existingPlayerSnapshot;
                            }
                        }
                    }

                    await dbContext.ClanSnapshots.AddAsync(clanSnapshot);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DetailsFetcherService is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}