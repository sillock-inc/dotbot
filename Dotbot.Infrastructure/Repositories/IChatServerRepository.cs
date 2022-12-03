using Dotbot.Infrastructure.Entities;
using FluentResults;

namespace Dotbot.Infrastructure.Repositories;

public interface IChatServerRepository : IRepository<ChatServer>
{
    Task<Result<ChatServer>> GetServerAsync(string serverId);
    Task<Result<ChatServer>> CreateServerAsync(string serverId);
}