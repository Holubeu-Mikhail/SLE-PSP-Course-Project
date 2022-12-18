namespace Client.Files
{
    public class FileWriter
    {
        private readonly string _basePath;

        public FileWriter(string basePath)
        {
            _basePath = basePath;
        }

        public void WriteToFile(string fileName, string content) =>
            File.WriteAllText($"{_basePath}{fileName}", content);
    }
}
