using Discord;
using Discord.WebSocket;
using Dotbot.Common.CommandHandlers;

namespace Dotbot.Discord.CommandHandlers;

public class DiscordChannelMessageContext : IServiceContext
{
    private readonly SocketMessage _message;

    public DiscordChannelMessageContext(SocketMessage message)
    {
        _message = message;
    }

    public async Task ReplyAsync(string msg)
    {
        var msgRef = new MessageReference(_message.Id);
        await _message.Channel.SendMessageAsync(msg, false, null, RequestOptions.Default, AllowedMentions.All, msgRef);
    }

    public async Task SendMessageAsync(string msg)
    {
        await _message.Channel.SendMessageAsync(msg, false, null, RequestOptions.Default, AllowedMentions.All);
    }

    public async Task SendFileAsync(string fileName, Stream fs)
    {
        await _message.Channel.SendFileAsync(fs, fileName);
    }

    public async Task<string> GetServerId()
    {
        return (_message.Channel as SocketGuildChannel).Guild.Id.ToString();
    }

    public async Task<string> GetChannelId()
    {
        return _message.Channel.Id.ToString();
    }

    public async Task<bool> HasAttachments()
    {
        return _message.Attachments.Any();
    }

    public async Task<IReadOnlyCollection<Attachment>> GetAttachments()
    {
        return _message.Attachments;
    }
}