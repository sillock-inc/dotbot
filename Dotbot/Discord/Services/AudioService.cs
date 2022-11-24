using System.Collections.Concurrent;
using System.Diagnostics;
using CliWrap;
using Discord;
using Discord.Audio;
using Discord.Commands;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Dotbot.Discord.Services;

public class AudioService : IAudioService
{
    private readonly ConcurrentDictionary<ulong, IAudioClient> _connectedChannels = new();
    
    public async Task JoinAudio(IGuild guild, IVoiceChannel target)
    {
        IAudioClient client;
        if (_connectedChannels.TryGetValue(guild.Id, out client))
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
        IAudioClient client;
        if (_connectedChannels.TryRemove(guild.Id, out client))
        {
            await client.StopAsync();
            //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
        }
    }
    
    public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string url)
    {
        IAudioClient client;

        int offsetInSeconds = 0;
        if (url.Contains("?t")) offsetInSeconds = int.Parse(url.Split("?t=")[1]);
        if (_connectedChannels.TryGetValue(guild.Id, out client))
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
            
            var video =  await youtube.Videos.GetAsync(url);
            
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                var offsetPercentage = offsetInSeconds / video.Duration.Value.TotalSeconds;
                //(int)(memoryStream.ToArray().Length*offsetPercentage)
                try {await discord.WriteAsync(memoryStream.ToArray(), 0, (int)memoryStream.Length); }
                finally { await discord.FlushAsync(); }
            }
        }
    }
}