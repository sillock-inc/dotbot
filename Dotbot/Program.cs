using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Dotbot.Common.CommandHandlers;
using Dotbot.Common.Factories;
using Dotbot.Common.Settings;
using Dotbot.Database;
using Dotbot.Database.Entities;
using Dotbot.Database.Extensions;
using Dotbot.Database.Repositories;
using Dotbot.Database.Services;
using Dotbot.Database.Settings;
using Dotbot.Discord.EventListeners;
using Dotbot.Discord.Extensions;
using Dotbot.Discord.InteractionHandler;
using Dotbot.Discord.Services;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Dotbot;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        //builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();


        var section = builder.Configuration.GetSection("MongoDbSettings");
        var settings = MongoClientSettings.FromConnectionString(section["ConnectionString"]);
        var mongoClient = new MongoClient(settings);

        //Discord registrations
        var discordConfig = new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers |
                             GatewayIntents.MessageContent | GatewayIntents.GuildVoiceStates,
            AlwaysDownloadUsers = true,
        };
        //builder.Services.AddSingleton<IMongoClient>(mongoClient);
        var db = mongoClient.GetDatabase("test");
        builder.Services.AddSingleton(db);
        builder.Services.AddSingleton<IGridFSBucket>(new GridFSBucket(db));
        builder.Services.AddMongoDbCollection<ChatServer, ChatServerClassMapExtension>("springGuild", new ChatServerClassMapExtension());
        builder.Services.AddMongoDbCollection<BotCommand, DiscordCommandClassMapExtension>("DiscordCommands", new DiscordCommandClassMapExtension());

        builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
        builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));
        
        builder.Services.AddSingleton(discordConfig);
        builder.Services.AddSingleton<DiscordSocketClient>();
        builder.Services.AddSingleton<IAudioService, AudioService>();
        builder.Services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
        builder.Services.AddSingleton<InteractionHandler>();
        builder.Services.AddSingleton<MessageReceivedEventListener>();
        builder.Services.AddHttpClient<SaveBotCommandHandler>();
        
        
        //TODO: .AddImplementingInterfaces
        builder.Services.AddTransient<IFileService, FileService>();
        builder.Services.AddTransient<IBotCommandHandler, DefaultBotCommandHandler>();
        builder.Services.AddTransient<IBotCommandHandler, PingBotCommandHandler>();
        builder.Services.AddTransient<IBotCommandHandler, SaveBotCommandHandler>();
        builder.Services.AddTransient<IBotCommandHandler, SavedCommandHandler>();
        builder.Services.AddTransient<IBotCommandHandler, AvatarCommandHandler>();
        builder.Services.AddSingleton<IBotCommandHandlerFactory, BotCommandHandlerFactory>();
        builder.Services.AddTransient<IChatServerRepository, ChatServerRepository>();
        builder.Services.AddTransient<IBotCommandRepository, BotCommandRepository>();
        builder.Services.AddSingleton<DbContext>(c =>
            new DbContext(c.GetRequiredService<IMongoCollection<BotCommand>>(),
                c.GetRequiredService<IMongoCollection<ChatServer>>()));
        
        builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //}

        //app.UseHttpsRedirection();

        //app.UseAuthorization();

        //app.MapControllers();

        var client = ClientEventRegistrations.RegisterClientEvents(app.Services);
        
        await app.Services.GetRequiredService<InteractionHandler>()
            .InitializeAsync();

        //var listener = app.Services.GetRequiredService<DiscordEventListener>();
        //await listener.StartAsync();
        await client.LoginAsync(TokenType.Bot, builder.Configuration["Discord:BotToken"]);
        await client.StartAsync();
        await client.SetGameAsync("Getting re-written in .NET");
        await app.RunAsync();
    }
}