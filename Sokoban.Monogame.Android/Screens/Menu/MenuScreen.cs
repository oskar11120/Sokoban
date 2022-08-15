using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Sokoban.Monogame.Android.Screens.Menu
{
    internal class MenuScreen
    {
        private readonly SpriteFont font;
        private readonly float fontScale;
        private readonly Vector2 screenCenter;
        private readonly IReadOnlyCollection<MenuItem> items;

        public MenuScreen(SpriteFont font, float fontScale, Rectangle screenRectangle, Action exit, GameState gameState)
        {
            this.font = font;
            this.fontScale = fontScale;
            screenCenter = screenRectangle.Center.ToVector2();

            var itemHeight = font.MeasureString("Aa").Y * fontScale;
            var verticalDistanceBetweenItems = itemHeight * 1.3f;
            var continueItem = CreateMenuItem("Continue", -verticalDistanceBetweenItems, gameState.OpenCurrentGameScreen);
            var levelSelectItem = CreateMenuItem("Level Select", 0, gameState.OpenLevelSelectionScreen);
            var exitItem = CreateMenuItem("Exit", verticalDistanceBetweenItems, exit);
            items = new[] { continueItem, levelSelectItem, exitItem };
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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in items)
            {
                item.Draw(spriteBatch);
            }
        }

        private MenuItem CreateMenuItem(string text, float heightOffset, Action onSelect)
        {
            var position = screenCenter + new Vector2(0, heightOffset);
            return new MenuItem(font, fontScale, text, position, onSelect);
        }
    }
}
