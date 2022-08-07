using Sokoban.Service.Models;

namespace Sokoban.Service
{
    public interface IGameEventHandler
    {
        void OnTrashBagMovement();
        void OnTeleport();
        void OnMovementBlockedByWall();
        void OnTrashEnteringTrashCan();
        void OnTrashLeavingTrashCan();
        void OnCompletion();
    }
}
