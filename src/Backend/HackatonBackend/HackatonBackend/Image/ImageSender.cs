using OpenAI_API;
using System.Diagnostics;
using System.Text;
namespace HackatonBackend.Image
{
    public class ImageSender
    {
        public static async Task<string> SendImage(string imagePath)
        {
            string pythonInterpreter = "python3";
            //!TODO
            string pythonScript = Utils.GetDir.GetSpecificSubdirPath("Image") + "imageSend.py";
            string? Key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("OpenAPIKey")["Key"];
            char[] temp = Key!.ToCharArray();
            Array.Reverse(temp);
            string reversedKey = new string(temp);
            // Create process info
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = pythonInterpreter;
            startInfo.StandardOutputEncoding = Encoding.UTF8;
            startInfo.Arguments = pythonScript;
            startInfo.UseShellExecute = false;    // Do not use OS shell
            startInfo.RedirectStandardOutput = true;  // Redirect the output stream
            startInfo.Arguments = $"{pythonScript} {reversedKey} {imagePath}";
            // Create and start the process
            using (Process process = Process.Start(startInfo))
            {
                // Read the output of the Python script
                using (StreamReader reader = process.StandardOutput)
                {
                    return reader.ReadToEnd();

                    // Display the decoded string
                }
            }

        }
    }
}
