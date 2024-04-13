namespace HackatonBackend.Utils
{
    public static class GetDir
    {
        private static DirectoryInfo? GetDirectory(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                        currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
        public static string GetSpecificSubdirPath(string pathSuffix = "")
        {
            var dir = GetDirectory();
            return dir.FullName + "\\HackatonBackend\\" + pathSuffix + "\\";
        }
    }
}
