using Discord;
using Discord.Commands;
using Discord.Interactions;
using Dotbot.Discord.Services;
using RunMode = Discord.Commands.RunMode;

namespace Dotbot.Discord.CommandHandlers;

public class PlayAudioCommandHandler : ModuleBase<ICommandContext>
{
    private readonly IAudioService _audioService;

    public PlayAudioCommandHandler(IAudioService audioService)
    {
        _audioService = audioService;
    }

    [SlashCommand("youtoob", "Play audio")]
    public async Task Play(string url)
    {
        await JoinChannel(url);
    }
    
    [Command("join", RunMode = RunMode.Async)] 
    public async Task JoinChannel(string url)
    {
        await _audioService.JoinAudio(Context.Guild, (Context.User as IVoiceState)?.VoiceChannel ?? throw new InvalidOperationException());
        await _audioService.SendAudioAsync(Context.Guild,
            (Context.User as IVoiceState)?.VoiceChannel ?? throw new InvalidOperationException(), url);
    }   
}