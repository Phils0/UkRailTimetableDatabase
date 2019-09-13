using CifParser;
using CifParser.Archives;
using Serilog;

namespace TimetableLoader
{
    public interface IFactory
    {
        IArchive GetArchive();
        IDatabase GetDatabase();
        IFileLoader CreateStationLoader(IArchive archive, IDatabase db);
    }

    internal class Factory : IFactory
    {
        private readonly ILoaderConfig _config;
        private readonly ILogger _logger;

        internal Factory(ILoaderConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public IFileLoader CreateCifLoader()
        {
            return new CifLoader(this);
        }
        
        public IArchive GetArchive() => new Archive(_config.TimetableArchiveFile, _logger);

        public IDatabase GetDatabase() => new SqlServer.Database(_config.ConnectionString, _logger);

        public IFileLoader CreateStationLoader(IArchive archive, IDatabase db)
        {
            var loader = db.CreateStationLoader();
            return new MasterStationFileLoader(archive.CreateParser(), loader);
        }
    }
}
