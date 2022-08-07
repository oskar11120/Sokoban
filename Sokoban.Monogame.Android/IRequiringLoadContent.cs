using Microsoft.Xna.Framework.Content;

namespace Sokoban.Monogame.Android
{
    internal interface IRequiringLoadContent
    {
        void LoadContent(ContentManager contentManager);
    }
}