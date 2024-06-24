using CSharpFunctionalExtensions;
using SchemaValidator.HttpService.SchemaValidationContext.Domain.Schemas;
using SchemaValidator.HttpService.Shared;

namespace SchemaValidator.HttpService.SchemaValidationContext.Features.ValidateJsonSchema;

public class SchemaValidatorService : IService<SchemaValidatorService>
{
    private readonly SchemaFactory _schemaFactory;
    private readonly SchemaValidation _schemaValidation;
    
    public SchemaValidatorService(SchemaFactory schemaFactory, SchemaValidation schemaValidation)
    {
        _schemaFactory = schemaFactory;
        _schemaValidation = schemaValidation;
    }

    public Result Validate(string schemaType, string json)
    {
        var schema = _schemaFactory.CreateJsonSchema(schemaType);
        if (schema.IsFailure)
            return Result.Failure(schema.Error);
        
        var result = _schemaValidation.Exec(schema.Value, json);
        if (result.IsFailure)
            return Result.Failure(result.Error);
        return Result.Success();
    }
}