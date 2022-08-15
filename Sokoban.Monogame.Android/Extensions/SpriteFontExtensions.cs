using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Monogame.Android.Extensions
{
    internal static class SpriteFontExtensions
    {
        public static Rectangle GetWrappingRectangle(this SpriteFont font, string text, float scale, Vector2 centerRectanglePosition)
        {
            var sizeBeforeScale = font.MeasureString(text);
            var sizeAfterScale = sizeBeforeScale * scale;
            var rectanglePosition = centerRectanglePosition - sizeAfterScale / 2;
            return new(rectanglePosition.ToPoint(), sizeAfterScale.ToPoint());
        }
    }
}
