using Discord;
using Dotbot.Common.Models;

namespace Dotbot.Common.CommandHandlers;

public interface IServiceContext
{
    Task ReplyAsync(string msg);
    Task SendMessageAsync(string msg);
    Task SendFileAsync(string fileName, Stream fs);
    Task<string> GetServerId();
    Task<string> GetChannelId();
    Task<bool> HasAttachments();
    Task<IReadOnlyCollection<Attachment>> GetAttachments();
    Task SendEmbedAsync(Embed build); //TODO: Replace with our own rolled embed builder or something
    Task<List<User>> GetUserMentionsAsync();
    Task<User?> GetUserAsync(ulong userId);
}