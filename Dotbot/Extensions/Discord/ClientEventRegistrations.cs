using Discord.WebSocket;
using Dotbot.Discord.EventListeners;

namespace Dotbot.Extensions.Discord;

public static class ClientEventRegistrations
{
    public static DiscordSocketClient RegisterClientEvents(IServiceProvider serviceProvider)
    {
        var client = serviceProvider.GetRequiredService<DiscordSocketClient>();
        var messageReceivedEventListener = serviceProvider.GetRequiredService<MessageReceivedEventListener>();

        client.MessageReceived += messageReceivedEventListener.OnMessageReceivedAsync;
        //Other client event registrations go here
        return client;
    }
}