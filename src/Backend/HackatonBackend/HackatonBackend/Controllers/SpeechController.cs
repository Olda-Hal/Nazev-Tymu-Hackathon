using HackatonBackend.DataSets;
using HackatonBackend.Models;
using HackatonBackend.ResponseAIWeb;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
namespace HackatonBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpeechController:ControllerBase
    {
        private readonly string _uploadFolderPath;

        public SpeechController()
        {
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadModel model)
        {
            if (model == null || model.File == null || model.File.Length == 0)
            {
                return BadRequest("Invalid file");
            }
            string uniqueFileName = Path.GetFileNameWithoutExtension(model.File.FileName) + "_" +
                                    Guid.NewGuid().ToString() +
                                    Path.GetExtension(model.File.FileName);
            Console.WriteLine("unique name: " + uniqueFileName);
            var directory = Utils.GetDir.GetSpecificSubdirPath("Data");
            //Console
            DataSetGetter dataSetGetter = new DataSetGetter();
            List<DataSet> dataSets = dataSetGetter.GetMockSets();
            string? input = await SpeechHandler.SpeechHandler.Transcript(directory+model.File.FileName, true);
            if(string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            DataSet bestDataSet = dataSetGetter.GetBestDataset(input, dataSets);
            WebManager webManager = new WebManager();
            // TODO fix this

            string bestDatasetContent = System.IO.File.ReadAllText(directory + bestDataSet.Name);


            string res = webManager.GenerateResponse(bestDatasetContent + input, input).Result;
            string resMP3Path = await SpeechHandler.SpeechHandler.TextToSpeech(res, directory+uniqueFileName);

            Console.WriteLine(res);
            Console.WriteLine(resMP3Path);

            // Path to your .mp3 file
            string filePath = resMP3Path.Replace(@"\", @"\\");

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Or handle the error as needed
            }

            // Read the file into a byte array (you can also stream it)
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            // Set the response headers
            Response.Headers.Add("Content-Disposition", "inline; filename="+uniqueFileName);
            Response.Headers.Add("Content-Type", "audio/mpeg"); // MIME type for .mp3

            // Write the file content to the response body
            return File(fileBytes, "audio/mpeg");
        }
    }
}
