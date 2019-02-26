﻿using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
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
                    .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts, config))
                    .WithNotParsed<Options>((errs) => HandleParseError(errs));
            }
            finally
            {
                Log.CloseAndFlush();                
            }

        }

        private static void RunOptionsAndReturnExitCode(Options opts, IConfiguration config)
        {
            try
            {
                Log.Information("Configure Loader");
                var factory = new Factory(config, Log.Logger);
                var loader = factory.Create();
              
                Log.Information("Uncompress, Parse and Load timetable: {file}", opts.TimetableFile);
                loader.Run(opts);
                Log.Information("{file} loaded", opts.TimetableFile);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Processing failed for {file}", opts.TimetableFile);
                Debugger.Break();
                throw;
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
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] {Message:lj} {Exception} {Properties}{NewLine}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Configured logging");
        }
    }
}