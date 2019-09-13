using CifParser.Archives;

namespace TimetableLoader
{
    internal static class MasterStationFile
    { 
        internal static void Load(IArchive archive, IDatabase db) 
        {
            if (archive.IsRdgZip)
            {
                var loader = db.CreateStationLoader();
                var parser = archive.CreateParser();
                var records = parser.ReadFile(RdgZipExtractor.StationExtension);
                loader.Load(records);
            }
        }
    }
}