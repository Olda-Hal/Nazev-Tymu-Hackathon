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
            return Ok(new { FilePath = filePath });
        }
        public string GetMetadata(string filePath)
        {
            string ret = "";
            try
            {
                using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    var image = System.Drawing.Image.FromStream(stream, false, false);

                    if (image.PropertyIdList != null)
                    {
                        foreach (int propertyId in image.PropertyIdList)
                        {
                            var propItem = image.GetPropertyItem(propertyId);

                            if (propertyId == 0x0004) 
                            {
                                var gpsInfo = GetGpsInfo(propItem);
                                ret +=$"GPS Info: {gpsInfo}\n";
                            }
                            else if (propertyId == 0x013b)
                            {
                                var author = System.Text.Encoding.ASCII.GetString(propItem.Value);
                                ret += $"Author: {author}\n";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading metadata: {ex.Message}");
            }
            return ret;
        }
        private string GetGpsInfo(PropertyItem propItem)
        {
            double degrees = BitConverter.ToUInt32(propItem.Value, 0) / BitConverter.ToUInt32(propItem.Value, 4);
            double minutes = BitConverter.ToUInt32(propItem.Value, 8) / BitConverter.ToUInt32(propItem.Value, 12);
            double seconds = BitConverter.ToUInt32(propItem.Value, 16) / BitConverter.ToUInt32(propItem.Value, 20);

            string directionRef = System.Text.Encoding.ASCII.GetString(propItem.Value, 4, 4).Trim();
            string latitudeRef = directionRef[0] == 'N' ? "North" : "South";

            string longitudeRef = System.Text.Encoding.ASCII.GetString(propItem.Value, 20, 4).Trim();
            string longitudeDir = longitudeRef[0] == 'E' ? "East" : "West";

            return $"{degrees}° {minutes}' {seconds}\" {latitudeRef}, {longitudeDir}";
        }
    }
}
