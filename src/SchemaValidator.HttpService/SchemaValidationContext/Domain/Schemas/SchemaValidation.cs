using System.Text;
using CSharpFunctionalExtensions;
using NJsonSchema;
using NJsonSchema.Validation;
using SchemaValidator.HttpService.Shared;

namespace SchemaValidator.HttpService.SchemaValidationContext.Domain.Schemas;

public class SchemaValidation : IService<SchemaValidation>
{
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
        
        if (errors.Count == 0)
            return Result.Success();

        var errorMessage = FormatErrorMessageForMultipleErrors(errors);
        return Result.Failure(errorMessage);
    }

    private string FormatErrorMessageForMultipleErrors(ICollection<ValidationError> errors)
    {
        var stringBuilder = new StringBuilder();
        foreach (var error in errors)
        {
            stringBuilder.AppendFormat("Path: {0} Error: {1}\n ", error.Path, error.Kind);
        }

        return stringBuilder.ToString();
    }
}