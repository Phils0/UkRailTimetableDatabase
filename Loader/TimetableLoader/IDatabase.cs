using System;

namespace TimetableLoader
{
    public interface IDatabase : IDisposable
    {
        void OpenConnection();
        IDatabaseLoader CreateCifLoader();
        IDatabaseLoader CreateStationLoader();
    }
}