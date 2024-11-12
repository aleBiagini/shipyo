var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    builder.WebHost.UseUrls("http://localhost:5002");
}
var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello ShipYo DEV!");
app.MapGet("/shipyo", () => "Hello ShipYo! PROD");

// Esegui l'app
app.Run();
