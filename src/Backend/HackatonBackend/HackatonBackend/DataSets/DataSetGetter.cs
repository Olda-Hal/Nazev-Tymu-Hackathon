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
        public List<String> GetMockSets()
        {
            string folderPath = @"C:\YourFolderPath";

            // Check if the directory exists
            if (Directory.Exists(folderPath))
            {
                // Get all file names in the directory
                string[] fileNames = Directory.GetFiles(folderPath);

                // Display each file name
                foreach (string fileName in fileNames)
                {
                    Console.WriteLine(fileName);
                }
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
            }

            Console.ReadLine(); // Keep console window open
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
