using LazyCache;
using Sokoban.Engine.Models;
using Sokoban.Service.Models;

namespace Sokoban.Service.Repositories
{
    internal class LevelDataJsonRepository : ILevelDataRepository
    {
        private readonly IAppCache cache;
        private readonly IJsonFileRepository<LevelData> baseRepository;

        public LevelDataJsonRepository(IAppCache cache, IJsonFileRepository<LevelData> baseRepository)
        {
            this.cache = cache;
            this.baseRepository = baseRepository;
        }

        public async Task<string> GetBiomeIdAsync(string levelId)
        {
            var levelData = await GetAsync(levelId);
            return levelData.Biome;
        }

        public async Task<GameState> GetInitialGameStateAsync(string levelId)
        {
            var levelData = await GetAsync(levelId);
            return levelData.InitialGameState;
        }

        public async IAsyncEnumerable<LevelIdStarTresholdsPair> GetLevelIdsAndStarTresholdsAsync()
        {
            var levelIds = baseRepository.GetItemIds();
            foreach (var levelId in levelIds)
            {
                var tresholds = await GetStarTresholdsAsync(levelId);
                yield return new(levelId, tresholds);
            }
        }

        public async Task<StarTresholdLookup> GetStarTresholdsAsync(string levelId)
        {
            var levelData = await GetAsync(levelId);
            return levelData.StarTresholds;
        }

        private Task<LevelData> GetAsync(string levelId)
        {
            return cache.GetOrAddAsync(
                $"{typeof(LevelData).FullName} - {levelId}",
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.MaxValue;
                    return baseRepository.GetAsync(levelId);
                });
        }

        internal sealed record LevelData(string Biome, StarTresholdLookup StarTresholds, GameState InitialGameState);
    }
}
