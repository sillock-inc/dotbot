using Dotbot.Infrastructure.Entities;
using FluentResults;

namespace Dotbot.Infrastructure.Repositories;

public interface IBotCommandRepository : IRepository<BotCommand>
{
    Task<Result<BotCommand>> GetCommand(string serverId, string key);
    Task<Result> SaveCommand(string serverId, string key, string content, bool overwrite = false);
    Task<Result> SaveCommand(string serverId, string key, string fileName, Stream fileStream, bool overwrite = false);
}