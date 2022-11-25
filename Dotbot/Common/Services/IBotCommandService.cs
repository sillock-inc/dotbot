using Dotbot.Common.Models;

namespace Dotbot.Common.Services;

public interface IBotCommandService
{
    Task<BotCommand?> GetCommand(string serverId, string key);

    Task<Stream?> GetCommandFileStream(BotCommand command);
    Task<Stream?> GetCommandFileStream(string serverId, string key, string fileName);
    Task SaveCommand(string serverId, string key, string content, bool overwrite = false);
    Task SaveCommand(string serverId, string key, string fileName, Stream fileStream, bool overwrite = false);
}