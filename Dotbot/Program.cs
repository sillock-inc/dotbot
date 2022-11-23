using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Dotbot.Discord.Services;
using Dotbot.EventHandlers;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var discordConfig = new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.AllUnprivileged  | GatewayIntents.GuildMembers | GatewayIntents.MessageContent | GatewayIntents.GuildVoiceStates
};

builder.Services.AddSingleton(discordConfig);
builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IAudioService, AudioService>();
builder.Services.AddSingleton<DiscordEventListener>();

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
