using CifExtractor;

namespace TimetableLoader
{
    public interface ILoaderConfig
    {
        string TimetableArchiveFile { get; }
        string ConnectionString { get; }
    }
}