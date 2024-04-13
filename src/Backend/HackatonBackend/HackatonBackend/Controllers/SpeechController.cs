using HackatonBackend.DataSets;
using HackatonBackend.Models;
using HackatonBackend.ResponseAIWeb;
using Microsoft.AspNetCore.Mvc;
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
            DataSetGetter dataSetGetter = new DataSetGetter();
            List<DataSet> dataSets = dataSetGetter.GetMockSets();
            string? input = await SpeechHandler.SpeechHandler.Transcript(model.File.FileName, true);
            if(string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            DataSet bestDataSet = dataSetGetter.GetBestDataset(input, dataSets);
            WebManager webManager = new WebManager();
            // TODO fix this
            string bestDatasetContent = System.IO.File.ReadAllText("C:\\Users\\petra\\Desktop\\hackathon\\Nazev-Tymu-Hackathon\\src\\Backend\\HackatonBackend\\HackatonBackend\\Data\\" + bestDataSet.Name);


            string res = webManager.GenerateResponse(bestDatasetContent + input, input);
            string resMP3Path = await SpeechHandler.SpeechHandler.TextToSpeech(res, uniqueFileName);

            Console.WriteLine(res);
            Console.WriteLine(resMP3Path);
            return Ok(new { FilePath = resMP3Path });
        }
    }
}
