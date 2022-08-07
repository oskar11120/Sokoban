using Newtonsoft.Json;

namespace Sokoban.Service.Repositories
{
    internal class JsonFileRepository<TItem> : IJsonFileRepository<TItem>
    {
        private readonly string rootDirectory;

        public JsonFileRepository(string rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public async Task<TItem> GetAsync(string itemId)
        {
            var fileContent = await File.ReadAllTextAsync(GetItemPath(itemId));
            return JsonConvert.DeserializeObject<TItem>(fileContent)!;
        }

        public Task UpdateAsync(string itemId, TItem updatedItem)
        {
            return File.WriteAllTextAsync(GetItemPath(itemId), JsonConvert.SerializeObject(updatedItem));
        }

        public IEnumerable<string> GetItemIds()
        {
            return Directory
                .EnumerateFiles(rootDirectory)
                .Select(path =>
                {
                    var span = path.AsSpan();
                    var lastSeparator = span.LastIndexOf(Path.PathSeparator);
                    return span
                        .Slice(lastSeparator + 1)
                        .TrimEnd(".json")
                        .ToString();
                });
        }

        private string GetItemPath(string itemId)
        {
            return Path.Combine(rootDirectory, $"{itemId}.json");
        }
    }
}
