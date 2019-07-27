using CifExtractor;
using CifParser;
using NSubstitute;
using Serilog;
using TimetableLoader;
using Xunit;

namespace TimetableLoaderTest
{
    public class FactoryTest
    {
        [Fact]
        public void ConstructTtisParser()
        {
            var stationParserFactory = Substitute.For<IParserFactory>();
            var factory = Create(stationParserFactory);
            
            var archive = Substitute.For<IArchive>();
            archive.IsDtdZip.Returns(false);
            
            var loader = factory.CreateStationLoader(archive, Substitute.For<IDatabase>());

            stationParserFactory.Received().CreateParser(1);
        }

        private Factory Create(IParserFactory stationParserFactory)
        {
            var factory = new Factory(Substitute.For<ILoaderConfig>(),
                Substitute.For<IParserFactory>(),
                stationParserFactory,
                Substitute.For<ILogger>());
            return factory;
        }

        [Fact]
        public void ConstructDtdParser()
        {
            var stationParserFactory = Substitute.For<IParserFactory>();
            var factory = Create(stationParserFactory);
            
            var archive = Substitute.For<IArchive>();
            archive.IsDtdZip.Returns(true);
            
            var loader = factory.CreateStationLoader(archive, Substitute.For<IDatabase>());

            stationParserFactory.Received().CreateParser(6);
        }
    }
}