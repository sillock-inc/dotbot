namespace Dotbot.Common.Services;

public interface IGridFsFileService
{
    Task<Stream?> GetFile(string fileName);
    Task SaveFile(string fileName, Stream fileStream);
}