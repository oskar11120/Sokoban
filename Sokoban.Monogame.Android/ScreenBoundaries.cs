using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban.Monogame.Android
{
    internal class ScreenBoundaries
    {
        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private Texture2D texture;
        public ScreenBoundaries(GraphicsDeviceManager graphicsDeviceManager)
        {
            this.graphicsDeviceManager = graphicsDeviceManager;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture is null)
            {
                texture = new(graphicsDeviceManager.GraphicsDevice, 1, 1);
                texture.SetData(new[] { Color.White });
            }

            var lineWidth = 7;
            var rect = graphicsDeviceManager.GraphicsDevice.PresentationParameters.Bounds;
            spriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Top, rect.Width, lineWidth), Color.Black);
            spriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Top, lineWidth, rect.Height), Color.Black);
            spriteBatch.Draw(texture, new Rectangle(rect.Right - lineWidth, rect.Top, lineWidth, rect.Height), Color.Black);
            spriteBatch.Draw(texture, new Rectangle(rect.Left, rect.Bottom - lineWidth, rect.Width, lineWidth), Color.Black);
        }
    }
}
