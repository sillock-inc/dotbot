using Discord;
using Dotbot.Events;
using MediatR;

namespace Dotbot.EventHandlers;

public class MessageReceivedHandler : INotificationHandler<DiscordMessageReceivedNotification>
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