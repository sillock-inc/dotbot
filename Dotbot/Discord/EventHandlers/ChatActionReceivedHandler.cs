using Discord;
using Dotbot.Common.Factories;
using Dotbot.Discord.CommandHandlers;
using Dotbot.Discord.Events;
using MediatR;

namespace Dotbot.Discord.EventHandlers;

public class ChatActionReceivedHandler : INotificationHandler<DiscordMessageReceivedNotification>
{
    private readonly ILogger _logger;

    public ChatActionReceivedHandler(ILogger<ChatActionReceivedHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(DiscordMessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        if (notification.Message.Content.ToLower() == "lol")
        {
            await notification.Message.AddReactionAsync(new Emoji(":rolling_on_the_floor_laughing:"));
        }
    }
}