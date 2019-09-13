using CifParser;
using System.IO;
using System.Linq;
using CifParser.RdgRecords;
using NSubstitute;
using Serilog;

namespace TimetableLoaderTest
{
    internal static class ParserHelper
    {
        public static IRecord[] ParseRecords(string data)
        {
            var input = new StringReader(data);

            var factory = new ConsolidatorFactory(Substitute.For<ILogger>());
            var parser = factory.CreateParser();
            var records = parser.Read(input).ToArray();
            return records;
        }
        
        public static Station[] ParseStationRecords(string data)
        {
            var input = new StringReader(data);

            var factory = new StationParserFactory(Substitute.For<ILogger>());
            var parser = factory.CreateParser(0);
            var records = parser.Read(input).Cast<Station>().ToArray();
            return records;
        }
    }
}
