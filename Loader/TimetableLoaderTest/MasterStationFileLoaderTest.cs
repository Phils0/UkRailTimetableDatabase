using System.Collections.Generic;
using CifParser;
using CifParser.Archives;
using NSubstitute;
using TimetableLoader;
using Xunit;

namespace TimetableLoaderTest
{
    public class MasterStationFileLoaderTest
    {
        [Fact]
        public void LoadsMasterStationFileWhenRdgZip()
        {
            var loader = Substitute.For<IDatabaseLoader>();
            var db = Substitute.For<IDatabase>();
            db.CreateStationLoader().Returns(loader);
            
            var archive = Substitute.For<IArchive>();
            archive.IsRdgZip.Returns(true);
            
            MasterStationFile.Load(archive, db);
            
            loader.Received().Load(Arg.Any<IEnumerable<IRecord>>());
        }
        
        [Fact]
        public void DoesNotLoadMasterStationFileWhenNrodArchive()
        {
            var loader = Substitute.For<IDatabaseLoader>();
            var db = Substitute.For<IDatabase>();
            db.CreateStationLoader().Returns(loader);
            
            var archive = Substitute.For<IArchive>();
            archive.IsRdgZip.Returns(false);
            
            MasterStationFile.Load(archive, db);
            
            loader.DidNotReceive().Load(Arg.Any<IEnumerable<IRecord>>());
        }
    }
}