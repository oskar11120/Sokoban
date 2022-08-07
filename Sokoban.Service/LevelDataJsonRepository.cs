using Sokoban.Engine.Models;
using Sokoban.Service.Models;

namespace Sokoban.Service
{
    internal class LevelDataJsonRepository : ILevelDataRepository
    {
        private readonly string rootDirectory;

        public LevelDataJsonRepository(string rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public Task<string> GetBiomeIdAsync(string levelId)
        {
            throw new NotImplementedException();
        }

        public Task<GameState> GetInitialGameStateAsync(string levelId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LevelIdStarTresholdsPair>> GetLevelIdsAndStarTresholdsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNextLevelIdAsync(string? previousLevelId)
        {
            throw new NotImplementedException();
        }

        public Task<StarTresholdLookup> GetStarTresholdsAsync(string levelId)
        {
            throw new NotImplementedException();
        }
    }
}
