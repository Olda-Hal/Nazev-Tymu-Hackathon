using DataCacher;
using System.Data;
using System.Xml;
using DataSet = DataCacher.DataSet;
namespace HackatonBackend.DataSets
{
    public class DataSetGetter
    {
        HttpClient client;
        public DataSetGetter()
        {
            client = new HttpClient();
        }
        public async Task<List<DataSet>?> GetAllSets()
        {
            List<DataSet> sets = new List<DataSet>();
            string apiUrl = "https://data.brno.cz/api/feed/rss/2.0";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string xmlContent = await response.Content.ReadAsStringAsync();
                    return ParseXml(xmlContent);

                }
                else
                {
                    throw new Exception("No DataSets found");
                }
            }
        }
        List<DataSet>? ParseXml(string xmlContent)
        {
            List<DataSet> ret = new List<DataSet>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);

                XmlNodeList items = xmlDoc.SelectNodes("//item")!;
                foreach (XmlNode item in items)
                {
                    string title = item.SelectSingleNode("title")?.InnerText!;
                    string description = item.SelectSingleNode("description")?.InnerText!;
                    DataSet temp = new DataSet() { Description = description, Name = title };
                    ret.Add(temp);
                }
            }
            catch
            {
                return null;
            }
            return ret;
        }
        public string GetValidDataSets()
        {
            string? Key = File.ReadAllLines("../../../key.txt").FirstOrDefault();
            char[] temp = Key!.ToCharArray();
            Array.Reverse(temp);
            string reversedKey = new string(temp);
            var api = new OpenAI_API.OpenAIAPI(apiKeys: reversedKey);
            var chat = api.Chat.CreateConversation();
            chat.Model = new OpenAI_API.Models.Model("gpt-3.5-turbo");
            string PREPROMT = "Jsi assistent na stránce BrnoID. tvým úkolem bude radit ohledně otázek týkající se města Brna." +
                " V následnují otázce dostaneš seznam datasetů a jejich popisů." +
                " Z jmena udělej stručnější popis o max dvou větách." +
                " Data Sety budou ve formátu [NAME:jmeno;DESCRIPTION:popis] +. Tvé odpovědi musí být ve formátu [NAME:jmeno;DESCRIPTION:tvujPopis]";
            chat.AppendSystemMessage(PREPROMT);
            string ds = "";
            string result = "";
            int i = 0;
            foreach (var dt in GetAllSets().Result!)
            {
                ds += "[NAME:" + dt.Name + ";DESCRIPTION:"+"]\n";
                chat.AppendUserInput(ds);
                var response = chat.GetResponseFromChatbotAsync().Result;
                result += response;
                Console.WriteLine(response);
            }
            Console.WriteLine(result);
            return result;
        }
    }
}
