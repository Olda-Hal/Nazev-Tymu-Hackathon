using HackatonBackend.DataSets;
using System.Threading;
namespace DataCacher
{
    class program
    {
        public static int Main()
        {
            var data = new DataSetGetter().GetValidDataSets();
            var csv = new DataCacher("./");
            csv.Data2CSV(data);
            //Timer timer = new Timer(TimerCallback, null, 0, 1000);
            return 0;
        }
    }
    //static void TimerCallback(object state)
    //{
    //    Console.WriteLine($"Timer fired at: {DateTime.Now}");
    //}

}
