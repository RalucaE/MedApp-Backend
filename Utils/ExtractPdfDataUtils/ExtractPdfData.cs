using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace MedicalAPI.Utils
{
    public class ExtractPdfData {
        public static List<Dictionary<string, object>> RunPythonScript(string path)
        {
            var resultList = new List<Dictionary<string, object>>();
            // Path to the Python interpreter
            string pythonPath = @"C:\Python312\python.exe";
            // Path to the Python script
            string scriptPath = @"C:/Users/CiangauRalucaElena/Desktop/MedicalApp/backend/MedicalAPI/Utils/ExtractPdfDataUtils/extract.py";

            // Set up the process to start the Python script
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = pythonPath, // Python executable
                Arguments = $"{scriptPath} {path}", // Script and arguments
                RedirectStandardOutput = true, // Redirect standard output
                RedirectStandardError = true, // Redirect standard error
                StandardOutputEncoding = Encoding.UTF8,
                UseShellExecute = false, // Do not use shell execution
                CreateNoWindow = true,  // Do not create a command window
                
            };
            // Start the process
            //using (Process process = Process.Start(psi))
            //{
            //    // Read the output from the Python script
            //    var output = process.StandardOutput.ReadToEnd();

            //    string error = process.StandardError.ReadToEnd();

            //    process.WaitForExit();

            //    // Output results

            //   resultList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(output);

            //    //resultList = output;
            //    if (!string.IsNullOrEmpty(error))
            //    {
            //        Console.WriteLine("Error:");
            //        Console.WriteLine(error);
            //    }

            //}
            using (Process process = new Process())
            {
                process.StartInfo = psi;
                process.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        resultList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(args.Data);                                         
                    }
                };
                process.ErrorDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Console.Error.WriteLine(args.Data);
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
            return resultList;
        }
    }
}