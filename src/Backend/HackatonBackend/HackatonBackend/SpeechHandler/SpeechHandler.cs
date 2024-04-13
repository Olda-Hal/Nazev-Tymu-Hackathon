namespace HackatonBackend.SpeechHandler
{
    public class SpeechHandler
    {
        private static OpenAI_API.OpenAIAPI? getAPI()
        {
            string? Key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("OpenAPIKey")["Key"];
            char[] temp = Key!.ToCharArray();
            Array.Reverse(temp);
            string reversedKey = new string(temp);
            var api = new OpenAI_API.OpenAIAPI(apiKeys: reversedKey);
            return api;
        }
        public static async Task<string?> Transcript(string pathToMP3,bool deleteOld=false)
        {
            var api = getAPI();
            string result = await api.Transcriptions.GetTextAsync(pathToMP3);
            if (deleteOld)
            {
                if (File.Exists(pathToMP3))
                {
                    try
                    {
                        File.Delete(pathToMP3);
                    }
                    catch
                    {
                        // do nothing ...
                    }
                }
            }
            return result;
        }
        public static async Task<string> TextToSpeech(string inputString,string outputPath,bool overwrite=true)
        {
            var api = getAPI();
            if (File.Exists(outputPath)&&overwrite==false)
            {
                throw new Exception("File already exist");
            }
            await api.TextToSpeech.SaveSpeechToFileAsync(inputString, outputPath);
            if (!File.Exists(outputPath))
            {
                throw new Exception("File already exist");
            }
            return outputPath;
        }
        //public static async Task<string> GetSpeechResponse(string inputMP3Path,string outputMP3Path)
        //{
        //    var api = getAPI();
        //    string? str = await Transcript(api,inputMP3Path, true);
        //    var chat = api.Chat.CreateConversation();
        //    if (str==null)
        //    {
        //        throw new Exception("Failed to convert MP3 to string");
        //    }
        //    chat.Model = new OpenAI_API.Models.Model("gpt-4-turbo");

        //    return await TextToSpeech(api, inputMP3Path, outputMP3Path);
        //}

    }
}
