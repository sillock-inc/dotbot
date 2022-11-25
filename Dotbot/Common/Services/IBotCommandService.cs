using Dotbot.Common.Models;
using FluentResults;

namespace Dotbot.Common.Services;

public interface IBotCommandService
{
    Task<Result<BotCommand>> GetCommand(string serverId, string key);

    Task<Result<Stream>> GetCommandFileStream(BotCommand command);
    Task<Result<Stream>> GetCommandFileStream(string serverId, string key, string fileName);
    Task<Result> SaveCommand(string serverId, string key, string content, bool overwrite = false);
    Task<Result> SaveCommand(string serverId, string key, string fileName, Stream fileStream, bool overwrite = false);
}