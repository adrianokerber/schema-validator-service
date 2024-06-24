using FastEndpoints;
using SchemaValidator.HttpService.Shared;

namespace SchemaValidator.HttpService.SchemaValidationContext.Features.ValidateJsonSchema;

public class PostEndpoint : Endpoint<PostRequest, object>
{
    private readonly HttpResponseFactory _httpResponseFactory;
    private readonly SchemaValidatorService _schemaValidatorService;

    public PostEndpoint(HttpResponseFactory httpResponseFactory, SchemaValidatorService schemaValidatorService)
    {
        _httpResponseFactory = httpResponseFactory;
        _schemaValidatorService = schemaValidatorService;
    }

    public override void Configure()
    {
        Post("/validate-json");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PostRequest req, CancellationToken ct)
    {
        var result = _schemaValidatorService.Validate(req.Metadata.Entity, req.Json);
        if (result.IsFailure)
        {
            await SendResultAsync(_httpResponseFactory.CreateErrorWith400("Invalid JSON", result.Error));
            return;
        }

        await SendResultAsync(_httpResponseFactory.CreateSuccessWith200("Valid JSON"));
    }
}