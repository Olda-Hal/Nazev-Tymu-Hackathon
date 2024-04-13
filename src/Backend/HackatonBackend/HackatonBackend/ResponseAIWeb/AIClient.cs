using OpenAI_API.Chat;
using System.Globalization;
namespace HackatonBackend.ResponseAIWeb
{
    public class AIClient
    {
        private OpenAI_API.OpenAIAPI api { get; set; }
        private String model { get; set; }
        public AIClient(string model, OpenAI_API.OpenAIAPI API)
        {
           api = API;
           this.model = model;
        }
        public string GenerateResponse(string prompt)
        {
            var chat = api.Chat.CreateConversation();
            chat.Model = model;
            chat.AppendSystemMessage("Jsi assistent na strance BrnoID, mužeš odpovidat pouze na otazky tykajici se brna, pokud nemáš přesná data a nevíš přesně tak řekni nevím, nesmíš podávat chybné informace. Spolu s otázkou ti bude poskytnuto několik datasetů které byli vybrané tak aby ti pomohli odpovědět, použivej je na upřesnění otázky.");
            chat.AppendUserInput(prompt);
            string res = chat.GetResponseFromChatbotAsync().Result;
            Console.WriteLine(res);
            return res;
        }

    }
}
