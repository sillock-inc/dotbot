using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Dotbot.Common.Services;

public class GridFsFileService : IGridFsFileService
{
    private readonly IGridFSBucket _gridFsBucket;

    public GridFsFileService(IGridFSBucket gridFsBucket)
    {
        _gridFsBucket = gridFsBucket;
    }

    public async Task<Stream?> GetFile(string fileName)
    {
        var filter = Builders<GridFSFileInfo>.Filter.And(Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileName));
        var options = new GridFSFindOptions
        {
            Limit = 1,
        };

        using var cursor = await _gridFsBucket.FindAsync(filter, options);
        var fileInfo = (await cursor.ToListAsync()).FirstOrDefault();
        if (fileInfo == null)
        {
            return null;
        }
            
        return await _gridFsBucket.OpenDownloadStreamAsync(fileInfo.Id);;
    }

    public async Task SaveFile(string fileName, Stream fileStream)
    {
        await _gridFsBucket.UploadFromStreamAsync(fileName, fileStream);
    }
}