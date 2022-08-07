using Sokoban.Engine;
using Sokoban.Service.Models;

namespace Sokoban.Service
{
    public interface IGameService
    {
        Task<string> GetBiomeIdAsync(string levelId);
        IAsyncEnumerable<LevelInfo> GetLevelsAsync();
        Task<Game> StartNewAsync(string levelId);
    }
}