using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Extensions.DependencyInjection;
using Sokoban.Service;

namespace Sokoban.Monogame.Android
{
    public class SokobanGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly IEnumerable<IRequiringLoadContent> servicesRequiringLoadContent;
        private readonly IGameService gameService;
        private Menu menu;

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

            menu = new(Content.Load<SpriteFont>("Fonts/Font"), GraphicsDevice.PresentationParameters.Bounds, Exit);

            foreach (var service in servicesRequiringLoadContent)
            {
                service.LoadContent(Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            menu.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            menu.Draw(spriteBatch);
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