using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using CifParser.Records;
using Serilog;

namespace TimetableLoader
{
    internal abstract class RecordLoaderBase
    {
        private SqlConnection _connection;
        private Sequence _sequence;
        protected ILogger _logger;

        protected abstract string TableName { get;}
        
        internal RecordLoaderBase(SqlConnection connection, Sequence sequence, ILogger logger)
        {
            _connection = connection;
            _sequence = sequence;
            _logger = logger;
        }

        internal DataTable Table { get; private set; }

        /// <summary>
        /// Create the DataTable to load the records into
        /// </summary>
        internal void CreateDataTable()
        {
            var table = new DataTable();

            // read the table structure from the database
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $"SELECT TOP 0 * FROM {TableName}";
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                };
            }

            Table = table;
        }
        
        /// <summary>
        /// Load the DataTable into the database
        /// </summary>
        /// <param name="transaction"></param>
        public void Load(SqlTransaction transaction)
        {
            using (var bulk = new SqlBulkCopy(_connection, SqlBulkCopyOptions.KeepIdentity, transaction))
            {
                try
                {
                    bulk.DestinationTableName = TableName;
                    bulk.WriteToServer(Table);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex, "Error loading schedules");
                    throw;
                }
            }
        }

        protected long SetNewId()
        {
            var newId = _sequence.GetNext();
            return newId;
        }

        protected object SetNullIfEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? (object) DBNull.Value : value;
        }
    }
}