using System.Collections.Concurrent;
using System.Diagnostics;
using CliWrap;
using Discord;
using Discord.Audio;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Dotbot.Discord.Services;

public class AudioService : IAudioService
{
    private readonly ConcurrentDictionary<ulong, IAudioClient> _connectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

    private IAudioClient _client;
    public async Task JoinAudio(IGuild guild, IVoiceChannel target)
    {
        
        if (_connectedChannels.TryGetValue(guild.Id, out _client))
        {
            return;
        }
        if (target.Guild.Id != guild.Id)
        {
            return;
        }

        var audioClient = await target.ConnectAsync();

        if (_connectedChannels.TryAdd(guild.Id, audioClient))
        {
            // If you add a method to log happenings from this service,
            // you can uncomment these commented lines to make use of that.
            //await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
        }
    }

    public async Task LeaveAudio(IGuild guild)
    {
        if (_connectedChannels.TryRemove(guild.Id, out _client))
        {
            await _client.StopAsync();
            //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
        }
    }
    
    public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string url)
    {
        // Your task: Get a full path to the file if the value of 'path' is only a filename.
        /*
        if (!File.Exists(path))
        {
            await channel.SendMessageAsync("File does not exist.");
            return;
        }*/
        if (_connectedChannels.TryGetValue(guild.Id, out _client))
        {
            
            var youtube = new YoutubeClient();

// You can specify either video ID or URL
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(
                url);
            var streamInfo = streamManifest.GetAudioOnlyStreams().TryGetWithHighestBitrate();
            var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
            var memoryStream = new MemoryStream();
            await Cli.Wrap("ffmpeg")
                .WithArguments(" -hide_banner -loglevel panic -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1")
                .WithStandardInputPipe(PipeSource.FromStream(stream))
                .WithStandardOutputPipe(PipeTarget.ToStream(memoryStream))
                .ExecuteAsync();
            
            
            using (var discord = _client.CreatePCMStream(AudioApplication.Mixed))
            {
                try {await discord.WriteAsync(memoryStream.ToArray(), 0, (int)memoryStream.Length); }
                finally { await discord.FlushAsync(); }
            }
        }
    }

    private Process CreateProcess(string path)
    {
        return Process.Start(new ProcessStartInfo
        {
            FileName = "ffmpeg.exe",
            Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
            UseShellExecute = false,
            RedirectStandardOutput = true
        });
    }
}