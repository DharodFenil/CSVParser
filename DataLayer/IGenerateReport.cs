using CSVParse.Model;
using System.Collections.Generic;

namespace CSVParse.DataLayer
{
    public interface IGenerateReport
    {
        string GenerateUniqueID();
        List<OutputCSVModel> Read(string filePath, string fileName, ref int rowCount);
        void Write(List<OutputCSVModel> lstOutputCSVModel, string fileName);
    }
}
