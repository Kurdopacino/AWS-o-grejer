using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<EncryptionService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// GET-endpoints
app.MapGet("/encrypt/{text}", (string text, EncryptionService service) =>
{
    return new { EncryptedText = service.Encrypt(text) };
});

app.MapGet("/decrypt/{text}", (string text, EncryptionService service) =>
{
    return new { DecryptedText = service.Decrypt(text) };
});

// POST-endpoints för JSON-anrop
app.MapPost("/encrypt", ([FromBody] InputModel model, EncryptionService service) =>
{
    return Results.Ok(new { EncryptedText = service.Encrypt(model.Text) });
});

app.MapPost("/decrypt", ([FromBody] InputModel model, EncryptionService service) =>
{
    return Results.Ok(new { DecryptedText = service.Decrypt(model.Text) });
});

app.Run(); // Kör applikationen

// Modell för indata
public class InputModel
{
    public string Text { get; set; }
}

// Krypteringstjänst
public class EncryptionService
{
    private const int Shift = 3; // Hur många steg vi flyttar bokstäver

    public string Encrypt(string input)
    {
        return new string(input.Select(c => (char)(c + Shift)).ToArray());
    }

    public string Decrypt(string input)
    {
        return new string(input.Select(c => (char)(c - Shift)).ToArray());
    }
    
}