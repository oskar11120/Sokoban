using Sokoban.Engine.Models;
using Sokoban.Service.Models;

namespace Sokoban.Service.Repositories
{
    internal interface ILevelDataRepository
    {
        Task<GameState> GetInitialGameStateAsync(string levelId);
        Task<StarTresholdLookup> GetStarTresholdsAsync(string levelId);
        Task<string> GetBiomeIdAsync(string levelId);
        IAsyncEnumerable<LevelIdStarTresholdsPair> GetLevelIdsAndStarTresholdsAsync();
    }
}
