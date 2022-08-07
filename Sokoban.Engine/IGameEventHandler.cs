namespace Sokoban.Engine
{
    public interface IGameEventHandler
    {
        void OnSnailMovement();
        void OnTrashbagMovement();
        void OnTeleport();
        void OnWallHit();
        void OnTrashInTrashCan();
    }
}
