using Sokoban.Engine.Models;
using System.Numerics;

namespace Sokoban.Engine
{
    public class Game
    {
        private readonly GameStateHistory states;
        private readonly IGameEventHandler gameEventHandler;

        public Game(GameState state, IGameEventHandler gameEventHandler)
        {
            states = new(state);
            this.gameEventHandler = gameEventHandler;
        }

        public GameState CurrentState => states.Current;
        public int MoveCount => states.Count - 1;

        public void TryMoveSnail(Vector2 targetPosition)
        {
            if (IsNextToSnail(targetPosition) || CurrentState.AllTrashBagsAreInTrashCans)
            {
                return;
            }
            if (IsAnythingBlockingSnailInPosition(targetPosition))
            {
                gameEventHandler.OnMovementBlockedByWall();
                return;
            }

            var movement = targetPosition - CurrentState.SnailPosition;
            var newState = TryMoveEntity(targetPosition, targetPosition + movement, CurrentState);
            newState = TryMoveEntity(CurrentState.SnailPosition, targetPosition, newState);
            ApplyEventHandlers(newState);
            states.Add(newState);
        }

        public bool CanUndo() => states.CanUndo();
        public void Undo() => states.Undo();
        public bool CanRedo() => states.CanRedo();
        public void Redo() => states.Redo();
        public void Restart() => states.Clear();

        private void ApplyEventHandlers(GameState newState)
        {
            if (newState.TrashBagPositions != CurrentState.TrashCanPositions)
                gameEventHandler.OnTrashbagMovement();

            if (DidAnythingTeleport(newState))
                gameEventHandler.OnTeleport();

            var newTrashBagInTrashCanCount = GetNumberOfTrashBagsInTrashCans(newState);
            var currentTrashBagInTrashCanCount = GetNumberOfTrashBagsInTrashCans(CurrentState);
            if (newTrashBagInTrashCanCount > currentTrashBagInTrashCanCount)
                gameEventHandler.OnTrashEnteringTrashCan();
            if (newTrashBagInTrashCanCount < currentTrashBagInTrashCanCount)
                gameEventHandler.OnTrashLeavingTrashCan();

            if (newState.AllTrashBagsAreInTrashCans)
                gameEventHandler.OnCompletion();
        }

        private bool DidAnythingTeleport(GameState newState)
        {
            return newState.TryGetOtherTeleportPosition(newState.SnailPosition, out _) ||
                GetNumberOfTrashBagsOnTeleports(newState) > GetNumberOfTrashBagsOnTeleports(CurrentState);
        }

        private static int GetNumberOfTrashBagsOnTeleports(GameState gameState)
        {
            return gameState
                .TrashBagPositions
                .Count(position => gameState.TryGetOtherTeleportPosition(position, out _));
        }

        private static int GetNumberOfTrashBagsInTrashCans(GameState gameState)
        {
            return gameState
                .TrashBagPositions
                .Count(position => gameState.TrashCanPositions.Contains(position));
        }

        private GameState TryMoveEntity(Vector2 moveableEntityPosition, Vector2 targetPosition, GameState gameState)
        {
            var movement = targetPosition - moveableEntityPosition;
            var targetPositionIsTeleportPosition = CurrentState.TryGetOtherTeleportPosition(targetPosition, out var otherTeleportPosition);
            return targetPositionIsTeleportPosition ?
                gameState
                    .TryUpdatePosition(otherTeleportPosition, otherTeleportPosition + movement)
                    .TryUpdatePosition(targetPosition, otherTeleportPosition) :
                gameState
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