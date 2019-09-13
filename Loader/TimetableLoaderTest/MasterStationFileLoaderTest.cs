using CifParser.Archives;
using NSubstitute;
using TimetableLoader;
using Xunit;

namespace TimetableLoaderTest
{
    public class MasterStationFileLoaderTest
    {
        private const string TestArchive = "Dummy.zip";
        
        [Fact]
        public void LoadsMasterStationFile()
        {
            var parser = Substitute.For<IArchiveParser>();
            var loader = new MasterStationFileLoader(parser, Substitute.For<IDatabaseLoader>());

            loader.Run(); 
            
            parser.Received().ReadFile(RdgZipExtractor.StationExtension);
        }
    }
}