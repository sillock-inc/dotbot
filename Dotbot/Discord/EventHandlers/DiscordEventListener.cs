using Discord.Interactions.Builders;
using Discord.Net;
using Discord.WebSocket;
using Dotbot.Discord.Services;
using Dotbot.Events;
using MediatR;
using Newtonsoft.Json;

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
           await _mediator.Publish(new DiscordMessageReceivedNotification(arg), _cancellationToken);
        return ;
    }
    
    private async Task OnReadyAsync()
    {
      /*  var guild = _client.GetGuild(301062316647120896);

        // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
        var guildCommand = new SlashCommandBuilder();

        // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
        guildCommand.WithName("first-command");

        // Descriptions can have a max length of 100.
        guildCommand.WithDescription("This is my first guild slash command!");

        // Let's do our global command
        var globalCommand = new SlashCommandBuilder();
        globalCommand.WithName("first-global-command");
        globalCommand.WithDescription("This is my first global slash command");

        try
        {
            // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
            await guild.CreateApplicationCommandAsync(guildCommand.Build());

            // With global commands we don't need the guild.
            await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
            // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
        }
        catch(ApplicationCommandException exception)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            Console.WriteLine(json);
        } 
        */
        await _mediator.Publish(ReadyNotification.Default, _cancellationToken);
    }
}