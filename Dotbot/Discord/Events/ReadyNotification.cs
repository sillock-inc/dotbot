using MediatR;

namespace Dotbot.Events;


public class ReadyNotification : INotification
{
    public static readonly ReadyNotification Default
        = new();

    private ReadyNotification()
    {
    }
}