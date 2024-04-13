using HackatonBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace HackatonBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly string _uploadFolderPath;

        public ImageController()
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
            string filePath = Path.Combine(_uploadFolderPath, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }
            var metaData = Image.MetadataConverter.GetMetadata(filePath);
            var promt = Image.ImageSender.SendImage(filePath).Result;
            System.IO.File.Delete(filePath);
            return Ok(new { Prompt = promt });
        }
    }
}
