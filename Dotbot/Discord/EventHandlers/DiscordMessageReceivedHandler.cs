using Discord;
using Dotbot.Discord.Events;
using MediatR;

namespace Dotbot.Discord.EventHandlers;

public class DiscordMessageReceivedHandler : INotificationHandler<DiscordMessageReceivedNotification>
{
    public async Task Handle(DiscordMessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"MediatR works! (Received a message by {notification.Message.Author.Username})");
        var msgRef = new MessageReference(notification.Message.Id);
        await notification.Message.Channel.SendMessageAsync("Boo", false, null, RequestOptions.Default,
            AllowedMentions.None, msgRef);
        // Your implementation
    }
}