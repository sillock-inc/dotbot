using Dotbot.Common.Models;
using MongoDB.Driver;

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

    public async Task<BotCommand?> GetCommand(string serverId, string key)
    {
        return _collection.AsQueryable().FirstOrDefault(x => x.ServiceId == serverId && x.Key == key);
    }

    public async Task<Stream?> GetCommandFileStream(BotCommand command)
    {
        if (command.Type != BotCommand.CommandType.FILE) return null;
        
        return await GetCommandFileStream(command.ServiceId, command.Key, command.FileName);
    }

    public async Task<Stream?> GetCommandFileStream(string serverId, string key, string fileName)
    {
        return await _gridFsFileService.GetFile($"{serverId}:{fileName}:{key}");
    }

    public async Task SaveCommand(string serverId, string key, string content, bool overwrite = false)
    {
        var command = await GetCommand(serverId, key);
        if (!overwrite && command != null)
        {
            return; //TODO: Result pattern    
        }

        if (command == null)
        {
            await _collection.InsertOneAsync(new BotCommand{Content = content, Key = key, ServiceId = serverId, Type = BotCommand.CommandType.STRING});
        }
        else
        {
            await _collection.FindOneAndReplaceAsync(command.Id, new BotCommand{Content = content, Key = key, ServiceId = serverId, Type = BotCommand.CommandType.STRING});
        }
    }

    public async Task SaveCommand(string serverId, string key, string fileName, Stream fileStream, bool overwrite = false)
    {
        var command = await GetCommand(serverId, key);
        if (!overwrite && command != null)
        {
            return; //TODO: Result pattern    
        }

        await _gridFsFileService.SaveFile($"{serverId}:{fileName}:{key}", fileStream);
        
        if (command == null)
        {
            await _collection.InsertOneAsync(new BotCommand{FileName = fileName, Key = key, ServiceId = serverId, Type = BotCommand.CommandType.FILE});
        }
        else
        {
            await _collection.FindOneAndReplaceAsync(command.Id, new BotCommand{FileName = fileName, Key = key, ServiceId = serverId, Type = BotCommand.CommandType.FILE});
        }
    }
}