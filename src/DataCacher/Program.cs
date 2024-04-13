namespace DataCacher
{
    class program
    {
        public static int Main()
        {
            List<(string, string)> testdata = new List<(string, string)>()
            {
                ("abcde","efgh"),
                ("Tomáš", "Jedno"),
            };
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            //DataCacher dataCacher = new DataCacher(System.IO.Directory.GetCurrentDirectory());
            //dataCacher.Data2CSV(testdata);
            Console.WriteLine("hello world!");
            return 0;
        }
    }
}
