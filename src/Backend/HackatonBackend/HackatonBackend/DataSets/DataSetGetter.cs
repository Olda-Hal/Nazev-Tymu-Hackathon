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
