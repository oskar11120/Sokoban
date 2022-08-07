using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Sokoban.Engine.Models
{
    public record GameState(
            Vector2 SnailPosition,
            IReadOnlySet<Vector2> WallPositions,
            ImmutableHashSet<Vector2> TrashBagPositions,
            IReadOnlySet<Vector2> TrashCanPositions,
            IReadOnlyCollection<TeleportPositionPair> TeleportPositions)
    {
        private readonly Lazy<IReadOnlyDictionary<Vector2, TeleportPositionPair>> teleportPositionPairs = new(
            () => TeleportPositions
                .SelectMany(pair => pair
                    .AsEnumerable()
                    .Select(position => KeyValuePair.Create(position, pair)))
                .ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value));

        private readonly Lazy<bool> allTrashBagsAreInTrashCans = new(
            () => TrashBagPositions.IsSubsetOf(TrashCanPositions));

        private IReadOnlyDictionary<Vector2, TeleportPositionPair> TeleportPositionPairs => teleportPositionPairs.Value;

        internal bool AllTrashBagsAreInTrashCans => allTrashBagsAreInTrashCans.Value;

        internal bool TryGetOtherTeleportPosition(Vector2 position, [NotNullWhen(true)] out Vector2 result)
        {
            if (TeleportPositionPairs.TryGetValue(position, out var pair))
            {
                result = pair
                    .AsEnumerable()
                    .First(other => other != position);
                return true;
            }

            result = default;
            return false;
        }

        internal GameState TryUpdatePosition(Vector2 currentPosition, Vector2 targetPosition)
        {
            if (SnailPosition == currentPosition)
            {
                return this with { SnailPosition = targetPosition };
            }
            if (TrashBagPositions.Contains(currentPosition))
            {
                return this with
                {
                    TrashBagPositions = TrashBagPositions
                        .Remove(currentPosition)
                        .Add(targetPosition)
                };
            }

            return this;
        }
    }
}
