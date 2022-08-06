using System.Numerics;

namespace Sokoban.Engine.Models
{
    public record struct TeleportPositionPair(Vector2 First, Vector2 Second) 
    {
        public IEnumerable<Vector2> AsEnumerable()
        {
            yield return First;
            yield return Second;
        }
    }
}
