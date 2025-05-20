using System.Reflection;
using Contato.Cadastrar.Application.Interfaces;
using Contato.Cadastrar.Application.Services;
using Contato.Cadastrar.Infra.RabbitMq;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"Ambiente atual: {builder.Environment.EnvironmentName}");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();

builder.Services.AddScoped<IContatoService, ContatoService>();


builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});

// Define a porta HTTP
builder.WebHost.UseUrls("http://*:8080");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

// Middleware Prometheus para requisições HTTP
app.UseHttpMetrics();

// Endpoints das métricas
app.MapMetrics(); // Exposição do /metrics


app.MapControllers();

app.Run();