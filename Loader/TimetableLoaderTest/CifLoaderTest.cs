using System.Collections.Generic;
using System.Data.SqlClient;
using CifParser;
using CifParser.Archives;
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
            var archive = Substitute.For<IArchive>();
            var loader = Substitute.For<IDatabaseLoader>();
            var db = Substitute.For<IDatabase>();
            db.CreateCifLoader().Returns(loader);
            
            CifFile.Load(archive, db);
            
            loader.Received().Load(Arg.Any<IEnumerable<IRecord>>());
        }
    }
}