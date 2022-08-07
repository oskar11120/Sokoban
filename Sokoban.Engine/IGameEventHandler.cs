namespace Sokoban.Engine
{
    public interface IGameEventHandler
    {
        void OnTrashbagMovement();
        void OnTeleport();
        void OnMovementBlockedByWall();
        void OnTrashEnteringTrashCan();
        void OnTrashLeavingTrashCan();
    }
}
