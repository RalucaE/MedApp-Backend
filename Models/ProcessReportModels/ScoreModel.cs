using MedicalAPI.Models.ExtractPdfModels;

namespace MedicalAPI.Models.ProcessReportModels
{
    public class ScoreModel
    {
        public PatientInfo patientInfo { get; set; }
        public string reportId { get; set; }
    }
}
