using Dotbot.Infrastructure.Entities;
using MongoDB.Driver;

namespace Dotbot.Infrastructure;

public class DbContext
{
    public DbContext(IMongoCollection<BotCommand> botCommands, IMongoCollection<ChatServer> chatServers)
    {
        BotCommands = botCommands;
        ChatServers = chatServers;
    }

    public IMongoCollection<BotCommand> BotCommands { get; }
    public IMongoCollection<ChatServer> ChatServers { get; }
}