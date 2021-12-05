using CsvHelper;
using CsvHelper.Configuration;
using CSVParse.DataLayer;
using CSVParse.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace CSVParse.Classes
{
    public class CSVParser : IGenerateReport
    {
        /// <summary>
        /// Generates Unique Date/Time based ID for new file creation
        /// </summary>
        /// <returns></returns>
        public string GenerateUniqueID()
        {
            return DateTime.Now.ToLongDateString().Replace(" ", "_") + "_" + DateTime.Now.Ticks;
        }

        /// <summary>
        /// Reads amd Validates the CSV files in the folder.
        /// </summary>
        /// <param name="filePath">Full File Path</param>
        /// <param name="fileName">File Name</param>
        /// <param name="rowCount">Count of the records in the CSV file.</param>
        /// <returns></returns>
        public List<OutputCSVModel> Read(string filePath, string fileName, ref int rowCount)
        {
            string[] file = fileName.Split('.'); //split the file name to name and extension
            List<OutputCSVModel> lstOutputData = new List<OutputCSVModel>();
            rowCount = 0;

            if (file[1] == "csv")
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    NewLine = Environment.NewLine,
                    HasHeaderRecord = true
                };

                try
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        reader.ReadLine(); //to skip the first row in the csv file
                        using (CsvReader csv = new CsvReader(reader, config))
                        {
                            csv.Read();
                            csv.ReadHeader();
                            while (csv.Read())
                            {
                                rowCount++;
                                if (csv.Parser.Record.Length == csv.HeaderRecord.Length)
                                {
                                    OutputCSVModel output = new OutputCSVModel();
                                    output.ISIN = csv.GetField<string>("ISIN");
                                    output.CFICode = csv.GetField<string>("CFICode");
                                    output.Venue = csv.GetField<string>("Venue");
                                    output.ContractSize = GetContractSize(csv.GetField<string>("AlgoParams"));

                                    if (output.ContractSize == null)
                                    {
                                        Logging.LogError("The record in the file" + fileName + " with ISIN: " + output.ISIN + " does not contain ContractSize value.");
                                    }
                                    else
                                    {
                                        lstOutputData.Add(output);
                                    }
                                }
                                else
                                {
                                    Logging.LogError("Invalid CSV file: " + fileName + ". Header does not match record count.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex.Message);
                }
            }
            else
            {
                Logging.LogError("The file " + fileName + " is not a CSV file.");
            }

            return lstOutputData;
        }

        /// <summary>
        /// Checks the Algo Params Column and gets the Price Multiplier value
        /// </summary>
        /// <param name="algoParams">Cell Value containing Price Multiplier value</param>
        /// <returns></returns>
        float? GetContractSize(string algoParams)
        {
            string strStart = "PriceMultiplier:";
            string strEnd = "|;";
            if (!String.IsNullOrEmpty(algoParams) && algoParams.Contains(strStart))
            {
                try
                {
                    int start, end;
                    start = algoParams.IndexOf(strStart, 0) + strStart.Length;
                    end = algoParams.IndexOf(strEnd, start);
                    return float.Parse(algoParams.Substring(start, end - start));
                }
                catch (Exception ex)
                {
                    Logging.LogError("Error while parsing Contract Size value: " + ex.Message);
                }
            }

            return null;
        }

        /// <summary>
        /// Writes the read CSV to the Report folder.
        /// </summary>
        /// <param name="lstOutputCSVModel">Model containg the Header/Row Data</param>
        /// <param name="fileName">Input File Name to be used as a prefix to the Report file name</param>
        public void Write(List<OutputCSVModel> lstOutputCSVModel, string fileName)
        {
            try
            {
                if (lstOutputCSVModel.Count > 0)
                {
                    using (var writer = new StreamWriter(ConfigurationManager.AppSettings["ReportsFolderPath"] + fileName + "_" + GenerateUniqueID() + ".csv"))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(lstOutputCSVModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.Message);
            }
        }
    }
}
