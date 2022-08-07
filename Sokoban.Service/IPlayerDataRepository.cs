using Sokoban.Service.Models;

namespace Sokoban.Service
{
    internal interface IPlayerDataRepository
    {
        public Task<IEnumerable<LevelIdMoveCountPair>> GetCompletedLevelMoveCountsAsync();
        public Task<int?> GetBestLevelMoveCountAsync(string levelId);
        public Task UpdateBestLevelMoveCountAsync(string levelId);
    }
}
