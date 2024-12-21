using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var cosmosEndpoint = builder.Configuration["CosmosDb:AccountEndpoint"] ?? "https://";
var cosmosKey = builder.Configuration["CosmosDb:AccountKey"] ?? "key";
var cosmosDatabaseName = builder.Configuration["CosmosDb:DatabaseName"] ?? "DatabaseName";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseCosmos(
        cosmosEndpoint,
        cosmosKey,
        cosmosDatabaseName,
        cosmosOptions =>
        {
            cosmosOptions.ConnectionMode(ConnectionMode.Gateway);
            cosmosOptions.WebProxy(new WebProxy());
            cosmosOptions.LimitToEndpoint();
            cosmosOptions.Region(Regions.AustraliaCentral);
            cosmosOptions.GatewayModeMaxConnectionLimit(32);
            cosmosOptions.MaxRequestsPerTcpConnection(8);
            cosmosOptions.MaxTcpConnectionsPerEndpoint(16);
            cosmosOptions.IdleTcpConnectionTimeout(TimeSpan.FromMinutes(1));
        }));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "Hello World!aaa");

app.Run();
