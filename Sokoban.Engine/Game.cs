using Sokoban.Engine.Models;
using System.Numerics;

namespace Sokoban.Engine
{
    public class Game
    {
        public Game(GameState state)
        {
            states = new(state);
        }

        private readonly GameStateHistory states;

        public GameState CurrentState => states.Current;

        public bool TryMoveSnail(Vector2 targetPosition)
        {
            var canMove = IsNextToSnail(targetPosition) &&
                !IsAnythingBlockingSnailInPosition(targetPosition);
            if (!canMove)
            {
                return false;
            }

            if (CurrentState.TryGetOtherTeleportPosition(targetPosition, out var otherTeleportPosition))
            {
                states.Add(CurrentState with { SnailPosition = otherTeleportPosition });
            }
            else if (CurrentState.TrashBagPositions.Contains(targetPosition))
            {
                var newTrashBagPosition = targetPosition + targetPosition - CurrentState.SnailPosition;
                var willTrashBagBeTeleported = CurrentState.TryGetOtherTeleportPosition(newTrashBagPosition, out var teleportPosition);
                newTrashBagPosition = willTrashBagBeTeleported ? teleportPosition : newTrashBagPosition;
                var newState = CurrentState with
                {
                    SnailPosition = targetPosition,
                    TrashBagPositions = CurrentState.TrashBagPositions
                        .Remove(targetPosition)
                        .Add(newTrashBagPosition)

                };
                states.Add(newState);
            }

            return true;
        }

        public bool CanUndo() => states.CanUndo();
        public void Undo() => states.Undo();
        public bool CanRedo() => states.CanRedo();
        public void Redo() => states.Redo();
        public void Restart() => states.Clear();

        private bool IsNextToSnail(Vector2 position)
        {
            var difference = CurrentState.SnailPosition - position;
            return difference.X == 0 && difference.Y == 1 ||
                difference.X == 1 && difference.Y == 0;
        }

        private bool IsAnythingBlockingSnailInPosition(Vector2 position)
        {
            var blockedByWall = CurrentState.WallPositions.Contains(position);
            if (blockedByWall)
            {
                return true;
            }

            var difference = CurrentState.SnailPosition - position;
            var positionAfter = position + difference;
            return CurrentState.TrashBagPositions.Contains(position) &&
                (CurrentState.TrashBagPositions.Contains(positionAfter) ||
                CurrentState.WallPositions.Contains(positionAfter));
        }
    }
}