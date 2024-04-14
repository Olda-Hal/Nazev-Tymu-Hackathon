using HackatonBackend.DataSets;
using HackatonBackend.Models;
using HackatonBackend.ResponseAIWeb;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace HackatonBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private WebManager webManager;
        private readonly ILogger<AIPromptController> _logger;
        private readonly string _uploadFolderPath;

        public UploadController(ILogger<AIPromptController> logger)
        {
            _logger = logger;
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(_uploadFolderPath))
            {
                Directory.CreateDirectory(_uploadFolderPath);
            }
        }

        [HttpPost("upload")]
        [EnableCors("AllowAllOrigins")]
        public async Task<IActionResult> UploadData([FromForm] FileUploadModel model)
        {
            if (model != null)
            {
                if (model.File != null)
                {
                    Console.WriteLine("Received file: " + model.File.FileName);
                    string extension = Path.GetExtension(model.File.FileName);
                    if (extension == ".txt")
                    {
                        string fileContent = "";
                        string filePath = Path.Combine(_uploadFolderPath, model.File.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(stream);
                        }
                        fileContent += System.IO.File.ReadAllText(filePath);
                        Console.WriteLine("Received message: " + fileContent);
                        if (string.IsNullOrWhiteSpace(fileContent))
                        {
                            return null;
                        }
                        string input = fileContent;

                        DataSetGetter dataSetGetter = new DataSetGetter();
                        List<DataSet> dataSets = dataSetGetter.GetMockSets();
                        DataSet bestDataSet = dataSetGetter.GetBestDataset(input, dataSets);
                        if (webManager == null)
                        {
                            webManager = new WebManager();
                        }
                        string bestDatasetContent = "";
                        if (bestDataSet != null)
                        {
                            bestDatasetContent = System.IO.File.ReadAllText(Utils.GetDir.GetSpecificSubdirPath("Data") + bestDataSet.Name);
                        }
                        bestDatasetContent += "\n Dnešní datum je 14.4.2024\n";



                        string res = webManager.GenerateResponse(bestDatasetContent + input, input).Result;
                        System.IO.File.Delete(filePath);
                        return new JsonResult(new { text = res });
                    }
                    if (extension == ".jpg" || extension == ".png")
                    {
                        Console.WriteLine("Received image");
                        Console.WriteLine(model.File.FileName);
                        if (model == null || model.File == null || model.File.Length == 0)
                        {
                            return BadRequest("Invalid file");
                        }
                        string uniqueFileName = Path.GetFileNameWithoutExtension(model.File.FileName) + "_" +
                                                Guid.NewGuid().ToString() +
                                                Path.GetExtension(model.File.FileName);
                        string filePath = Path.Combine(_uploadFolderPath, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(stream);
                        }
                        var metaData = Image.MetadataConverter.GetMetadata(filePath);
                        var promt = Image.ImageSender.SendImage(filePath).Result;
                        System.IO.File.Delete(filePath);
                        return new JsonResult(new { text = promt });
                    }
                    if (extension == ".mp3")
                    {
                        if (model == null || model.File == null || model.File.Length == 0)
                        {
                            return BadRequest("Invalid file");
                        }
                        string uniqueFileName = Path.GetFileNameWithoutExtension(model.File.FileName) + "_" +
                                                Guid.NewGuid().ToString() +
                                                Path.GetExtension(model.File.FileName);
                        string filePath = Path.Combine(_uploadFolderPath, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(stream);
                        }
                        DataSetGetter dataSetGetter = new DataSetGetter();
                        List<DataSet> dataSets = dataSetGetter.GetMockSets();
                        string? input = await SpeechHandler.SpeechHandler.Transcript(_uploadFolderPath + "\\" + uniqueFileName, true);
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            return null;
                        }
                        DataSet bestDataSet = dataSetGetter.GetBestDataset(input, dataSets);
                        WebManager webManager = new WebManager();
                        // TODO fix this
                        var directory = Utils.GetDir.GetSpecificSubdirPath("Data");

                        string bestDatasetContent = System.IO.File.ReadAllText(directory + bestDataSet.Name);


                        string res = webManager.GenerateResponse(bestDatasetContent + input, input).Result;
                        string resMP3Path = await SpeechHandler.SpeechHandler.TextToSpeech(res, uniqueFileName);

                        Console.WriteLine(res);
                        Console.WriteLine(resMP3Path);

                        // Path to your .mp3 file
                        filePath = resMP3Path.Replace(@"\", @"\\");

                        // Check if the file exists
                        if (!System.IO.File.Exists(filePath))
                        {
                            return NotFound(); // Or handle the error as needed
                        }

                        // Read the file into a byte array (you can also stream it)
                        byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                        // Set the response headers
                        Response.Headers.Add("Content-Disposition", "inline; filename=" + uniqueFileName);
                        Response.Headers.Add("Content-Type", "audio/mpeg"); // MIME type for .mp3

                        // Write the file content to the response body
                        return File(fileBytes, "audio/mpeg");
                    }
                }
            }
            
            return new JsonResult(new { message = "File uploaded" });
        }
    }

}
