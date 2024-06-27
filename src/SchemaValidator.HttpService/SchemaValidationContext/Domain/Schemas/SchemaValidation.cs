using CSharpFunctionalExtensions;
using NJsonSchema;
using NJsonSchema.Validation;
using SchemaValidator.HttpService.Shared;

namespace SchemaValidator.HttpService.SchemaValidationContext.Domain.Schemas;

public class SchemaValidation : IService<SchemaValidation>
{
    private const string ErrorTemplate = "Path: {0} Error: {1}";
    private readonly JsonSchemaValidator _schemaValidator;

    public SchemaValidation()
    {
        _schemaValidator = CreateSchemaValidator(false);
    }

    private JsonSchemaValidator CreateSchemaValidator(bool isCaseSensitive = true)
    {
        var comparerProperty = isCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        
        var settings = new JsonSchemaValidatorSettings()
        {
            PropertyStringComparer = comparerProperty,
        };

        return new JsonSchemaValidator(settings);
    }
    
    public Result Exec(JsonSchema schema, string json)
    {
        var errors = _schemaValidator.Validate(json, schema);
        
        Result result = Result.Success();
        foreach (var error in errors)
        {
            var errorMessage = string.Format(ErrorTemplate, error.Path, error.Kind);
            result = Result.Combine(" \n", result, Result.Failure(errorMessage));
        }

        return result;
    }
}