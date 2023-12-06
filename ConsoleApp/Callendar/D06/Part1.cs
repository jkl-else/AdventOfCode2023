using System.Text.RegularExpressions;

namespace ConsoleApp.Callendar.D06
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input1");//Result = 1083852 - Result = 23501589
            var times = Parse(input[0], "Time");
            var distances = Parse(input[1], "Distance");

            var result = times.Select((t, i) =>
            {
                var distance = distances[i];
                var optionalValues = CalculateDistances(t)
                    .Where(d => d > distance);
                return optionalValues.Count();
            });

            return result.Aggregate(1, (current, r) => current * r)
                .ToString();
        }
        private static List<int> Parse(string value, string id) => new Regex($"{id}:[ ]*(?<values>[0-9 ]*)")
            .Match(value)
            .Groups["values"]
            .Value.Split(' ')
            .Where(x => x.Length > 0)
            .Select(int.Parse)
            .ToList();
        private static IEnumerable<int> CalculateDistances(int time)
        {
            for (var i = 0; i < time; i++)
                yield return i * (time - i);
        }
    }
}
