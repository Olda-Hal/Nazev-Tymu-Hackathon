
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using System.Collections;
using TH = System.Threading.Tasks;
using HackatonBackend.ResponseAIWeb;
using HackatonBackend.DataSets;
using System.Collections.Generic;
using System.IO;

namespace HackatonBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AIPromptController : ControllerBase
    {
        private readonly ILogger<AIPromptController> _logger;

        public AIPromptController(ILogger<AIPromptController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{input}", Name = "AskAI")]
        public async Task<IEnumerable<AIPrompt>> Get(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            DataSetGetter dataSetGetter = new DataSetGetter();
            List<DataSet> dataSets = dataSetGetter.GetMockSets();
            DataSet bestDataSet = dataSetGetter.GetBestDataset(input, dataSets);
            WebManager webManager = new WebManager();
            string bestDatasetContent = System.IO.File.ReadAllText("C:\\Users\\petra\\Desktop\\hackathon\\Nazev-Tymu-Hackathon\\src\\Backend\\HackatonBackend\\HackatonBackend\\Data\\" + bestDataSet.Name);


            string res = webManager.GenerateResponse(bestDatasetContent + input, input);

            AIPrompt aIRequest = new AIPrompt
            {
                Prompt = res,
            };
            List<AIPrompt> list = new List<AIPrompt>();
            list.Add(aIRequest);
            return list;
        }
    }
}
