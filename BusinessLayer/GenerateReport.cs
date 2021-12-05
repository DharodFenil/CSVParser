using CSVParse.DataLayer;
using CSVParse.Model;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace CSVParse.BusinessLayer
{
    public class GenerateReport
    {
        private IGenerateReport report;

        public GenerateReport(IGenerateReport report)
        {
            this.report = report;
        }    
        
        /// <summary>
        /// Generates and Filters the files in the source folder to write the respective report files
        /// </summary>
        public void Generate()
        {
            string path = ConfigurationManager.AppSettings["SourceFolderPath"];
            int rowCount = 0;
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles())
            {
                string[] fileName = file.Name.Split('.'); //split the file name to name and extension
                List<OutputCSVModel> lstData = report.Read(file.FullName, file.Name, ref rowCount);
                report.Write(lstData, fileName[0]);
            }
        }
    }
}
