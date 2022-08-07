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

        public void TryMoveSnail(Vector2 targetPosition)
        {
            var canMove = IsNextToSnail(targetPosition) &&
                !IsAnythingBlockingSnailInPosition(targetPosition);
            if (!canMove)
            {
                return;
            }

            var movement = targetPosition - CurrentState.SnailPosition;
            var newState = TryMoveEntity(targetPosition, targetPosition + movement, CurrentState);
            newState = TryMoveEntity(CurrentState.SnailPosition, targetPosition, newState);
            states.Add(newState);
        }

        public bool CanUndo() => states.CanUndo();
        public void Undo() => states.Undo();
        public bool CanRedo() => states.CanRedo();
        public void Redo() => states.Redo();
        public void Restart() => states.Clear();

        private GameState TryMoveEntity(Vector2 moveableEntityPosition, Vector2 targetPosition, GameState gameState)
        {
            var movement = targetPosition - moveableEntityPosition;
            var targetPositionIsTeleportPosition = CurrentState.TryGetOtherTeleportPosition(targetPosition, out var otherTeleportPosition);
            return targetPositionIsTeleportPosition ?
                CurrentState
                    .TryUpdatePosition(otherTeleportPosition, otherTeleportPosition + movement)
                    .TryUpdatePosition(targetPosition, otherTeleportPosition) :
                CurrentState
                    .TryUpdatePosition(moveableEntityPosition, targetPosition);              
        }

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