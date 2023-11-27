namespace ConsoleApp
{
    internal abstract class Part
    {
        public abstract Task<string> GetResultAsync();
        /// <summary>
        /// Read lies from txt dokument
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected Task<string[]> ReadFileLinesAsync(string fileName) => File.ReadAllLinesAsync(GetPath(fileName));
        /// <summary>
        /// Read all text from dokument
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected Task<string> ReadFileTextAsync(string fileName) => File.ReadAllTextAsync(GetPath(fileName));
        /// <summary>
        /// Get full path by file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetPath(string fileName)
        {
            var filePath = Path.Combine(String.Join("\\", GetType().Namespace!.Split('.').Skip(1)), $"{fileName}.txt");
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\", filePath);
        }
    }
}
