using Discord.WebSocket;
using Dotbot.Discord.Services;
using Dotbot.Events;
using MediatR;

namespace Dotbot.EventHandlers;

public class DiscordEventListener
{
    private readonly DiscordSocketClient _client;
    private readonly IMediator _mediator;
    private readonly CancellationToken _cancellationToken;
    private readonly IAudioService _audioService;
    
    public DiscordEventListener(DiscordSocketClient client, IMediator mediator, IAudioService audioService)
    {
        _client = client;
        _mediator = mediator;
        _audioService = audioService;
        _cancellationToken = new CancellationTokenSource().Token;
    }

    public Task StartAsync()
    {
        _client.Ready += OnReadyAsync;
        _client.MessageReceived += OnMessageReceivedAsync;

        return Task.CompletedTask;
    }

    private async Task OnMessageReceivedAsync(SocketMessage arg)
    {
        if(arg.Author.IsBot) return;
        
        if(arg.Channel.Id == 686698171350515792)
           await _mediator.Publish(new DiscordMessageReceivedNotification(arg), _cancellationToken);
        return ;
    }
    
    private Task OnReadyAsync()
    {
        return _mediator.Publish(ReadyNotification.Default, _cancellationToken);
    }
}