using Dotbot.Database.Entities;
using Dotbot.Database.Repositories;
using FluentResults;
using static FluentResults.Result;

namespace Dotbot.Common.Services;

public class ChatServerService : IChatServerService
{
    private readonly IChatServerRepository _repository;

    public ChatServerService(IChatServerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ChatServer>> Get(string id)
    {
        return await _repository.GetServerAsync(id);
    }

    public async Task<Result<ChatServer>> Create(string id)
    {
        return await _repository.CreateServerAsync(id);
    }

    public async Task<Result> AddModerator(string serverId, string modId)
    {
        var server = await Get(serverId);
        if (server.IsFailed)
        {
            return Fail(server.Errors);
        }

        await _repository.AddModId(serverId, modId);
        return Ok();
    }

    public Task<Result> RemoveModerator(string serverId, string modId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> IsModerator(string serverId, string modId)
    {
        var server = await Get(serverId);
        if (server.IsFailed)
        {
            return Fail(server.Errors);
        }

        return Ok(server.Value.ModeratorIds.Contains(modId));
    }

    public Task<Result> SetXkcdChannel(string serverId, string modId)
    {
        throw new NotImplementedException();
    }
}