//using iText.Kernel.Pdf.Canvas.Parser;
//using iText.Kernel.Pdf;
//using MedicalAPI.Models.ExtractPdfModels;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace MedicalAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PdfTableExtractor : ControllerBase
//    {
//        [HttpPost("extract-tables")]
//        public IActionResult ExtractTables([FromForm] IFormFile file)
//        {
//            if (file == null || file.Length == 0)
//            {
//                return BadRequest("No file uploaded.");
//            }

//            try
//            {
//                // Read PDF file content
//                using (var stream = file.OpenReadStream())
//                {
//                    using (var reader = new PdfReader(stream))
//                    {
//                        using (var pdfDoc = new PdfDocument(reader))
//                        {
//                            List<List<string>> tables = new List<List<string>>();

//                            // Iterate through all pages
//                            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
//                            {
//                                var page = pdfDoc.GetPage(i);
//                                var pageText = PdfTextExtractor.GetTextFromPage(page);

//                                // Example: Parsing the text (you might need a more advanced parsing strategy here)
//                                var table = ParseTableFromText(pageText);
//                                if (table != null && table.Count > 0)
//                                {
//                                    tables.Add(table);
//                                }
//                            }

//                            // Return tables as JSON response
//                            return Ok(new TableResponse { Tables = tables });
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        // Example method for parsing a simple table from the text
//        private List<string> ParseTableFromText(string text)
//        {
//            var table = new List<string>();

//            // Basic parsing logic - split text into lines and process
//            var lines = text.Split('\n');
//            foreach (var line in lines)
//            {
//                var columns = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//                if (columns.Length > 0)
//                {
//                    table.Add(string.Join(", ", columns)); // or customize table row structure
//                }
//            }

//            return table;
//        }
//    }
//}
