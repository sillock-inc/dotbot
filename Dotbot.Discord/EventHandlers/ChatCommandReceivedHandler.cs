using Dotbot.Common.Factories;
using Dotbot.Discord.CommandHandlers;
using Dotbot.Discord.Events;
using Dotbot.Discord.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Dotbot.Discord.EventHandlers;

public class ChatCommandReceivedHandler : INotificationHandler<DiscordMessageReceivedNotification>
{
    private readonly ILogger _logger;
    private readonly IBotCommandHandlerFactory _commandHandlerFactory;
    private readonly BotSettings _botSettings;
    public ChatCommandReceivedHandler(IBotCommandHandlerFactory commandHandlerFactory, ILogger<ChatCommandReceivedHandler> logger, IOptions<BotSettings> botSettings)
    {
        _commandHandlerFactory = commandHandlerFactory;
        _logger = logger;
        _botSettings = botSettings.Value;
    }

    public async Task Handle(DiscordMessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("<{AuthorUsername}>: {Message}", notification.Message.Author.Username, notification.Message.Content);

        var messageSplit = notification.Message.Content.Split(' ');

        if (messageSplit[0].StartsWith(_botSettings.CommandPrefix))
        {
            await _commandHandlerFactory.GetCommand(messageSplit[0][1..]).HandleAsync(notification.Message.Content[1..], new DiscordChannelMessageContext(notification.Message));
        }
    }
}