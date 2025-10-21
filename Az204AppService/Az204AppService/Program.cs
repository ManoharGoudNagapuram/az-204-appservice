using Azure.Identity;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFile("Logs/startup.log");
Console.WriteLine($"Running in environment: {builder.Environment.EnvironmentName}");

// Add services to the container.

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
else
{
    string? kvUri = builder.Configuration["KeyVaultUri"];
    if (!string.IsNullOrEmpty(kvUri))
    {
        var azureServiceTokenProvider = new DefaultAzureCredential();
        builder.Configuration.AddAzureKeyVault(new Uri(kvUri), azureServiceTokenProvider);
    }
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
