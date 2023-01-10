using Discord;
using Discord.WebSocket;
using Dotbot.Common.CommandHandlers;
using Dotbot.Common.Models;
using Dotbot.Discord.Extensions;

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
        return _message.Channel.AsGuildChannel()?.Guild.Id.ToString();
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

    public async Task SendEmbedAsync(Embed build)
    {
        await _message.Channel.SendMessageAsync(embed: build);
    }

    public async Task<string?> GetAvatarImageUrl(ulong userId)
    {
        var user = _message.Channel.AsGuildChannel()?.Guild.GetUser(userId);
        return user?.GetDisplayAvatarUrl();
    }

    public async Task<List<User>> GetUserMentionsAsync()
    {
        return _message.MentionedUsers.Select(x => DiscordUserToUser((IGuildUser) x)).ToList();
    }

    public async Task<User?> GetUserAsync(ulong userId)
    {
        var user = _message.Channel.AsGuildChannel()?.Guild.GetUser(userId);
        return user == null ? null : DiscordUserToUser(user);
    }
    
    private static User DiscordUserToUser(IGuildUser user)
    {
        return new User
        {
            Id = user.Id,
            EffectiveAvatarUrl = user.GetDisplayAvatarUrl(size:512),
            Nickname = user.Nickname,
            Username = $"{user.Username}#{user.Discriminator}"
        }; 
    }
}