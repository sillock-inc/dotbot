using System.Data.Common;
using System.Reflection;
using Bot.Gateway.Infrastructure;
using Bot.Gateway.Services;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.LocalStack;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Bot.Gateway.FunctionalTests.Setup;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithDatabase("postgres")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithImage("postgres:16")
        .WithPortBinding(54321, 5432)
        .WithCleanUp(true)
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3-management")
        .WithPortBinding(5672, true)
        .Build();

    private readonly LocalStackContainer _localStackContainer = new LocalStackBuilder()
        .WithImage("localstack/localstack:2.0")
        .Build();
    
        
    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public HttpClient HttpClient { get; private set; } = null!;
    public string PostgresConnectionString { get; private set; } = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.Sources.Add(new MemoryConfigurationSource
            {
                InitialData = new Dictionary<string, string>
                {
                    {"S3:ServiceURL", _localStackContainer.GetConnectionString()},
                    {"AWS_ACCESS_KEY_ID", "unused"},
                    {"AWS_SECRET_ACCESS_KEY", "unused"}
                }!
            });
        });
        builder.ConfigureTestServices(services =>
        {
            services.RemoveHostedService<DiscordRegistrationService>();
            services.RemoveDbContext<DotbotContext>();
            services.AddDbContext<DotbotContext>(options => { options.UseNpgsql(_dbContainer.GetConnectionString()); });
            services.EnsureDbCreated<DotbotContext>();
            services.AddMassTransitTestHarness(x =>
            {
                x.AddConsumers(Assembly.GetExecutingAssembly());
                x.UsingRabbitMq((context,cfg) =>
                {
                    cfg.Host(_rabbitMqContainer.Hostname, 
                        _rabbitMqContainer.GetMappedPublicPort(5672),
                        "/", 
                        h => {
                            h.Username("rabbitmq");
                            h.Password("rabbitmq");
                        });
                    cfg.UseDelayedMessageScheduler();                
                    cfg.ConfigureEndpoints(context);
                });
                x.SetTestTimeouts(testInactivityTimeout: TimeSpan.FromSeconds(10), testTimeout: TimeSpan.FromSeconds(10));
            });
            services.AddMockAuthentication();
        });
    }
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
        await _localStackContainer.StartAsync();
        
        PostgresConnectionString = _dbContainer.GetConnectionString();
        _dbConnection = new NpgsqlConnection(PostgresConnectionString);
        HttpClient = CreateClient();
        
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
        await _dbConnection.DisposeAsync();
        await _localStackContainer.DisposeAsync();
    }
    
    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
}