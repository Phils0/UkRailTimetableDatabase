using CifParser.Archives;

namespace TimetableLoader
{
    internal class MasterStationFileLoader : IFileLoader
    {
        private IArchiveParser _parser;
        private IDatabaseLoader _loader;

        public MasterStationFileLoader(IArchiveParser parser, IDatabaseLoader loader)
        {
            _parser = parser;
            _loader = loader;
        }
        
        public void Run()
        {
            var records = _parser.ReadFile(RdgZipExtractor.StationExtension);
            _loader.Load(records);
        }
    }
}