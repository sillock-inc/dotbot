using Dotbot.Common.Models;
using FluentResults;

namespace Dotbot.Common.Services;

public interface IChatServerService
{
    Task<Result<ChatServer>> GetServerAsync(string serverId);
    Task<Result<ChatServer>> CreateServerAsync(string serverId);
}