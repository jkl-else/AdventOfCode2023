using System.Text.RegularExpressions;

namespace ConsoleApp.Callendar.D05
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileTextAsync("Input1"); //Result = 389056265 - Result in: 00:00:00.0692409
            var seeds = new Regex("^seeds: (?<seeds>[0-9 ]*)").Match(input).Groups[1].Value
                .Split(' ')
                .Select(x => new Seed(long.Parse(x)))
                .ToList();
            var mapgroups = new Regex(@"(?<id>[a-z-]*) map:\r\n(?<map>[0-9 \r\n]*)\r\n")
                .Matches(input)
                .Select(x => new
                {
                    Id = x.Groups["id"].Value,
                    Maps = x.Groups["map"].Value
                    .Split(Environment.NewLine)
                    .Where(y => y.Length > 0)
                    .Select(y =>
                    {
                        var values = y.Split(' ').Select(long.Parse).ToArray();
                        return new Map(values[0], values[1], values[2]);
                    }).ToList()
                }).ToList();
            foreach (var seed in seeds)
            {
                var throughput = seed.Id;
                foreach(var mapGroup in mapgroups)
                {
                    var map = mapGroup.Maps.Find(x => x.ContainsSource(throughput));
                    if (map == null)
                        continue;
                    throughput = map.Evaluate(throughput);
                }
                seed.Location = throughput;
            }

            return seeds.Min(x => x.Location).ToString();
        }

        internal record Map(long Start, long SourceStart, long Range)
        {
            public bool ContainsSource(long source) => SourceStart <= source && SourceStart + Range >= source;
            public long Evaluate(long value) => (Start + Range) - (SourceStart + Range - value);
        }
        internal record Seed(long Id)
        {
            public long Location { get; set;}
        }
    }
}
