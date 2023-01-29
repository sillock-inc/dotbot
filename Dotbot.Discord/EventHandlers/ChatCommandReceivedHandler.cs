using Discord.WebSocket;
using Dotbot.Common.Factories;
using Dotbot.Common.Services;
using Dotbot.Common.Settings;
using Dotbot.Discord.CommandHandlers;
using Dotbot.Discord.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dotbot.Discord.EventHandlers;

public class ChatCommandReceivedHandler : INotificationHandler<DiscordMessageReceivedNotification>
{
    private readonly ILogger _logger;
    private readonly IBotCommandHandlerFactory _commandHandlerFactory;
    private readonly IBotModeratorCommandHandlerFactory _moderatorCommandHandlerFactory;
    private readonly IChatServerService _chatServerService;
    private readonly BotSettings _botSettings;

    public ChatCommandReceivedHandler(IBotCommandHandlerFactory commandHandlerFactory,
        ILogger<ChatCommandReceivedHandler> logger, IOptions<BotSettings> botSettings,
        IBotModeratorCommandHandlerFactory moderatorCommandHandlerFactory, IChatServerService chatServerService)
    {
        _commandHandlerFactory = commandHandlerFactory;
        _logger = logger;
        _moderatorCommandHandlerFactory = moderatorCommandHandlerFactory;
        _chatServerService = chatServerService;
        _botSettings = botSettings.Value;
    }

    public async Task Handle(DiscordMessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("<{AuthorUsername}>: {Message}", notification.Message.Author.Username,
            notification.Message.Content);

        if (notification.Message.Channel is SocketGuildChannel sgc)
        {
            await _chatServerService.Create(sgc.Guild.Id.ToString());
        }

        var messageSplit = notification.Message.Content.Split(' ');

        if (messageSplit[0].StartsWith(_botSettings.CommandPrefix))
        {
            await _commandHandlerFactory.GetCommand(messageSplit[0][1..]).HandleAsync(notification.Message.Content[1..],
                new DiscordChannelMessageContext(notification.Message));
        }

        if (messageSplit[0].StartsWith(_botSettings.ModCommandPrefix))
        {
            //The permission check should probably be here or in the context
            await _moderatorCommandHandlerFactory.GetCommand(messageSplit[0][1..]).HandleAsync(
                notification.Message.Content[1..], new DiscordChannelMessageContext(notification.Message));
        }
    }
}