using MedicalAPI.Models.ProcessReportModels;

namespace MedicalAPI.Models
{
    public class PatientModel
    {
        public PatientInfo pacientInfo { get; set; }
        public object reportData { get; set; }
        public PatientModel(PatientInfo pacientInfo, object reportData)
        {
             this.pacientInfo = pacientInfo;
             this.reportData = reportData;
        }

        public double GetIMC()
        {
            this.pacientInfo.height = this.pacientInfo.height/100;
            return this.pacientInfo.weight / (this.pacientInfo.height * this.pacientInfo.height);
        }
        public List<string> AnemiaScore()
        {
            var anomalies = new List<string>();
            var hemoglobina = "";
            var vem = "";
            var nrEritrocite = "";
            var rdwCV = "";
            Console.WriteLine(reportData);
            //foreach (var report in this.reportData)
            //{
            //    var headers = report.Keys;
            //    ReportModel reportModel = new ReportModel((string)report["Test"], (string)report["Rezultat"], (string)report["UM"], (string)report["Interval de referinta"]);
                 
            //    if (reportModel.Test == "Hemoglobina")
            //    {
            //        hemoglobina = reportModel.Rezultat;
            //    }
            //    if (reportModel.Test == "Nr. eritrocite")
            //    {
            //        nrEritrocite = reportModel.Rezultat;
            //    }
            //    if (reportModel.Test == "VEM")
            //    {
            //        vem = reportModel.Rezultat;
            //    }
            //    if (reportModel.Test == "RDW-CV")
            //    {
            //        rdwCV = reportModel.Rezultat;
            //    }
            //}
            //if (double.TryParse(hemoglobina, out double hemoglobinaValue))
            //{
            //    if (hemoglobinaValue < 12)
            //        anomalies.Add("Anemia detected (low hemoglobin).");
            //}

            //if (double.TryParse(vem, out double vemValue))
            //{
            //    if (vemValue < 80)
            //        anomalies.Add("Microcytic anemia (possible iron deficiency).");
            //    if (vemValue > 110)
            //        anomalies.Add("Macrocytic anemia (possible B12 or folate deficiency).");
            //}

            //if (double.TryParse(rdwCV, out double rdwCVValue))
            //{
            //    if (rdwCVValue > 14.5)
            //        anomalies.Add("Iron deficiency anemia or mixed anemia.");
            //}

            //if (double.TryParse(nrEritrocite, out double nrEritrociteValue))
            //{
            //    if (nrEritrociteValue < 3.8)
            //        anomalies.Add("Anemia due to low red blood cell count.");
            //}
            //if(!(anomalies.Count > 0))
            //    anomalies.Add("No significant anemia detected.");
            //// Return all anomalies or a default message if no anomalies were found
            return anomalies;
        }
        //scor inflamație sistemică
        public List<string> NLRScore()
        {
            var anomalies = new List<string>();
            var nrNeutrofile = "";
            var nrLimfocite = "";
            //foreach (var report in this.reportData)
            //{
            //    ReportModel reportModel = new ReportModel((string)report["Test"], (string)report["Rezultat"], (string)report["UM"], (string)report["Interval de referinta"]);

            //    if (reportModel.Test.Contains("neutrofile"))
            //    {
            //        nrNeutrofile = reportModel.Rezultat;
            //    }
            //    if (reportModel.Test.Contains("limfocite"))
            //    {
            //        nrLimfocite = reportModel.Rezultat;
            //    }
            //}
            //if (double.TryParse(nrNeutrofile, out double nrNeutrofileValue) && double.TryParse(nrLimfocite, out double nrLimfociteValue))
            //{
            //    var NLR = nrNeutrofileValue / nrLimfociteValue;
            //    if (NLR < 2) anomalies.Add("Risc scăzut de inflamație sistemică.");
            //    if (NLR > 2 && NLR < 4) anomalies.Add("Inflamație moderată");
            //}
            //if (!(anomalies.Count > 0))
            //    anomalies.Add("Inflamație severă (posibilă infecție, sepsis sau boli cronice)");
            return anomalies;
        }
        public List<string> PLRScore()
        {
            var anomalies = new List<string>();
            var nrTrombocite = "";
            var nrLimfocite = "";
            //foreach (var report in this.reportData)
            //{
            //    ReportModel reportModel = new ReportModel((string)report["Test"], (string)report["Rezultat"], (string)report["UM"], (string)report["Interval de referinta"]);

            //    if (reportModel.Test.Contains("trombocite"))
            //    {
            //        nrTrombocite = reportModel.Rezultat;
            //    }
            //    if (reportModel.Test.Contains("limfocite"))
            //    {
            //        nrLimfocite = reportModel.Rezultat;
            //    }
            //}
            //if (double.TryParse(nrTrombocite, out double nrTrombociteValue) && double.TryParse(nrLimfocite, out double nrLimfociteValue))
            //{
            //    var PLR = nrTrombociteValue / nrLimfociteValue;
            //    if (PLR < 100) anomalies.Add("Inflamație redusă");
            //    if (PLR > 2 && PLR < 4) anomalies.Add("Inflamație moderată");
            //}
            //if (!(anomalies.Count > 0))
            //    anomalies.Add("Inflamație severă, asociată cu boli cardiovasculare sau cancere.");
            return anomalies;
        }
    }
}
