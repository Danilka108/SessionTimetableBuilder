using System.Text.Json;
using System.Text.Json.Serialization;

namespace Storage;

internal class SerializableStorageSet
{
    private readonly IEnumerable<Identified<SerializableEntity>> _entities;
    private readonly int _lastId;

    private SerializableStorageSet(int lastId, IEnumerable<Identified<SerializableEntity>> entities)
    {
        _entities = entities;
        _lastId = lastId;
    }

    internal StorageSet<TEntity> ToTypedStorageSet<TEntity>(StorageTransaction? transaction = null)
    {
        var typedEntities = _entities
            .Select(entity => new Identified<TEntity>(
                entity.Id,
                entity.Entity.ToTypedEntity<TEntity>())
            )
            .ToList();

        return new StorageSet<TEntity>(_lastId, typedEntities, transaction);
    }

    internal static SerializableStorageSet FromTypedStorageSet<TEntity>(StorageSet<TEntity> storageSet)
    {
        var serializableEntities = storageSet
            .Select(entity =>
                new Identified<SerializableEntity>(
                    entity.Id,
                    SerializableEntity.FromTypedEntity(entity.Entity)
                )
            );

        return new SerializableStorageSet(storageSet.LastId, serializableEntities);
    }

    internal static SerializableStorageSet CreateEmpty()
    {
        return new SerializableStorageSet(0, new List<Identified<SerializableEntity>>());
    }

    internal class ConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(SerializableStorageSet);
        }

        public override JsonConverter CreateConverter(Type _, JsonSerializerOptions options)
        {
            return new Converter(options);
        }
    }

    private class Converter : JsonConverter<SerializableStorageSet>
    {
        private readonly JsonConverter<IEnumerable<Identified<SerializableEntity>>> _entitiesConverter;
        private readonly Type _entitiesType;

        private readonly JsonConverter<int> _lastIdConverter;
        private readonly Type _lastIdType;

        public Converter(JsonSerializerOptions options)
        {
            _lastIdType = typeof(int);
            _lastIdConverter = (JsonConverter<int>)options.GetConverter(_lastIdType);

            _entitiesType = typeof(IEnumerable<Identified<SerializableEntity>>);
            _entitiesConverter =
                (JsonConverter<IEnumerable<Identified<SerializableEntity>>>)options.GetConverter(_entitiesType);
        }

        public override SerializableStorageSet Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();

            var lastId = ReadLastId(ref reader, options);
            var entities = ReadEntities(ref reader, options);

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject) throw new JsonException();

            return new SerializableStorageSet(lastId, entities);
        }

        private int ReadLastId(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
            if (reader.GetString() != nameof(_lastId)) throw new JsonException();

            reader.Read();
            return _lastIdConverter.Read(ref reader, _lastIdType, options);
        }

        private IEnumerable<Identified<SerializableEntity>> ReadEntities(ref Utf8JsonReader reader,
            JsonSerializerOptions options)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
            if (reader.GetString() != nameof(_entities)) throw new JsonException();

            reader.Read();
            var entities = _entitiesConverter.Read(ref reader, _entitiesType, options);
            return entities ?? throw new NullReferenceException(nameof(entities));
        }

        public override void Write(Utf8JsonWriter writer, SerializableStorageSet value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(_lastId));
            _lastIdConverter.Write(writer, value._lastId, options);

            writer.WritePropertyName(nameof(_entities));
            _entitiesConverter.Write(writer, value._entities, options);

            writer.WriteEndObject();
        }
    }
}