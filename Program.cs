using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Register EncryptionService
builder.Services.AddSingleton<EncryptionService>();

var app = builder.Build();

// Root endpoint
app.MapGet("/", () => "Hello World!");

// GET-endpoints
app.MapGet("/encrypt/{text}", (string text, EncryptionService service) =>
{
    if (string.IsNullOrWhiteSpace(text))
    {
        return Results.BadRequest("Text cannot be empty.");
    }
    return Results.Ok(new { EncryptedText = service.Encrypt(text) });
});

app.MapGet("/decrypt/{text}", (string text, EncryptionService service) =>
{
    if (string.IsNullOrWhiteSpace(text))
    {
        return Results.BadRequest("Text cannot be empty.");
    }
    return Results.Ok(new { DecryptedText = service.Decrypt(text) });
});

// POST-endpoints for JSON
app.MapPost("/encrypt", ([FromBody] InputModel model, EncryptionService service) =>
{
    if (string.IsNullOrWhiteSpace(model.Text))
    {
        return Results.BadRequest("Text cannot be empty.");
    }
    return Results.Ok(new { EncryptedText = service.Encrypt(model.Text) });
});

app.MapPost("/decrypt", ([FromBody] InputModel model, EncryptionService service) =>
{
    if (string.IsNullOrWhiteSpace(model.Text))
    {
        return Results.BadRequest("Text cannot be empty.");
    }
    return Results.Ok(new { DecryptedText = service.Decrypt(model.Text) });
});

app.Run(); // Always run the application

// Input model
public class InputModel
{
    public string Text { get; set; } = string.Empty;
}

// Encryption service
public class EncryptionService
{
    private const int Shift = 3; // Shift for Caesar cipher

    public string Encrypt(string input)
    {
        return new string(input.Select(c => (char)(c + Shift)).ToArray());
    }

    public string Decrypt(string input)
    {
        return new string(input.Select(c => (char)(c - Shift)).ToArray());
    }
}
