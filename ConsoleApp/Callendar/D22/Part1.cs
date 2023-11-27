namespace ConsoleApp.Callendar.D22
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Test");
            throw new NotFiniteNumberException();
        }
    }
}
