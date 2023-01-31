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

    public async Task<Result> RemoveModerator(string serverId, string modId)
    {
        var server = await Get(serverId);
        if (server.IsFailed)
        {
            return Fail(server.Errors);
        }

        await _repository.RemoveModId(serverId, modId);
        return Ok();
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

    public async Task<Result> SetXkcdChannel(string serverId, string channelId)
    {
        var server = await Get(serverId);
        if (server.IsFailed)
        {
            return Fail(server.Errors);
        }

        await _repository.SetXkcdChannelId(serverId, channelId);
        return Ok();
    }
}