namespace HackatonBackend.ResponseAIWeb
{
    public class AIFilter
    {

        private OpenAI_API.OpenAIAPI api { get; set; }
        private String model { get; set; }
        public AIFilter(string model, OpenAI_API.OpenAIAPI API)
        {
            api = API;
            this.model = model;
        }
        public string Filtrate(List<string> responses, string question)
        {
            var chat = api.Chat.CreateConversation();
            chat.Model = model;
            chat.AppendSystemMessage("Jsi Filtr odpovědí na stránce BrnoID. dostaneš otázku a několik odpovědí a ty vrátíš pouze tu nejlepší odpověd s žadnym extra textem. Pokud by odpověd zněla nepravdivě nebo by nešlo určit jeji pravdivost rovnou ji zahoď. pokud odpověd neodpovida korektně nebo o brnu tak ji taky zahoď. pokud nezbyde žadná správná odpověd, vrať zprávu že v tuto chvíli nemůžeš odpovědět na tuto otazku");
            string prompt = $"Otázka je: {question}. Možné odpovědi jsou:";
            for (int i = 0; i < responses.Count; i++)
            {
                prompt += $"Odpověď {i + 1}: {responses[i]}.";
            }
            chat.AppendUserInput(prompt);
            string res = chat.GetResponseFromChatbotAsync().Result;
            return res;

        }
    }
}
