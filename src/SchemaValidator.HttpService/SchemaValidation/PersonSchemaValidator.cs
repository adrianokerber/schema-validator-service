using CSharpFunctionalExtensions;
using NJsonSchema;
using SchemaValidator.HttpService.Schemas.Person;

namespace SchemaValidator.HttpService.SchemaValidation;

public static class PersonSchemaValidator
{
    private const string ErrorTemplate = "Path: {0} Error: {1}";
    
    static JsonSchema _personSchema = JsonSchema.FromType<Person>();
    
    public static Result Validate(string json)
    {
        var errors = _personSchema.Validate(json);
        
        Result result = Result.Success();
        foreach (var error in errors)
        {
            var errorMessage = string.Format(ErrorTemplate, error.Path, error.Kind);
            result = Result.Combine("\n", result, Result.Failure(errorMessage));
        }

        return result;
    }
}