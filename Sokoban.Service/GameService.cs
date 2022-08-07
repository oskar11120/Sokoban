using MoreLinq;
using Sokoban.Engine;
using Sokoban.Service.Models;

namespace Sokoban.Service
{
    internal class GameService
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
                    var previousMoveCount = await playerDataRepository.GetBestLevelMoveCountAsync(levelId);
                    await onCompletion(moveCount, previousMoveCount, await levelDataRepository.GetStarTresholdsAsync(levelId));
                    if (previousMoveCount is null || previousMoveCount < moveCount)
                        await playerDataRepository.UpdateBestLevelMoveCountAsync(levelId);
                }));
        }

        public Task<string> GetBiomeIdAsync(string levelId)
        {
            return levelDataRepository.GetBiomeIdAsync(levelId);
        }

        public async Task<IEnumerable<LevelInfo>> GetLevelsAsync()
        {
            var starTresholds = await levelDataRepository.GetLevelIdsAndStarTresholdsAsync();
            var moveCounts = await playerDataRepository.GetCompletedLevelMoveCountsAsync();
            return starTresholds.LeftJoin(
                moveCounts,
                pair => pair.LevelId,
                pair => pair.LevelId,
                pair => new LevelInfo(pair.LevelId, pair.StarTresholds, null),
                (one, other) => new LevelInfo(one.LevelId, one.StarTresholds, other.MoveCount));
        }
    }
}
