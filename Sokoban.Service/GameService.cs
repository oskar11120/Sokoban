using MoreLinq;
using Sokoban.Engine;
using Sokoban.Service.Models;
using Sokoban.Service.Repositories;

namespace Sokoban.Service
{
    internal class GameService : IGameService
    {
        private readonly ILevelDataRepository levelDataRepository;
        private readonly IPlayerDataRepository playerDataRepository;
        private readonly IGameEventHandler gameEventHandler;

        public GameService(
            ILevelDataRepository levelDataRepository,
            IPlayerDataRepository playerDataRepository,
            IGameEventHandler gameEventHandler)
        {
            this.levelDataRepository = levelDataRepository;
            this.playerDataRepository = playerDataRepository;
            this.gameEventHandler = gameEventHandler;
        }

        public async Task<Game> StartNewAsync(string levelId)
        {
            var initialState = await levelDataRepository.GetInitialGameStateAsync(levelId);
            return new Game(
                initialState,
                new BaseGameEventHandler(gameEventHandler, async (moveCount, onCompletion) =>
                {
                    onCompletion();
                    var previousMoveCount = await playerDataRepository.GetBestLevelMoveCountAsync(levelId);
                    if (previousMoveCount is null || previousMoveCount < moveCount)
                        await playerDataRepository.UpdateBestLevelMoveCountAsync(levelId, moveCount);
                }));
        }

        public Task<string> GetBiomeIdAsync(string levelId)
        {
            return levelDataRepository.GetBiomeIdAsync(levelId);
        }

        public async IAsyncEnumerable<LevelInfo> GetLevelsAsync()
        {
            var moveCountsByLevelId = await playerDataRepository.GetBestLevelMoveCountsByLevelIdAsync();
            var levelIdsAndStarTresholds = levelDataRepository.GetLevelIdsAndStarTresholdsAsync();
            await foreach (var (levelId, starTreshold) in levelIdsAndStarTresholds)
            {
                int? moveCount = moveCountsByLevelId.TryGetValue(levelId, out var count) ? count : null;
                yield return new LevelInfo(levelId, starTreshold, moveCount);
            }
        }
    }
}
