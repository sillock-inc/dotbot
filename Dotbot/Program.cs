using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Dotbot.Discord.Models;
using Dotbot.Discord.InteractionHandler;
using Dotbot.Discord.Services;
using Dotbot.EventHandlers;
using MediatR;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var discordConfig = new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.AllUnprivileged  | GatewayIntents.GuildMembers | GatewayIntents.MessageContent | GatewayIntents.GuildVoiceStates,
    AlwaysDownloadUsers = true,
};

// var section = builder.Configuration.GetSection("MongoDbSettings");
//
// var settings = MongoClientSettings.FromConnectionString(section["ConnectionString"]);
//
// var mongoClient = new MongoClient(settings);
//
// var collection = mongoClient.GetDatabase("test").GetCollection<ChatServer>("springGuild");
//
// var ourServer = collection.AsQueryable().FirstOrDefault(x => x.ServiceId == "632651372260753458");

builder.Services.AddSingleton(discordConfig);
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IAudioService, AudioService>();
builder.Services.AddSingleton<DiscordEventListener>();
builder.Services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
builder.Services.AddSingleton<InteractionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var client = app.Services.GetRequiredService<DiscordSocketClient>();
await app.Services.GetRequiredService<InteractionHandler>()
    .InitializeAsync();

client.Log += async (msg) =>
{
    await Task.CompletedTask;
    Console.WriteLine(msg.Message);
};
var listener = app.Services.GetRequiredService<DiscordEventListener>();
await listener.StartAsync();
await client.LoginAsync(TokenType.Bot, builder.Configuration["Discord:BotToken"]);
await client.StartAsync();
app.Run();

//await Task.Delay(Timeout.Infinite);
