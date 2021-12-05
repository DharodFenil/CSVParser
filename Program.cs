using CSVParse.BusinessLayer;
using CSVParse.Classes;
using CSVParse.DataLayer;
using System;

namespace CSVParse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IGenerateReport report = new CSVParser();
                GenerateReport generate = new GenerateReport(report);
                generate.Generate();
            }
            catch(Exception ex)
            {
                Logging.LogError(ex.StackTrace);
            }
        }
    }
}
