using Discord;
using Discord.Commands;
using Discord.Interactions;
using Dotbot.Discord.Services;
using RunMode = Discord.Interactions.RunMode;

namespace Dotbot.Discord.CommandHandlers;

public class PlayAudioCommandHandler : ModuleBase<ICommandContext>
{
    private readonly IAudioService _audioService;

    public PlayAudioCommandHandler(IAudioService audioService)
    {
        _audioService = audioService;
    }

    [SlashCommand("join", "join voice chat",false, RunMode.Async)]
    public async Task JoinCmd()
    {
        await _audioService.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
    }
    
    [SlashCommand("leave", "leave voice chat",false, RunMode.Async)]
    public async Task LeaveCmd()
    {
        await _audioService.LeaveAudio(Context.Guild);
    }
    
    [SlashCommand("play", "play moosic",false, RunMode.Async)]
    public async Task PlayCmd([Remainder] string song)
    {
        await _audioService.SendAudioAsync(Context.Guild, Context.Channel, song);
    }
    
   /* [Command("join", RunMode = RunMode.Async)] 
    public async Task JoinChannel(string url)
    {
        await _audioService.JoinAudio(Context.Guild, (Context.User as IVoiceState)?.VoiceChannel ?? throw new InvalidOperationException());
        await _audioService.SendAudioAsync(Context.Guild,
            (Context.User as IVoiceState)?.VoiceChannel ?? throw new InvalidOperationException(), url);
    } */  
}