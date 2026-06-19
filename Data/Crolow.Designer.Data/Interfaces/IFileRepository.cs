namespace Crolow.Designer.Data.Interfaces
{
    public interface IFileRepository
    {
        byte[] GetFile(Guid fileId);
        string UploadFile(Guid id, string filePath, Stream bytes, Dictionary<string, string> metadata);
    }
}