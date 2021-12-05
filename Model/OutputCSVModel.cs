using CsvHelper.Configuration.Attributes;

namespace CSVParse.Model
{
    public class OutputCSVModel
    {
        public string ISIN { get; set; }
        public string CFICode { get; set; }
        public string Venue { get; set; }

        [Name("Contract Size")]
        public float? ContractSize { get; set; }
    }
}
