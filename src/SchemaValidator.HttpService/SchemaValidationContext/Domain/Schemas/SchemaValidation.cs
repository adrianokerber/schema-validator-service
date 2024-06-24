using CSharpFunctionalExtensions;
using NJsonSchema;
using SchemaValidator.HttpService.Shared;

namespace SchemaValidator.HttpService.SchemaValidationContext.Domain.Schemas;

public class SchemaValidation : IService<SchemaValidation>
{
    private const string ErrorTemplate = "Path: {0} Error: {1}";
    
    public Result Exec(JsonSchema schema, string json)
    {
        var errors = schema.Validate(json);
        
        Result result = Result.Success();
        foreach (var error in errors)
        {
            var errorMessage = string.Format(ErrorTemplate, error.Path, error.Kind);
            result = Result.Combine(" \n", result, Result.Failure(errorMessage));
        }

        return result;
    }
}