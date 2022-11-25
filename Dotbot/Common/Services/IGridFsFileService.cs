using FluentResults;

namespace Dotbot.Common.Services;

public interface IGridFsFileService
{
    Task<Result<Stream>> GetFile(string fileName);
    Task<Result> SaveFile(string fileName, Stream fileStream);
}