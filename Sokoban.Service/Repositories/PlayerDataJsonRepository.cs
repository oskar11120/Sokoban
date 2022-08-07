using LazyCache;
using System.Collections.Immutable;

namespace Sokoban.Service.Repositories
{
    internal class PlayerDataJsonRepository : IPlayerDataRepository
    {
        private readonly IAppCache cache;
        private readonly IJsonFileRepository<PlayerData> baseRepository;

        public PlayerDataJsonRepository(IAppCache cache, IJsonFileRepository<PlayerData> baseRepository)
        {
            this.cache = cache;
            this.baseRepository = baseRepository;
        }

        public async Task<int?> GetBestLevelMoveCountAsync(string levelId)
        {
            var counts = await GetBestLevelMoveCountsByLevelIdAsync();
            return counts.TryGetValue(levelId, out var count) ? count : null;
        }

        public async Task<IReadOnlyDictionary<string, int>> GetBestLevelMoveCountsByLevelIdAsync()
        {
            var playerData = await GetAsync();
            return playerData.BestLevelMoveCounts;
        }

        public async Task UpdateBestLevelMoveCountAsync(string levelId, int count)
        {
            var playerData = await GetAsync();
            var updatedCounts = playerData.BestLevelMoveCounts.SetItem(levelId, count);
            await WriteAsync(playerData with { BestLevelMoveCounts = updatedCounts });
        }

        private Task<PlayerData> GetAsync()
        {
            const string id = nameof(PlayerData);
            return cache.GetOrAddAsync(
                id,
                cacheEntry =>
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.MaxValue;
                    return baseRepository.GetAsync(id);
                });
        }

        private Task WriteAsync(PlayerData playerData)
        {
            const string id = nameof(PlayerData);
            cache.Remove(id);
            cache.Add(id, playerData, TimeSpan.MaxValue);
            return baseRepository.UpdateAsync(id, playerData);
        }

        internal sealed record PlayerData(ImmutableDictionary<string, int> BestLevelMoveCounts);
    }
}
