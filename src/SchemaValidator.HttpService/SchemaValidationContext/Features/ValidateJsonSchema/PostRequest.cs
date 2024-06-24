namespace SchemaValidator.HttpService.SchemaValidationContext.Features.ValidateJsonSchema;

public record PostRequest(Metadata Metadata, string Json);
public record Metadata(string Entity, int? SchemaVersion);