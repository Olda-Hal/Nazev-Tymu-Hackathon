
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using System.Collections;
using TH = System.Threading.Tasks;
using HackatonBackend.ResponseAIWeb;
using HackatonBackend.DataSets;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;


namespace HackatonBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AIPromptController : ControllerBase
    {
        private WebManager webManager;
        private readonly ILogger<AIPromptController> _logger;

        public AIPromptController(ILogger<AIPromptController> logger)
        {
            _logger = logger;
        }

        public class MessageModel
        {
            public List<Message> Messages { get; set; }
        }

        public class Message
        {
            public string Role { get; set; }
            public string Text { get; set; }
        }
        [EnableCors("AllowAllOrigins")]
        [HttpPost("AskAI")]
        public async Task<IActionResult> Get([FromBody] MessageModel model)
        {
            Console.WriteLine("Received message: " + model.Messages[0].Text);
            if (string.IsNullOrWhiteSpace(model.Messages[0].Text))
            {
                return null;
            }
            string input = model.Messages[0].Text;

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

            return new JsonResult(new {text = res });
        }
    }
}
