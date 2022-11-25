using Dotbot.Common.Factories;
using Dotbot.Discord.CommandHandlers;
using Dotbot.Discord.Events;
using MediatR;

namespace Dotbot.Discord.EventHandlers;

public class DiscordMessageReceivedHandler : INotificationHandler<DiscordMessageReceivedNotification>
{
    private readonly IBotCommandHandlerFactory _commandHandlerFactory;

    public DiscordMessageReceivedHandler(IBotCommandHandlerFactory commandHandlerFactory)
    {
        _commandHandlerFactory = commandHandlerFactory;
    }

    public async Task Handle(DiscordMessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"MediatR works! (Received a message by {notification.Message.Author.Username})");

        var messageSplit = notification.Message.Content.Split(' ');

        if (messageSplit[0].StartsWith(">"))
        {
            await _commandHandlerFactory.GetCommand(messageSplit[0][1..]).HandleAsync(notification.Message.Content[1..], new DiscordChannelMessageContext(notification.Message));
        }
    }
}