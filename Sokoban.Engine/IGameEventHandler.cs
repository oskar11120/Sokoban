namespace Sokoban.Engine
{
    public interface IGameEventHandler
    {
        void OnTrashBagMovement();
        void OnTeleport();
        void OnMovementBlockedByWall();
        void OnTrashEnteringTrashCan();
        void OnTrashLeavingTrashCan();
        Task OnCompletion(int moveCount);
    }
}
