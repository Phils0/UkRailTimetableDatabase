using System.Data.SqlClient;
using CifExtractor;
using CifParser;

namespace TimetableLoader
{
    public interface IFileLoader
    {
        void Run();
    }

    internal class CifLoader : IFileLoader
    {
        private IFactory _factory;

        internal CifLoader(IFactory factory)
        {
            _factory = factory;
        }

        public void Run()
        {
            using (var db = _factory.GetDatabase())
            {
                db.OpenConnection();

                var archive = _factory.GetArchive();
                LoadCif(archive, db);
                if (archive.IsRdgZip)
                {
                    var stationLoader = _factory.CreateStationLoader(archive, db);
                    stationLoader.Run();
                }
            }
        }

        private void LoadCif(IArchive archive, IDatabase db)
        {
            var reader = archive.CreateExtractor().ExtractCif();
            var records = _factory.CreateParser().Read(reader);
            var loader = db.CreateCifLoader();
            loader.Load(records);
        }
    }
}