using CSharpFunctionalExtensions;
using NJsonSchema;

namespace SchemaValidator.HttpService.Schemas;

public static class SchemaFactory
{
    // TODO: revamp factory!
    public static Result<JsonSchema> CreateJsonSchema(string schemaType)
    {
        switch (schemaType)
        {
            case "Person":
                return CreatePersonSchema();
            default:
                return Result.Failure<JsonSchema>("Schema not found");
        }
    }

    private static JsonSchema CreatePersonSchema()
        => JsonSchema.FromType<Person.Person>();
}