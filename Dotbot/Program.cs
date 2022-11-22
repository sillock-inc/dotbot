using Discord;
using Discord.WebSocket;var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var discordConfig = new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.AllUnprivileged
};

builder.Services.AddSingleton(discordConfig);
builder.Services.AddSingleton<DiscordSocketClient>();


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

client.MessageReceived += async msg =>
{
    await Task.CompletedTask;
    Console.WriteLine(msg);
};

await client.LoginAsync(TokenType.Bot, "");
await client.StartAsync();
app.Run();

//await Task.Delay(Timeout.Infinite);