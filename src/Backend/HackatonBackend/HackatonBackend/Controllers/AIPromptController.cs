
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using System.Collections;
using TH = System.Threading.Tasks;

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
            string res = "NEGUNGUJE UU DEAJAJAJJAA";
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
