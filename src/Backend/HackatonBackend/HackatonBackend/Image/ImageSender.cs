using OpenAI_API;
using System.Diagnostics;
namespace HackatonBackend.Image
{
    public class ImageSender
    {
        public static async void SendImage(string imagePath)
        {
            string pythonInterpreter = "python3";
            string pythonScript = @"C:\Users\petra\Desktop\hackathon\Nazev-Tymu-Hackathon\src\Backend\HackatonBackend\HackatonBackend\Image\imageSend.py";
            string? Key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("OpenAPIKey")["Key"];
            char[] temp = Key!.ToCharArray();
            Array.Reverse(temp);
            string reversedKey = new string(temp);
            // Create process info
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = pythonInterpreter;
            startInfo.Arguments = pythonScript;
            startInfo.UseShellExecute = false;    // Do not use OS shell
            startInfo.RedirectStandardOutput = true;  // Redirect the output stream
            startInfo.Arguments = $"{pythonScript} {reversedKey} {imagePath}";
            // Create and start the process
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            // Read the output stream asynchronously
            process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            process.BeginOutputReadLine();

            // Wait for the process to exit
            process.WaitForExit();

        }
    }
}
