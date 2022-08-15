using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Sokoban.Service;

namespace Sokoban.Monogame.Android.Screens.LevelSelect
{
    internal class LevelSelectScreen
    {
        private readonly IReadOnlyCollection<LevelSelectItem> items;

        public LevelSelectScreen(Rectangle screenRectangle, SpriteFont font, float fontScale, IGameService gameService, GameState gameState)
        {
            var itemHeight = LevelSelectItem.GetHeight(font, fontScale);
            items = gameService
                .GetLevelsAsync()
                .ToEnumerable()
                .Select((level, i) => new LevelSelectItem(
                    new(screenRectangle.Center.X, i * itemHeight), 
                    font, 
                    fontScale, 
                    level,
                    () => 
                    {
                        var newGame = gameService.StartNewAsync(level.LevelId).GetAwaiter().GetResult();
                        gameState.ReplaceSelectedGame(newGame);
                    }))
                .ToArray();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in items)
            {
                item.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            var pressedTouchLocations = TouchPanel
                .GetState()
                .Where(location => location.State == TouchLocationState.Pressed);

            foreach (var item in items)
            {
                item.Update(pressedTouchLocations);
            }
        }
    }
}
