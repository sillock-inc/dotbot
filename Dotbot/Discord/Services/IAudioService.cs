using Discord;

namespace Dotbot.Discord.Services;

public interface IAudioService
{
    Task JoinAudio(IGuild guild, IVoiceChannel target);
    Task LeaveAudio(IGuild guild);
    Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path);
}