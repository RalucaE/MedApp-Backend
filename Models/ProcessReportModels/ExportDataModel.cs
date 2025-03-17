namespace MedicalAPI.Models.ProcessReportModels
{
    //public class ExportDataModel
    //{
    //    public string Title { get; set; }
    //    public string Content { get; set; }
    //    public List<string> Tables { get; set; }
    //}
    public class ExportDataModel
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public List<ReportModel> ExtractedData { get; set; }
        public string Timestamp { get; set; }
    }

}
