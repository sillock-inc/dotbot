using Dotbot.Common.Models;
using FluentResults;
using MongoDB.Driver;
using static FluentResults.Result;

namespace Dotbot.Common.Services;

public class BotCommandService : IBotCommandService
{
    private readonly IMongoCollection<BotCommand> _collection;
    private readonly IGridFsFileService _gridFsFileService;
        
    public BotCommandService(IMongoCollection<BotCommand> collection, IGridFsFileService gridFsFileService)
    {
        _collection = collection;
        _gridFsFileService = gridFsFileService;
    }

    public async Task<Result<BotCommand>> GetCommand(string serverId, string key)
    {
        var cmd = _collection.AsQueryable().FirstOrDefault(x => x.ServiceId == serverId && x.Key == key);

        return cmd == null ? Fail("Command not found") : Ok(cmd);
    }

    public async Task<Result<Stream>> GetCommandFileStream(BotCommand command)
    {
        if (command.Type != BotCommand.CommandType.FILE) return Fail("Command is not a file");
        
        return await GetCommandFileStream(command.ServiceId, command.Key, command.FileName);
    }

    public async Task<Result<Stream>> GetCommandFileStream(string serverId, string key, string fileName)
    {
        return await _gridFsFileService.GetFile($"{serverId}:{fileName}:{key}");
    }

    public async Task<Result> SaveCommand(string serverId, string key, string content, bool overwrite = false)
    {
        var command = await GetCommand(serverId, key);
        if (!overwrite && command.IsSuccess)
        {
            return Fail("Command already exists"); 
        }

        if (command.IsSuccess)
        {
            await _collection.InsertOneAsync(new BotCommand{Content = content, Key = key, ServiceId = serverId, Type = BotCommand.CommandType.STRING});
        }
        else
        {
            await _collection.FindOneAndReplaceAsync(command.Value.Id, new BotCommand{Content = content, Key = key, ServiceId = serverId, Type = BotCommand.CommandType.STRING});
        }
        return Ok();
    }

    public async Task<Result> SaveCommand(string serverId, string key, string fileName, Stream fileStream, bool overwrite = false)
    {
        var command = await GetCommand(serverId, key);
        if (!overwrite && command.IsSuccess)
        {
            return Fail("Command already exists");
        }

        await _gridFsFileService.SaveFile($"{serverId}:{fileName}:{key}", fileStream);
        
        if (command.IsSuccess)
        {
            await _collection.InsertOneAsync(new BotCommand{FileName = fileName, Key = key, ServiceId = serverId, Type = BotCommand.CommandType.FILE});
        }
        else
        {
            await _collection.FindOneAndReplaceAsync(command.Value.Id, new BotCommand{FileName = fileName, Key = key, ServiceId = serverId, Type = BotCommand.CommandType.FILE});
        }
        return Ok();
    }
}