using System.Diagnostics.Tracing;
using System.IO;
using System.Xml;

namespace HackatonBackend.DataSets
{
    public class DataSetGetter
    {
        HttpClient client;
        public DataSetGetter()
        {
            client = new HttpClient();
        }
        public DataSet? GetBestDataset(string prompt, List<DataSet> datasets)
        {
            string? Key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("OpenAPIKey")["Key"];
            char[] temp = Key!.ToCharArray();
            Array.Reverse(temp);
            string reversedKey = new string(temp);
            var api = new OpenAI_API.OpenAIAPI(apiKeys: reversedKey);
            var chat = api.Chat.CreateConversation();
            chat.Model = new OpenAI_API.Models.Model("gpt-4-turbo");
            string PREPROMT = "Dostaneš seznam datasetů ve formatu NAME:jmenoDataSetu; a taky otázku. tvým úkolem bude vybrat jeden dataset který nejlépe pomůže odpovědět na zadanou otázku. odpovíš steným formátem jak ti byl zadaný Dataset. tudíž NAME:jmenoDataSetu";
            chat.AppendSystemMessage(PREPROMT);
            string ds = "";
            foreach (var dt in datasets)
            {
                ds += "NAME:" + dt.Name + "\n";
            }
            ds += prompt;
            chat.AppendUserInput(ds);
            var response = chat.GetResponseFromChatbotAsync().Result;
            if (response.Contains("NAME"))
            {
                string[] split = response.Split(":");
                string name = split[1];
                try
                {
                    return datasets.Find(x => x.Name == name);
                }
                catch
                {
                    return null;
                }
                
            }
            else
            {
                return null;
            }
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
        public List<DataSet> GetMockSets()
        {
            string folderPath = @"../../../Data";
            folderPath = "C:\\Users\\petra\\Desktop\\hackathon\\Nazev-Tymu-Hackathon\\src\\Backend\\HackatonBackend\\HackatonBackend\\Data\\";
            List<DataSet> sets = new List<DataSet>();

            // Check if the directory exists
            if (Directory.Exists(folderPath))
            {
                // Get all file names in the directory
                string[] fileNames = Directory.GetFiles(folderPath);

                // Display each file name
                foreach (string fileName in fileNames)
                {
                    sets.Add(new DataSet() { Name = fileName.Split(new char[] {'\\'}).Last(), Description = fileName });
                }
                return sets;
            }
            else
            {
                throw new Exception("Directory does not exist.");
            }

            return null; // Keep console window open
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
    }
}
