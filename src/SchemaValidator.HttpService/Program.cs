using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using SchemaValidator.HttpService.Schemas;
using SchemaValidator.HttpService.SchemaValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/validate-schema", ([Required] ValidationRequest validationRequest) =>
    {
        // 1. Obter schema do DB ou factory baseado nos metadados
        var schema = SchemaFactory.CreateJsonSchema(validationRequest.Metadata.SchemaType);
        if (schema.IsFailure)
            return Results.BadRequest($"Schema error... {schema.Error}");
        
        var result = SchemaValidation.Exec(schema.Value, validationRequest.Json);

        if (result.IsFailure)
            return Results.BadRequest($"Errors:\n{result.Error}");
        return Results.Ok("Valid JSON");
    })
    .WithName("ValidateJsonSchema")
    .WithOpenApi();

// TODO: create a HttpResponseFactory to format responses
// TODO: create a CRUD for schemas if stored as JSON on a DB instead of a static factory

app.Run();

public record ValidationRequest(Metadata Metadata, string Json);
public record Metadata(string SchemaType);
