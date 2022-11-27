using Dotbot.Common.Models;
using FluentResults;
using MongoDB.Driver;

namespace Dotbot.Common.Services;

public class ChatServerService : IChatServerService
{
    private readonly IMongoCollection<ChatServer> _collection;

    public ChatServerService(IMongoCollection<ChatServer> collection)
    {
        _collection = collection;
    }

    public async Task<Result<ChatServer>> GetServerAsync(string serverId)
    {
        var server = _collection.AsQueryable().FirstOrDefault(x => x.ServiceId == serverId);

        return server == null ? Result.Fail("No such server") : Result.Ok(server);

    }

    public async Task<Result<ChatServer>> CreateServerAsync(string serverId)
    {
        if ((await GetServerAsync(serverId)).IsSuccess)
        {
            return Result.Fail("Server already exists");
        }
        await _collection.InsertOneAsync(new ChatServer(serverId));
        return await GetServerAsync(serverId);
    }
}