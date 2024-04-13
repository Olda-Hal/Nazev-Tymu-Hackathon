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
            var api=SpeechHandler.getAPI();
            if(api==null)
            {
                throw new ArgumentNullException(nameof(api));
            }
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
        public static async void TextToSpeech(string inputString,string outputPath,bool overwrite=true)
        {
            var api = SpeechHandler.getAPI();
            if (api == null)
            {
                throw new ArgumentNullException(nameof(api));
            }
            if(File.Exists(outputPath)&&overwrite==false)
            {
                throw new Exception("File already exist");
            }
            await api.TextToSpeech.SaveSpeechToFileAsync(inputString, outputPath);
        }
    }
}
