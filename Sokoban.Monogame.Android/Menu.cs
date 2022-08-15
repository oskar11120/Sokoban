using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Sokoban.Monogame.Android
{
    internal class Menu
    {
        private readonly SpriteFont font;
        private readonly int fontScale;
        private readonly Vector2 screenCenter;
        private readonly IReadOnlyCollection<MenuItem> items;

        public Menu(SpriteFont font, Rectangle screenRectangle, Action exit)
        {
            this.font = font;
            fontScale = screenRectangle.Width / 150 / 3;
            screenCenter = screenRectangle.Center.ToVector2();

            var itemHeight = font.MeasureString("Aa").Y * fontScale;
            var verticalDistanceBetweenItems = itemHeight * 1.3f;
            var continueItem = CreateMenuItem("Continue", -verticalDistanceBetweenItems, () => { });
            var levelSelectItem = CreateMenuItem("Level Select", 0, () => { });
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
