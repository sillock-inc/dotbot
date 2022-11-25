using Discord;

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
}