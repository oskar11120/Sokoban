using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sokoban.Monogame.Android
{
    internal class Menu
    {
        private readonly SpriteFont font;
        private readonly Rectangle screenRectangle;
        private readonly int fontScale;
        private readonly Vector2 screenCenter;
        private readonly IReadOnlyCollection<MenuItem> items;

        public Menu(SpriteFont font, Rectangle screenRectangle)
        {
            this.font = font;
            this.screenRectangle = screenRectangle;
            fontScale = screenRectangle.Width / 150;
            screenCenter = screenRectangle.Center.ToVector2();

            var itemHeight = font.MeasureString("Aa").Y * fontScale;
            var verticalDistanceBetweenItems = itemHeight * 1.3f;
            var levelSelect = CreateMenuItem("Level Select", 0, () => { });
            var @continue = CreateMenuItem("Continue", -verticalDistanceBetweenItems, () => { });
            var exit = CreateMenuItem("Exit", verticalDistanceBetweenItems, () => { });
            items = new[] { levelSelect, @continue, exit };
        }

        public void Update()
        {
            foreach (var item in items)
            {
                item.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in items)
            {
                item.Draw(spriteBatch);
            };
        }

        private MenuItem CreateMenuItem(string text, float heightOffset, Action onSelect)
        {
            var position = screenCenter + new Vector2(0, heightOffset);
            return new MenuItem(font, fontScale, text, position, onSelect);
        }
    }
}
