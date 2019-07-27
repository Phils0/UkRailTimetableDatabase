﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CifParser;
using CifParser.Records;
using Serilog;

namespace TimetableLoader.SqlServer
{
    public interface IDatabaseIdLookup
    {
        /// <summary>
        /// Lookup a database Id based upon a unique code
        /// </summary>
        /// <param name="code">Unique code</param>
        /// <returns>Id</returns>
        long Find(string code);
    }
    
    /// <summary>
    /// Bulk load Locations TI, TA, TD
    /// </summary>
    /// <remarks> This has a built in assumption that loading a Full CIF file and so there are only TiplocInsert records (as it expccts Tiploc code to be unique)
    /// Usage:
    /// <list type="number">
    /// <item>
    /// <description>Construct the object.</description>
    /// </item>
    /// <item>
    /// <description>CreateDataTable<see cref="CreateDataTable" /></description>
    /// </item>
    /// <item>
    /// <description>Add<see cref="Add(IRecord)" /> the records</description>
    /// </item>
    /// <item>
    /// <description>Load<see cref="Load"/> to upload the records to the database</description>
    /// </item>
    /// </list>
    /// </remarks>
    internal class LocationLoader : RecordLoaderBase, IRecordLoader, IDatabaseIdLookup
    {
        protected override string TableName => "Locations";
        
        /// <summary>
        /// Provides the lookup from Tiploc to database Id
        /// </summary>
        internal IDictionary<string, long> Lookup { get; } = new Dictionary<string, long>();

        internal LocationLoader(SqlConnection connection, Sequence sequence, ILogger logger) :
            base(connection, sequence, logger)
        {
        }

        /// <summary>
        /// Add a record to the DataTable
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Success</returns>
        public bool Add(IRecord record)
        {
            switch (record)
            {
                case TiplocInsertAmend insertAmend:
                    Add(insertAmend);
                    return true;
                case TiplocDelete delete:
                    Add(delete);
                    return true;
                default:
                    return false;
            }
        }

        private long Add(TiplocInsertAmend record)
        {
            var pair = CreateInsert(record);
            var row = pair.row;
            row["Nlc"] = record.Nalco;
            row["NlcCheckCharacter"] = record.NalcoCheckCharacter ;
            row["NlcDescription"] = SetNullIfEmpty(record.NlcDescription);
            row["Stanox"] = record.Stanox;
            row["ThreeLetterCode"] = SetNullIfEmpty(record.ThreeLetterCode);
            return pair.id;
        }

        private (DataRow row, long id) CreateInsert(TiplocInsertAmend record)
        {
            var databaseId = SetNewId(record.Code);
            var row = Table.NewRow();
            row["Id"] = databaseId;
            row["Action"] = record.Action == RecordAction.Create ? "I" : "U";
            row["Tiploc"] = record.Code;
            row["Description"] = record.Description;
            Table.Rows.Add(row);
            return (row, databaseId);
        }  
        
        private long SetNewId(string tiploc)
        {
            var newId = GetNewId();
            Lookup.Add(tiploc, newId);
            return newId;
        }  

        private void Add(TiplocDelete record)
        {
            var row = Table.NewRow();
            row["Id"] = SetNewId(record.Code);
            row["Action"] = "D";
            row["Tiploc"] = record.Code;
            Table.Rows.Add(row);
        }
       
        public long Find(string tiploc)
        {
            if (Lookup.TryGetValue(tiploc, out var id))
                return id;
            
            Serilog.Log.Warning("Adding missing location {tiploc}", tiploc);
            var record = new TiplocInsertAmend()
            {
                Action = RecordAction.Create,
                Code = tiploc,
                Description = $"{tiploc} - MISSING"
            };
            id = CreateInsert(record).id;
            return id;
        }
    }
}
