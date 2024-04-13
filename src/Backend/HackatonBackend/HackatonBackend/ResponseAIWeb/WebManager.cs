using HackatonBackend.DataSets;
using OpenAI_API.Models;
using TH = System.Threading.Tasks;

namespace HackatonBackend.ResponseAIWeb
{
    public class WebManager
    {
        public int density { get; set; }
        public List<string> plausibleModels { get; set; }
        public AIFilter filter { get; set; }
        public string APIKey
        {
            get
            {
                string? Key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("OpenAPIKey")["Key"];
                char[] temp = Key!.ToCharArray();
                Array.Reverse(temp);
                string reversedKey = new string(temp);
                return reversedKey;
            }
        }
        public OpenAI_API.OpenAIAPI API { get; set; }
        public List<AIClient> clients { get; set; }

        public WebManager(int density=3, List<string> plausibleModels = null)
        {
            API = new OpenAI_API.OpenAIAPI(APIKey);
            this.density = density;
            if (plausibleModels is null){
                this.plausibleModels = new List<string>() { "gpt-4-turbo" };
            }
            else
            {
                this.plausibleModels = plausibleModels;
            }
            inicializeWeb();
            

        }
        private void inicializeWeb()
        {
            clients = new List<AIClient>();
            for (int i = 0; i < density; i++)
            {
                // picks random model from plausiblemodels and creates a client
                string model = plausibleModels[new Random().Next(plausibleModels.Count)];
                clients.Add(new AIClient(model, API));
            }
            filter = new AIFilter("gpt-4-turbo", API);
        }
        public string GenerateResponse(string prompt, string question)
        {
            List<string> response = new List<string>();
            for (int i = 0; i < density; i++)
            {
                Console.WriteLine($"Generating response {i}");
                response.Add(TH.Task.Run(() => clients[i].GenerateResponse(prompt)).Result);
                
            }
            return filter.Filtrate(response, question);
        }
        
    }
}
