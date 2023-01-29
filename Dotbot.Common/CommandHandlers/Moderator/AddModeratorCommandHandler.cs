using Dotbot.Common.Services;
using FluentResults;
using static Dotbot.Common.Models.FormattedMessage;
using static FluentResults.Result;

namespace Dotbot.Common.CommandHandlers.Moderator;

public class AddModeratorCommandHandler: IBotModeratorCommandHandler
{
    public AddModeratorCommandHandler(IChatServerService chatServerService)
    {
        _chatServerService = chatServerService;
    }

    public ModeratorCommandType CommandType => ModeratorCommandType.AddModerator;

    private readonly IChatServerService _chatServerService;
    
    [Test]
    public async Task<Result> HandleAsync(string content, IServiceContext context)
    {
        var serverId = await context.GetServerId();
        var (isSuccess, isFailed, value) = await _chatServerService.IsModerator(serverId, (await context.GetAuthorId()).ToString());
        if (isFailed || (isSuccess && !value))
        {
            await context.SendEmbedAsync(ErrorMessage("Not authorised"));
            return Fail("User not authorised");
        }

        var mentions = await context.GetUserMentionsAsync();

        if (mentions.Count == 0)
        {
            await context.SendEmbedAsync(ErrorMessage("No users provided"));
            return Fail("No users provided"); 
        }
        
        foreach (var mention in mentions)
        {
            var result = await _chatServerService.AddModerator(serverId, mention.Id.ToString());
            if(result.IsFailed)
            {
                await context.SendEmbedAsync(ErrorMessage(result.Errors));
                return Fail(result.Reasons.ToString()); 
            }
        }

        await context.SendEmbedAsync(Success("Users added as moderators"));
        
        return Ok();
    }

    public class TestAttribute : Attribute
    {
        public TestAttribute()
        {
            
        }
    }
    
}