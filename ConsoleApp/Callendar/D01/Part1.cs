namespace ConsoleApp.Callendar.D01
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input1");//Result = 55090 - Result in: 00:00:00.0429417
            return input.Select(x => new
            {
                First = x.FirstOrDefault(char.IsDigit),
                Last = x.LastOrDefault(char.IsDigit)
            })
            .Select(x => new string(new[] { x.First, x.Last }))
            .Select(int.Parse)
            .Sum()
            .ToString();
        }
    }
}
