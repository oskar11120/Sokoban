using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Sokoban.Monogame.Android.Extensions;
using Sokoban.Service.Models;

namespace Sokoban.Monogame.Android.Screens.LevelSelect
{
    internal class LevelSelectItem
    {
        private readonly Vector2 position;
        private readonly SpriteFont font;
        private readonly float fontScale;
        private readonly Action onSelect;
        private readonly Vector2 fontSize;
        private readonly string text;
        private readonly Rectangle touchRectangle;

        public LevelSelectItem(Vector2 position, SpriteFont font, float fontScale, LevelInfo level, Action onSelect)
        {
            this.position = position;
            this.font = font;
            this.fontScale = fontScale;
            this.onSelect = onSelect;
            fontSize = font.MeasureString(text);
            touchRectangle = font.GetWrappingRectangle(text, fontScale, position);

            var (levelId, starTreshold, bestMoveCount) = level;
            text = $"{levelId}\n{starTreshold}/3 Acquired Stars\nBest: {bestMoveCount?.ToString() ?? "None"}";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, Color.White, 0, fontSize / 2, fontScale, SpriteEffects.None, 0);
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

        public static float GetHeight(SpriteFont font, float fontScale)
        {
            return font.GetWrappingRectangle("Aa\nAa\nAa", fontScale, default).Height;
        }
    }
}
