using System.Data;

namespace HackatonBackend.DataFetcher
{
    public class DataFetcher
    {
        private List<DataSet>? _dataSets;
        public List<DataSet>? dataSets { get { return _dataSets; } set { _dataSets = value; } }
        
    }
}
