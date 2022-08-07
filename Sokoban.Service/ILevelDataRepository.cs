using Sokoban.Engine.Models;
using Sokoban.Service.Models;

namespace Sokoban.Service
{
    internal interface ILevelDataRepository
    {
        public Task<GameState> GetInitialGameStateAsync(string levelId);
        public Task<StarTresholdLookup> GetStarTresholdsAsync(string levelId);
        public Task<string> GetBiomeIdAsync(string levelId);
        public Task<string> GetNextLevelIdAsync(string? previousLevelId);
        public Task<IEnumerable<LevelIdStarTresholdsPair>> GetLevelIdsAndStartTresholdsAsync();
    }
}
