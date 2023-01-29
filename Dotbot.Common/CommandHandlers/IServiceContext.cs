using Dotbot.Common.Models;

namespace Dotbot.Common.CommandHandlers;

public interface IServiceContext
{
    Privilege Privilege();
    Task ReplyAsync(string msg);
    Task SendMessageAsync(string msg);
    Task SendFileAsync(string fileName, Stream fs);
    Task<string> GetServerId();
    Task<string> GetChannelId();
    Task<bool> HasAttachments();
    Task<IReadOnlyCollection<MessageAttachment>> GetAttachments();
    Task SendEmbedAsync(FormattedMessage build); 
    Task<List<User>> GetUserMentionsAsync();
    Task<User?> GetUserAsync(ulong userId);
    Task<ulong> GetAuthorId();
}

public enum Privilege {
    Admin,
    Moderator,
    Any
}