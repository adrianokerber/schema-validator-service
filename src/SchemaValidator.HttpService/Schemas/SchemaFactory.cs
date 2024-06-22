using CSharpFunctionalExtensions;
using NJsonSchema;

namespace SchemaValidator.HttpService.Schemas;

public static class SchemaFactory
{
    // TODO: revamp factory!
    public static Result<JsonSchema> CreateJsonSchema(string schemaType)
    {
        var schema = RetrieveRegisteredSchema(schemaType);
        if (schema.HasNoValue)
            return Result.Failure<JsonSchema>("Schema not found");
        return schema.Value;
    }

    private static Maybe<JsonSchema> RetrieveRegisteredSchema(string type)
    {
        var schema = _jsonSchemaRegistry.GetValueOrDefault(type);
        if (schema == null)
            return Maybe<JsonSchema>.None;
        return schema;
    }
    
    // TODO: revamp the way schemas are registered!
    static Dictionary<string, JsonSchema> _jsonSchemaRegistry = new Dictionary<string, JsonSchema>()
    {
        { "Person", JsonSchema.FromType<Person.Person>() },
    };
}