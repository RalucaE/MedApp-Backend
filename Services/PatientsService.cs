using MedicalAPI.Models;
using MedicalAPI.Models.ProcessReportModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace MedicalAPI.Services
{
    public class PatientsService
    {    
        public List<List<string>> CompareReports(List<object> reports)
        {
        //    var h1 = 0.0;
        //    var h2 = 0.0;

        //    //foreach (var report in reports)
        //    //{
        //    //    ReportModel reportModel = new ReportModel((string)report["Test"], (string)report["Rezultat"], (string)report["UM"], (string)report["Interval de referinta"]);
        //    //    if (reportModel.Test.Contains("eritrocite"))
        //    //    {                  
        //    //        //h1 = reportModel.Rezultat;
        //    //    }
        //    //}
        //    //foreach (var report in report2)
        //    //{
        //    //    var t = report["Test"];
        //    //    ReportModel reportModel = new ReportModel(
        //    //        (string)report["Test"],
        //    //        (string)report["Rezultat"],
        //    //        (string)report["UM"],
        //    //        (string)report["Interval de referinta"]);
        //    //    if (reportModel.Test.Contains("eritrocite"))
        //    //    {
        //    //        //h2 = reportModel.Rezultat;
        //    //    }
        //        //var x = t.GetType();
        //        //Console.WriteLine(t.GetType());
        //        //if((string)t.Contains("Hemoglobina"))
        //        //{
        //        //    h2 = 4;
        //        //}
        //        //var r = report["Rezultat"];
        //        //if ((string)report["Test"].Contains("Hemoglobina"))
        //        //{
        //        //    h2 = (double)report["Rezultat"];
        //        //}
        //    }
            var values = new List<List<string>>
            {
                new List<string> { "h1", "h1" },
                new List<string> { "h2", "h2" },
            };
            return values;

        }
    }
}

