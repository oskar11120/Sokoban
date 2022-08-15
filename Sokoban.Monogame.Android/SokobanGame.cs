using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Extensions.DependencyInjection;
using Sokoban.Service;
using Sokoban.Monogame.Android.Screens.Menu;
using Sokoban.Monogame.Android.Screens.LevelSelect;

namespace Sokoban.Monogame.Android
{
    public class SokobanGame : Game
    {
        private readonly IEnumerable<IRequiringLoadContent> servicesRequiringLoadContent;
        private readonly IGameService gameService;
        private readonly GameState gameState = new();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private MenuScreen menuScreen;
        private LevelSelectScreen levelSelectScreen;

        public SokobanGame()
        {
            graphics = new(this);
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            servicesRequiringLoadContent = ServiceProvider.Services.GetServices<IRequiringLoadContent>();
            gameService = ServiceProvider.Services.GetRequiredService<IGameService>();
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();

            var screenRectangle = GraphicsDevice.PresentationParameters.Bounds;
            var font = Content.Load<SpriteFont>("Fonts/Font");
            var fontScale = screenRectangle.Width / 150 / 3;
            menuScreen = new(font, fontScale, screenRectangle, Exit, gameState);
            levelSelectScreen = new(screenRectangle, font, fontScale, gameService, gameState);

            foreach (var service in servicesRequiringLoadContent)
            {
                service.LoadContent(Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            gameState.Switch(
                menuScreen.Update,
                levelSelectScreen.Update,
                _ => { });

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            gameState.Switch(
                () => menuScreen.Draw(spriteBatch),
                () => levelSelectScreen.Draw(spriteBatch),
                _ => { });

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            ServiceProvider.Dispose();
            base.Dispose(disposing);
        }

    }
}