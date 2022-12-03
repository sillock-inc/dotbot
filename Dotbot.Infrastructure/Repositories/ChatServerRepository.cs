using Dotbot.Infrastructure.Entities;
using FluentResults;
using MongoDB.Driver;

namespace Dotbot.Infrastructure.Repositories;

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
}