using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Storage.Entity;

internal class SerializableEntity
{
    private readonly object _entity;
    private readonly Type _entityType;

    private SerializableEntity(Type entityType, object entity)
    {
        _entityType = entityType;
        _entity = entity;
    }

    private string GetEntityTypeName()
    {
        var type = _entityType.AssemblyQualifiedName;
        return type ?? throw new NullReferenceException(nameof(type));
    }

    public TEntity ToTypedEntity<TEntity>()
    {
        if (typeof(TEntity) == _entityType && _entity is TEntity entity) return entity;

        throw new InvalidCastException();
    }

    public static SerializableEntity FromTypedEntity<TEntity>(TEntity entity)
    {
        return new SerializableEntity(typeof(TEntity), entity);
    }

    internal class ConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(SerializableEntity);
        }

        public override JsonConverter CreateConverter(Type _, JsonSerializerOptions options)
        {
            return new Converter(options);
        }
    }

    private class Converter : JsonConverter<SerializableEntity>
    {
        private readonly JsonConverter<string> _typeNameConverter;
        private readonly Type _typeNameType;

        public Converter(JsonSerializerOptions options)
        {
            _typeNameType = typeof(string);
            _typeNameConverter = (JsonConverter<string>)options.GetConverter(_typeNameType);
        }

        public override SerializableEntity Read
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException();

            var entityType = ReadType(ref reader, options);
            var entity = ReadEntity(ref reader, entityType, options);

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject) throw new JsonException();

            return new SerializableEntity(entityType, entity);
        }

        private Type ReadType(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
            if (reader.GetString() != nameof(_entityType)) throw new JsonException();

            reader.Read();
            var typeName = _typeNameConverter.Read(ref reader, _typeNameType, options)
                           ?? throw new JsonException();

            var entityType = Type.GetType(typeName);
            return entityType ?? throw new NullReferenceException(nameof(entityType));
        }

        private object ReadEntity
            (ref Utf8JsonReader reader, Type entityType, JsonSerializerOptions options)
        {
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException();
            if (reader.GetString() != nameof(_entity)) throw new JsonException();

            reader.Read();

            try
            {
                return TryReadEntity(ref reader, options, entityType);
            }
            catch (Exception e)
            {
                throw new JsonException("Failed to call JsonConverter.Read", e);
            }
        }

        private object TryReadEntity
            (ref Utf8JsonReader reader, JsonSerializerOptions options, Type entityType)
        {
            var entityConverter = options.GetConverter(entityType);

            var converterConstExpr = Expression.Constant(entityConverter);

            var readMethod = entityConverter
                .GetType()
                .GetMethod
                (
                    nameof(JsonConverter<object>.Read),
                    BindingFlags.Public | BindingFlags.Instance
                );
            var readMethodParams = readMethod
                .GetParameters()
                .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                .ToArray();

            var readCallExpr = Expression.Call(converterConstExpr, readMethod, readMethodParams);
            var readCallExprAsObj = Expression.TypeAs(readCallExpr, typeof(object));

            var readDelegateExpr = Expression
                .Lambda<JsonConverterRead>(readCallExprAsObj, readMethodParams);

            var readResult = readDelegateExpr.Compile()(ref reader, entityType, options);

            return readResult;
        }

        public override void Write
            (Utf8JsonWriter writer, SerializableEntity value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(_entityType));
            _typeNameConverter.Write(writer, value.GetEntityTypeName(), options);

            writer.WritePropertyName(nameof(_entity));

            try
            {
                TryWriteEntity(writer, value, options);
            }
            catch (Exception e)
            {
                throw new JsonException("Failed to call JsonConverter.Write", e);
            }

            writer.WriteEndObject();
        }

        private void TryWriteEntity
            (Utf8JsonWriter writer, SerializableEntity value, JsonSerializerOptions options)
        {
            var entityConverter = options.GetConverter(value._entityType);
            var entityConverterType = typeof(JsonConverter<>).MakeGenericType(value._entityType);
            entityConverterType.GetMethod(nameof(JsonConverter<object>.Write))
                ?
                .Invoke(entityConverter, new[] { writer, value._entity, options });
        }

        private delegate object JsonConverterRead
        (
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        );
    }
}