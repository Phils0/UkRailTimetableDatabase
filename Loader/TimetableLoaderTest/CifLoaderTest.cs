using System.Data.SqlClient;
using CifExtractor;
using CifParser;
using NSubstitute;
using TimetableLoader;
using Xunit;

namespace TimetableLoaderTest
{
    public class CifLoaderTest
    {
        [Fact]
        public void LoadsCifFile()
        {
            var extractor = Substitute.For<IExtractor>();
            var archive = Substitute.For<IArchive>();
            archive.CreateExtractor().Returns(extractor);
            
            var factory = CreateStubFactory();
            factory.GetArchive().Returns(archive);
            
            var loader = new CifLoader(factory);

            loader.Run();
            
            extractor.Received().ExtractCif();
        }
        
        private static IFactory CreateStubFactory()
        {
            var factory = Substitute.For<IFactory>();

            var parser = Substitute.For<IParser>();
            factory.CreateParser().Returns(parser);
            
            var db = Substitute.For<IDatabase>();
            factory.GetDatabase().Returns(db);
            
            return factory;
        }
        
        [Fact]
        public void LoadsMasterStationFileWhenRdgZip()
        {
            var stationsLoader = Substitute.For<IFileLoader>();
            
            var archive = Substitute.For<IArchive>();
            archive.IsRdgZip.Returns(true);

            var factory = CreateStubFactory();
            factory.GetArchive().Returns(archive);
            factory.CreateStationLoader(Arg.Any<IArchive>(),Arg.Any<IDatabase>()).Returns(stationsLoader);
            
            var loader = new CifLoader(factory);
            
            loader.Run();
            
            stationsLoader.Received().Run();
        }
        
        [Fact]
        public void DoesNotLoadMasterStationFileWhenNrodArchive()
        {
            var stationsLoader = Substitute.For<IFileLoader>();
            
            var archive = Substitute.For<IArchive>();
            archive.IsRdgZip.Returns(false);

            var factory = CreateStubFactory();
            factory.GetArchive().Returns(archive);
            factory.CreateStationLoader(Arg.Any<IArchive>(),Arg.Any<IDatabase>()).Returns(stationsLoader);
            
            var loader = new CifLoader(factory);
            
            loader.Run();
            
            stationsLoader.DidNotReceive().Run();

        }
    }
}