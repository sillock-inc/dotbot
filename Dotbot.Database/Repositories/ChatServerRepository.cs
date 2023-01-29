using Dotbot.Database.Entities;
using FluentResults;
using MongoDB.Driver;

namespace Dotbot.Database.Repositories;

public class ChatServerRepository : IChatServerRepository
{
    private readonly DbContext _dbContext;

    public ChatServerRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ChatServer>> GetServerAsync(string serverId)
    {
        var server = _dbContext.ChatServers.AsQueryable().FirstOrDefault(x => x.ServiceId == serverId);

        return server == null ? Result.Fail("No such server") : Result.Ok(server);

    }

    public async Task<Result<ChatServer>> CreateServerAsync(string serverId)
    {
        if ((await GetServerAsync(serverId)).IsSuccess)
        {
            return Result.Fail("Server already exists");
        }
        await _dbContext.ChatServers.InsertOneAsync(new ChatServer(serverId));
        return await GetServerAsync(serverId);
    }

    public async Task AddModId(string serverId, string modId)
    {
        var filter = Builders<ChatServer>.Filter.Where(x => x.ServiceId == serverId);
        var update = Builders<ChatServer>.Update.Push(u => u.ModeratorIds, modId);
        await _dbContext.ChatServers.UpdateOneAsync(filter, update);
    }
}