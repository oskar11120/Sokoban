using IBaseGameEventHandler = Sokoban.Engine.IGameEventHandler;

namespace Sokoban.Service
{
    internal class BaseGameEventHandler : IBaseGameEventHandler
    {
        private readonly IGameEventHandler wrappingHandler;
        private readonly Func<int, Action, Task> onCompletion;

        public BaseGameEventHandler(IGameEventHandler wrappingHandler, Func<int, Action, Task> onCompletion)
        {
            this.wrappingHandler = wrappingHandler;
            this.onCompletion = onCompletion;
        }

        public Task OnCompletion(int moveCount) => onCompletion(moveCount, wrappingHandler.OnCompletion);
        public void OnMovementBlockedByWall() => wrappingHandler.OnMovementBlockedByWall();
        public void OnTeleport() => wrappingHandler.OnTeleport();
        public void OnTrashBagMovement() => wrappingHandler.OnTrashBagMovement();
        public void OnTrashEnteringTrashCan() => wrappingHandler.OnTrashEnteringTrashCan();
        public void OnTrashLeavingTrashCan() => wrappingHandler.OnTrashLeavingTrashCan();
    }
}
