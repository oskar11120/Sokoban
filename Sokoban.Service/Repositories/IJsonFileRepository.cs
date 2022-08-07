namespace Sokoban.Service.Repositories
{
    internal interface IJsonFileRepository<TItem>
    {
        IEnumerable<string> GetItemIds();
        Task<TItem> GetAsync(string itemId);
        Task UpdateAsync(string itemId, TItem updatedItem);
    }
}