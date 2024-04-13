using HackatonBackend.DataSets;
using TH = System.Threading.Tasks;

namespace HackatonBackend.ResponseAIWeb
{
    public class WebManager
    {
        public int density { get; set; }
        public List<string> plausibleModels { get; set; }
        OpenAI_API.OpenAIAPI API = new OpenAI_API.OpenAIAPI("API_KEY");
        public List<AIClient> clients { get; set; }

        public WebManager(int density=3, List<string> plausibleModels = null)
        {
            this.density = density;
            if (plausibleModels is null){
                this.plausibleModels = new List<string>() { "gpt3-turbo" };
            }
            else
            {
                this.plausibleModels = plausibleModels;
            }
            

        }
        private void inicializeWeb()
        {
            for (int i = 0; i < density; i++)
            {
                // picks random model from plausiblemodels and creates a client
                string model = plausibleModels[new Random().Next(plausibleModels.Count)];
                clients.Add(new AIClient(model, API));
                
            }
        }
        public List<string> GenerateResponse(string prompt)
        {
            List<string> response = new List<string>();
            for (int i = 0; i < density; i++)
            {
                response.Add(TH.Task.Run(() => clients[i].GenerateResponse(prompt)).Result);
                
            }
            return response;
        }
        
    }
}
