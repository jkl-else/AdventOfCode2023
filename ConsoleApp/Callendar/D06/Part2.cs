using System.Text.RegularExpressions;

namespace ConsoleApp.Callendar.D06
{
    internal class Part2 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input2");//Result = 23501589 - Result in: 00:00:00.6314922

            var time = Parse(input[0], "Time");
            var distance = Parse(input[1], "Distance");

            var result = CalculateDistances(time)
                .Count(x => x > distance);
            return result.ToString();
        }
        private static long Parse(string value, string id)
        {
            var result = String.Join("", new Regex($"{id}:[ ]*(?<values>[0-9 ]*)")
                .Match(value)
                .Groups["values"]
                .Value.Split(' ')
                .Where(x => x.Length > 0));
            return long.Parse(result);
        }
        private static IEnumerable<long> CalculateDistances(long time)
        {
            for (long i = 0; i < time; i++)
                yield return i * (time - i);
        }
    }
}
