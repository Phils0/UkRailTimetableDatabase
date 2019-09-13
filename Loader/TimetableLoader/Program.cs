using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CifParser;
using CifParser.Archives;
using CommandLine;

namespace TimetableLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureLogging();

            try
            {
                var config = ConfigureApp();

                CommandLine.Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(opts => Run(opts, config))
                    .WithNotParsed<Options>(HandleParseError);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void Run(Options opts, IConfiguration configuration)
        {
            try
            {
                var loaderConfig = new LoaderConfig(configuration, opts);
                Log.Information("Loader Configuration: {config}", loaderConfig);
                Load(loaderConfig);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Processing failed for {file}", opts.TimetableArchiveFile);
                throw;
            }

            void Load(LoaderConfig config)
            {
                var archive = new Archive(config.TimetableArchiveFile, Log.Logger);
                
                Log.Information("Loading timetable: {file}", config.TimetableArchiveFile);
                using (var db = new SqlServer.Database(config.ConnectionString, Log.Logger))
                {
                    db.OpenConnection();
                
                    CifFile.Load(archive, db);
                    MasterStationFile.Load(archive, db);
                }
                Log.Information("{file} loaded", config.TimetableArchiveFile);
            }
        }
        
        private static IConfiguration ConfigureApp()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var error in errs)
            {
                Log.Error(error.ToString());
            }
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .Destructure.ByTransforming<CifParser.Records.Tiploc>(
                    r => new {r.Code, r.Action})
                .WriteTo.Console()
                .WriteTo.File(@"TimetableLoader-.log",
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] {Message:lj} {Exception} {Properties:j}{NewLine}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Configured logging");
        }
    }
}