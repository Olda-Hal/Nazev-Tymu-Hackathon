using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCacher
{
    public class DataCacher
    {
        private String path;
        public DataCacher(String path)
        {
            this.path = path;
        }
        public void Data2CSV(List<(string,string)> data)
        {
            String fullPath = path+"cache.csv";

            using (var streamWriter = new StreamWriter(fullPath))
            {
                // Write header
                streamWriter.WriteLine("Name,Age");

                // Write data rows
                foreach (var item in data)
                {
                    streamWriter.WriteLine($"{item.Item1},{item.Item2}");
                }
            }
        }
    }
}
