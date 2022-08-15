using Microsoft.Extensions.DependencyInjection;
using Sokoban.Service;
using BaseServiceProvider = Microsoft.Extensions.DependencyInjection.ServiceProvider;

namespace Sokoban.Monogame.Android
{
    internal class ServiceProvider
    {
        private static readonly BaseServiceProvider Singleton = Create();
        public static readonly IServiceProvider Services = Singleton;

        private static BaseServiceProvider Create()
        {
            return new ServiceCollection()
                .AddSingleton<SoundPlayer>()
                .AddSingleton<IRequiringLoadContent>(serviceProvider => serviceProvider.GetRequiredService<SoundPlayer>())
                .AddSingleton(serviceProvider => serviceProvider.GetServices<IRequiringLoadContent>().ToArray())
                .AddSokobanGameService(Settings.LevelDataDirectory, Settings.PlayerDataDirectory, serviceProvider => serviceProvider.GetRequiredService<SoundPlayer>())
                .BuildServiceProvider();
        }

        public static void Dispose()
        {
            Singleton.Dispose();
        }

        private static class Settings
        {
            private static readonly string rootDirectory = "Resources";

            public static readonly string LevelDataDirectory = Path.Combine(rootDirectory, "LevelData");
            public static readonly string PlayerDataDirectory = rootDirectory;
        }
    }
}
