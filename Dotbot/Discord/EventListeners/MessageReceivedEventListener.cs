using Discord.WebSocket;
using Dotbot.Discord.Events;
using MediatR;

namespace Dotbot.Discord.EventListeners;

public class MessageReceivedEventListener
{
    private readonly IMediator _mediator;

    public MessageReceivedEventListener(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task OnMessageReceivedAsync(SocketMessage arg)
    {
        if(arg.Author.IsBot) return;

        /*if (arg.Content.Contains(">play"))
        {
            var url = arg.Content.Split(" ")[1];
            var channel = arg.Channel as SocketGuildChannel;
            var user = arg.Author as SocketGuildUser;
            
            await _audioService.JoinAudio(channel.Guild, user.VoiceChannel ?? throw new InvalidOperationException());
            await _audioService.SendAudioAsync( channel.Guild,
                user.VoiceChannel ?? throw new InvalidOperationException(), url);
        }
        */
        
        if(arg.Channel.Id == 686698171350515792)
            await _mediator.Publish(new DiscordMessageReceivedNotification(arg));
        return ;
    }
}