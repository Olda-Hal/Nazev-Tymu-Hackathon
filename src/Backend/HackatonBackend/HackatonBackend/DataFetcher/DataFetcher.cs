using HackatonBackend.DataSets;
using System.Data;

namespace HackatonBackend.DataFetcher
{
    public class DataFetcher
    {
        public List<DataSets.DataSet>? dataSets => new DataSets.DataSetGetter().GetAllSets().Result;

        public string GetValidDataSets(string question)
        {
            string? Key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("OpenAPIKey")["Key"];
            char[] temp = Key!.ToCharArray();
            Array.Reverse(temp);
            string reversedKey = new string(temp);
            var api = new OpenAI_API.OpenAIAPI(apiKeys: reversedKey);
            var chat = api.Chat.CreateConversation();
            chat.Model = new OpenAI_API.Models.Model("gpt-4-turbo");
            string PREPROMT = "Jsi assistent na stránce BrnoID. tvým úkolem bude radit ohledně otázek týkající se města Brna," +
                " na nic co se netýká Brna nesmíš odpovídat. Aby tvoje odpovědi byli přesnější, " +
                "při každé otázce ti bude poskytnutý seznam datasetů ve formátu [NAME:jmenoDataSetu;DESCRIPTION:description] z kterých si můžeš vybrat a dostaneš k ním konkretní informace." +
                "Pokud chceš získat data z datasetu který ti byl nabýdnut odpověz \"DATASET {jmenodatasetu}\" a v další odpovědi dostaneš žádaná data. Vyber maximálně 10 datasetů";
            chat.AppendSystemMessage(PREPROMT);
            string ds = "";
            foreach(var dt in dataSets!)
            {
                ds += "[NAME:"+dt.Name + ";DESCRIPTION:"+"]\n";
            }
            ds += question;
            chat.AppendUserInput(ds);
            var response = chat.GetResponseFromChatbotAsync().Result;
            Console.WriteLine(response);
            return response;
        }
        private string ReverseString(string str)
        {
            string ret = "";
            for(int i = str.Length-1; i < -1;i--)
            {
                ret += str[i];
            }
            return ret;
        }
    }
}
