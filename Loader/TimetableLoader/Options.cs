using System.Collections.Generic;
using CommandLine;

namespace TimetableLoader
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Timetable archive file")]
        public string TimetableArchiveFile { get; set; }
        
        [Option('d', "database", Required = false, Default = "", HelpText = "Database to connect to")]
        public string Database { get; set; }
    }
}