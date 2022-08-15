using LazyCache;
using Microsoft.Extensions.DependencyInjection;
using Sokoban.Service.Repositories;
using static Sokoban.Service.Repositories.LevelDataJsonRepository;
using static Sokoban.Service.Repositories.PlayerDataJsonRepository;
using IBaseGameEventHandler = Sokoban.Engine.IGameEventHandler;

namespace Sokoban.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSokobanGameService(
            this IServiceCollection services,
            string levelDataDirectory,
            string playerDataDirectory,
            Func<IServiceProvider, IGameEventHandler> gameEventHandlerFactory)
        {
            return services
                .AddSingleton<IJsonFileRepository<LevelData>>(new JsonFileRepository<LevelData>(levelDataDirectory))
                .AddSingleton<IJsonFileRepository<PlayerData>>(new JsonFileRepository<PlayerData>(playerDataDirectory))
                .AddSingleton<ILevelDataRepository, LevelDataJsonRepository>()
                .AddSingleton<IPlayerDataRepository, PlayerDataJsonRepository>()
                .AddSingleton<IBaseGameEventHandler, BaseGameEventHandler>()
                .AddSingleton(gameEventHandlerFactory)
                .AddSingleton<IAppCache>(new CachingService())
                .AddSingleton<IGameService, GameService>();
        }
    }
}
