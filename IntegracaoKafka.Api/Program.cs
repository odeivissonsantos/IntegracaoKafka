using IntegracaoKafka.Api;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = $"v{typeof(Program).Assembly.GetName().Version?.ToString()} " +
            $"{(builder.Configuration["Ambiente"] == "1" ? "Produção" : "Desenvolvimento")}",
        Title = "ApiIntegracaoKafka",
        Description = "API de abertura e consultas acerca dos serviços Kafka"
    });
});

builder.ConfigureDI();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
