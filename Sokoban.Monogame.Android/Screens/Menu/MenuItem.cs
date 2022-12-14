using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Sokoban.Monogame.Android.Extensions;

namespace Sokoban.Monogame.Android.Screens.Menu
{
    internal class MenuItem
    {
        private readonly SpriteFont font;
        private readonly float fontScale;
        private readonly string text;
        private readonly Vector2 position;
        private readonly Vector2 fontSize;
        private readonly Rectangle touchRectangle;
        private readonly Action onSelect;

        public MenuItem(SpriteFont font, float fontScale, string text, Vector2 position, Action onSelect)
        {
            this.font = font;
            this.fontScale = fontScale;
            this.text = text;
            this.position = position;
            this.onSelect = onSelect;
            fontSize = font.MeasureString(text);
            touchRectangle = font.GetWrappingRectangle(text, fontScale, position);
        }

        public void Update(IEnumerable<TouchLocation> pressedTouchLocations)
        {
            foreach (var location in pressedTouchLocations)
            {
                if (touchRectangle.Contains(location.Position))
                {
                    onSelect();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, Color.White, 0, fontSize / 2, fontScale, SpriteEffects.None, 0);
        }
    }
}
