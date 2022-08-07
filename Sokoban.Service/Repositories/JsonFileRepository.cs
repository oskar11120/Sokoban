using Newtonsoft.Json;
using System.Collections.Concurrent;

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
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(ReadonlySetJsonConverter.Singleton);
            return JsonConvert.DeserializeObject<TItem>(fileContent, settings)!;
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

        private class ReadonlySetJsonConverter : JsonConverter
        {
            public static readonly ReadonlySetJsonConverter Singleton = new();

            private ReadonlySetJsonConverter()
            {

            }

            private readonly ConcurrentDictionary<Type, JsonConverter> converters = new();

            public override bool CanConvert(Type objectType)
            {
                return objectType.GetGenericTypeDefinition() == typeof(IReadOnlySet<>);
            }

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                return converters.GetOrAdd(
                    objectType,
                    genericArgumentType =>
                    {
                        var type = typeof(ReadonlySetJsonConverter<>)
                            .MakeGenericType(new[] { objectType });
                        return (JsonConverter)Activator.CreateInstance(type)!;
                    });
            }

            public override bool CanWrite => false;

            public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

        private class ReadonlySetJsonConverter<T> : JsonConverter<IReadOnlySet<T>>
        {
            public override IReadOnlySet<T>? ReadJson(JsonReader reader, Type objectType, IReadOnlySet<T>? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return serializer
                    .Deserialize<IEnumerable<T>>(reader)!
                    .ToHashSet();
            }

            public override bool CanWrite => false;

            public override void WriteJson(JsonWriter writer, IReadOnlySet<T>? value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
