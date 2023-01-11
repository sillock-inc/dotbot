using Discord;
using Discord.WebSocket;
using Dotbot.Common.CommandHandlers;
using Dotbot.Common.Models;
using Dotbot.Discord.Extensions.Discord;

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

    public async Task<IReadOnlyCollection<MessageAttachment>> GetAttachments()
    {
        return _message.Attachments.Select(Convert).ToList();
    }

    public async Task SendEmbedAsync(FormattedMessage build)
    {
        await _message.Channel.SendMessageAsync(embed: Convert(build));
    }


    public async Task<string?> GetAvatarImageUrl(ulong userId)
    {
        var user = _message.Channel.AsGuildChannel()?.Guild.GetUser(userId);
        return user?.GetDisplayAvatarUrl();
    }

    public async Task<List<User>> GetUserMentionsAsync()
    {
        return _message.MentionedUsers.Select(x => Convert((IGuildUser)x)).ToList();
    }

    public async Task<User?> GetUserAsync(ulong userId)
    {
        var user = _message.Channel.AsGuildChannel()?.Guild.GetUser(userId);
        return user == null ? null : Convert(user);
    }

    //TODO: Can probably make these extension functions or something
    private static User Convert(IGuildUser user)
    {
        return new User
        {
            Id = user.Id,
            EffectiveAvatarUrl = user.GetDisplayAvatarUrl(size: 512),
            Nickname = user.Nickname,
            Username = $"{user.Username}#{user.Discriminator}"
        };
    }

    private static MessageAttachment Convert(Attachment attachment)
    {
        return new MessageAttachment
        {
            Filename = attachment.Filename,
            Url = attachment.Url
        };
    }

    private static Embed Convert(FormattedMessage message)
    {
        Color? color = null;

        if (message.Color != null)
        {
            var msgColor = (System.Drawing.Color)message.Color;
            color = new Color(msgColor.R, msgColor.G, msgColor.B);
        }

        var eb = new EmbedBuilder
        {
            Title = message.Title,
            ImageUrl = message.ImageUrl,
            Description = message.Description,
            Color = color,
            Timestamp = message.Timestamp
        };

        foreach (var field in message.Fields)
        {
            eb.AddField(field.Name, field.Value, field.Inline);
        }

        return eb.Build();
    }

}