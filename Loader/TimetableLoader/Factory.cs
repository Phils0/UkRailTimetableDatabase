using CifExtractor;
using CifParser;
using Serilog;

namespace TimetableLoader
{
    public interface IFactory
    {
        IArchive GetArchive();
        IParser CreateParser();
        IDatabase GetDatabase();
        IFileLoader CreateStationLoader(IArchive archive, IDatabase db);
    }

    internal class Factory : IFactory
    {
        private readonly ILoaderConfig _config;
        private readonly ILogger _logger;
        private readonly IParserFactory _factory;
        private readonly IParserFactory _rdgFactory;

        internal Factory(ILoaderConfig config, IParserFactory cifParserFactory, IParserFactory stationParserFactory, ILogger logger)
        {
            _config = config;
            _logger = logger;
            _factory = cifParserFactory;
            _rdgFactory = stationParserFactory;
        }

        public IFileLoader CreateCifLoader()
        {
            return new CifLoader(this);
        }
        
        public IArchive GetArchive() => new Archive(_config.TimetableArchiveFile, _logger);

        public IParser CreateParser() => _factory.CreateParser();
        
        public IDatabase GetDatabase() => new Database(_config.ConnectionString, _logger);

        public IFileLoader CreateStationLoader(IArchive archive, IDatabase db)
        {
            var extractor = new RdgZipExtractor(archive, _logger);
            var ignoreLines = archive.IsDtdZip
                ? StationParserFactory.DtdIgnoreLines
                : StationParserFactory.TtisIgnoreLines;
            var parser = _rdgFactory.CreateParser(ignoreLines);
            var loader = db.CreateStationLoader();

            return new MasterStationFileLoader(extractor, parser, loader);
        }
    }
}
