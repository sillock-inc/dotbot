using Dotbot.Common.Factories;
using Dotbot.Discord.CommandHandlers;
using Dotbot.Discord.Events;
using MediatR;

namespace Dotbot.Discord.EventHandlers;

public class ChatCommandReceivedHandler : INotificationHandler<DiscordMessageReceivedNotification>
{
    private readonly ILogger _logger;
    private readonly IBotCommandHandlerFactory _commandHandlerFactory;

    public ChatCommandReceivedHandler(IBotCommandHandlerFactory commandHandlerFactory, ILogger<ChatCommandReceivedHandler> logger)
    {
        _commandHandlerFactory = commandHandlerFactory;
        _logger = logger;
    }

    public async Task Handle(DiscordMessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("<{AuthorUsername}>: {Message}", notification.Message.Author.Username, notification.Message.Content);

        var messageSplit = notification.Message.Content.Split(' ');

        if (messageSplit[0].StartsWith(">"))
        {
            await _commandHandlerFactory.GetCommand(messageSplit[0][1..]).HandleAsync(notification.Message.Content[1..], new DiscordChannelMessageContext(notification.Message));
        }
    }
}