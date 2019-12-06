namespace ScoopFramework.FileStream.Interface
{
    interface IFilesStream
    {
        object FileStreamDelete(string path);
        bool FileStreamSearch(string path);
    }
}
