using CSharpFunctionalExtensions;
using NJsonSchema;
using SchemaValidator.HttpService.Shared;

namespace SchemaValidator.HttpService.SchemaValidationContext.Domain.Schemas;

public class SchemaFactory : IService<SchemaFactory>
{
    // TODO: revamp the way schemas are registered!
    private Dictionary<string, JsonSchema> _jsonSchemaRegistry = new Dictionary<string, JsonSchema>()
    {
        { "Person", JsonSchema.FromType<Person.Person>() },
    };
    
    // TODO: revamp factory!
    public Result<JsonSchema> CreateJsonSchema(string schemaType)
    {
        var schema = RetrieveRegisteredSchema(schemaType);
        if (schema.HasNoValue)
            return Result.Failure<JsonSchema>("Schema not found");
        return schema.Value;
    }

    private Maybe<JsonSchema> RetrieveRegisteredSchema(string type)
    {
        var schema = _jsonSchemaRegistry.GetValueOrDefault(type);
        if (schema == null)
            return Maybe<JsonSchema>.None;
        return schema;
    }
    
    // TODO: create schema registry process throught a builder
    // Ex: SchemaRegistryBuilder.AddSchema(key, schema)
}