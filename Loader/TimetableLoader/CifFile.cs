using System;
using CifParser.Archives;

namespace TimetableLoader
{
    internal static class CifFile
    {
        internal static void Load(IArchive archive, IDatabase db)
        {
            var loader = db.CreateCifLoader();
            var parser = archive.CreateCifParser();
            var records = parser.Read();
            loader.Load(records);
        }
    }
}