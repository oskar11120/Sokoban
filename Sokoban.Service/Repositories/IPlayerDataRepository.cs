namespace Sokoban.Service.Repositories
{
    internal interface IPlayerDataRepository
    {
        Task<IReadOnlyDictionary<string, int>> GetBestLevelMoveCountsByLevelIdAsync();
        Task<int?> GetBestLevelMoveCountAsync(string levelId);
        Task UpdateBestLevelMoveCountAsync(string levelId, int count);
    }
}
