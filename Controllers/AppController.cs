using MedicalAPI.Models;
using MedicalAPI.Models.AuthenticationModels;
using MedicalAPI.Models.ExtractPdfModels;
using MedicalAPI.Repositories;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MedicalAPI.Utils;
using MedicalAPI.Models.ProcessReportModels;
using Azure;
using Nest;
using MedicalAPI.Models.ElasticSearchModels;
using Newtonsoft.Json;
using System.Text.Json;

namespace MedicalAPI.Controllers
{
    public class AppController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly UsersService _authenticationService;
        private readonly IReportsRepository<Report> _reportsRepository;
        private readonly PatientsService _patientsService;
        private readonly ElasticSearchService _elasticSearchService;
        public AppController(
            IUsersRepository usersRepository,
            UsersService authenticationService,            
            IReportsRepository<Report> reportsRepository,
            PatientsService patientsService,
            ElasticSearchService elasticSearchService
        )
        {
            _usersRepository = usersRepository;
            _authenticationService = authenticationService;
            _reportsRepository = reportsRepository;
            _patientsService = patientsService;
            _elasticSearchService = elasticSearchService;
        }

        #region Authentication
        [HttpGet("/users")]
        public IActionResult GetUsers()
        {
            var users = _usersRepository.GetMany();
            if (users != null)
            {
                var usersJson = users.Select(user => user.toJson()).ToList();
                return Ok(usersJson);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("/register")]
        public IActionResult Register([FromBody] RegisterRequest registerRequest)
        {
            var succes = _authenticationService.Register(registerRequest);
            if (!succes)
                return BadRequest(new { message = "Something went wrong" });
            else
                return Ok(new { message = "User registered" });
        }
        [HttpPost("/login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var loginResponse = _authenticationService.Login(loginRequest);
            if (loginResponse?.Token == null)
            {
                return BadRequest(loginResponse);
            }
            return Ok(loginResponse);
        }
        [HttpPut("/editUser")]
        public IActionResult EditUser([FromBody] User user)
        {
            var succes = _usersRepository.Update(user);
            if (!succes)
                return BadRequest(new { message = "Something went wrong" });
            else
                return Ok(new { message = "User edited" });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("/deleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var succes = _usersRepository.Delete(id);
            if (!succes)
                return BadRequest(new { message = "Something went wrong" });
            else
                return Ok(new { message = "User added" });
        }
        #endregion

        [Authorize]
        [HttpPost("/addReport")]
        public async Task<IActionResult> AddReport([FromForm] string report, [FromForm] IFormFile file)
        {
            if (string.IsNullOrEmpty(report) == null || file == null)
            {
                return BadRequest(new { message = "Report data or file is missing" });
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "reports", file.FileName);
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest(new { message = "Failed to save the file" });
            }
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            try
            {
                var result = ExtractPdfData.RunPythonScript(filePath);

                // Save extracted data to in ElasticSearch
                var documentId = Guid.NewGuid().ToString();
                //Prepare the document to index in Elasticsearch
                var document = new
                {
                    Id = documentId,
                    FilePath = filePath,
                    ExtractedData = result,
                    Timestamp = DateTime.UtcNow
                };

                // Index the document in Elasticsearch
                if (_elasticSearchService.Client == null)
                {
                    throw new InvalidOperationException("Elasticsearch client is not initialized.");
                }
                var indexResponse = _elasticSearchService.Client.Index(document, i => i
                    .Index("pdf_data")  // Use the default index or specify another one
                    .Id(documentId)     // Explicitly set the document ID
                );

                if (!indexResponse.IsValid)
                {
                    return StatusCode(500, "Failed to index document in Elasticsearch: " + indexResponse.ServerError.Error.Reason);
                }

               


                //Add report to database             

                // Store the report in the database, including the file path
                var reportObject = JsonConvert.DeserializeObject<Report>(report);
                reportObject.FilePath = filePath;
                reportObject.ReportType = documentId;
                var succes = _reportsRepository.Create(reportObject);
                if (!succes)
                    return BadRequest(new { message = "Something went wrong" });
                else
                {
                    return Ok(new { message = "Report added" });
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); // Or use a proper logger
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }
        [Authorize]
        [HttpGet("/reports")]
        public IActionResult GetReports()
        {
            var reports = _reportsRepository.GetMany();
            if (reports != null )
            {         
                return Ok(reports.ToList());
            }
            else
            {
                return NotFound();
            }
        }
        [Authorize]
        [HttpGet("/getReport/{id}")]
        public IActionResult GetReport(int id)
        {
            var report = _reportsRepository.Get(id);
            if (report != null)
                return Ok(report);
            else
                return NotFound();

        }
        [Authorize]
        [HttpDelete("/deleteReport/{id}")]
        public IActionResult DeleteReport(int id)
        {

            var report = _reportsRepository.Get(id);
            if (report == null)
            {
                return NotFound(new { message = "Report not found" });
            }
            if (!string.IsNullOrEmpty(report.FilePath) && System.IO.File.Exists(report.FilePath))
            {
                System.IO.File.Delete(report.FilePath);
            }

            var succes = _reportsRepository.Delete(id);
            if (!succes)
                return BadRequest(new { message = "Something went wrong" });
            else
                return Ok(new { message = "Report deleted" });
        }
        [Authorize]
        [HttpGet("/getReportsByUserId/{id}")]
        public IActionResult GetReportsByUserId(int id)
        {
            var reports = _reportsRepository.GetReportsByUserId(id);
            if (reports != null)
            {
                return Ok(reports.ToList());
            }
            else
            {
                return NotFound();
            }
        }
        [Authorize]
        [HttpGet("/getReportData/{documentId}")]
        public IActionResult getDataFromElastic(string documentId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(documentId))
                {
                    return BadRequest(new { Message = "Document ID is required." });
                }

                // Retrieve the document from Elasticsearch
                var response = _elasticSearchService.Client.Get<object>(documentId, g => g.Index("pdf_data"));

                if (!response.Found)
                {
                    return NotFound(new { Message = "Document not found", DocumentId = documentId });
                }
               
               
                // Return the document data
                return Ok(new
                {
                    Data = response.Source
                });
            }
            catch (Exception ex)
            {
                // Handle any errors
                return StatusCode(500, new { Message = "An error occurred while retrieving the document", Error = ex.Message });
            }
        }
        
        [HttpPost("/calculateScores")]
        public IActionResult EvaluateAnemiaRisk([FromBody] ScoreModel scoreModel)
        {
            if (scoreModel == null)
            {
                return BadRequest("Invalid patient data.");
            }
            string reportId = scoreModel.reportId;
            //var reportData = ExtractPdfData.RunPythonScript(reportId);
            var reportData = _elasticSearchService.Client.Get<object>(reportId, g => g.Index("pdf_data"));
           // var serDoc = JsonConvert.SerializeObject(reportData.Source);

            var report = JsonConvert.DeserializeObject<ExportDataModel>(JsonConvert.SerializeObject(reportData.Source));
            // Call the function to evaluate anemia risk
            PatientModel patientModel = new PatientModel(
                scoreModel.patientInfo,
                report.ExtractedData
            );
            //double IMC = patientModel.CalculeazaIMC();
            //List<string> anemiaScore = patientModel.AnemiaScore();
            //List<string> nlrScore = patientModel.NLRScore();
            //List<string> plrScore = patientModel.PLRScore();
            // Return the risk as the response
            //var scores = new List<object>
            //{
            //    new { Title = "AnemiaScore", Anomalies = anemiaScore },
            //    new { Title = "NLRScore", Anomalies = nlrScore },
            //    new { Title = "PLRScore", Anomalies = plrScore },
            //};
            
           
            return Ok(report.ExtractedData);
        }

        [HttpPost("/compareReports")]
        public IActionResult compareReports([FromBody] List<ElasticPdfModel> documentIds)
        {
            if (documentIds == null || documentIds.Count < 2)
            {
                throw new ArgumentException("At least two documents are required for comparison.");
            }
            var documentsData = new List<object>();

            foreach (var documentId in documentIds)
            {
                var response = _elasticSearchService.Client.Get<object>(
                documentId.id,
                g => g.Index("pdf_data")
                );

                if (!response.IsValid)
                {
                    throw new Exception($"Failed to retrieve document with id: {documentId.id}");
                }

                documentsData.Add(response.Source);
            }


            _patientsService.CompareReports(documentsData);
            return Ok(documentsData);
        }
    }
}