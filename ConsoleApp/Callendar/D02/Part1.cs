namespace ConsoleApp.Callendar.D02
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input1");//Result = 2551 - Result in: 00:00:00.0441129
            var requirement = new Dictionary<string, int>
            {
                ["red"] = 12,
                ["green"] = 13,
                ["blue"] = 14
            };
            var list = input.Select(x => new
            {
                Id = ParseGameId(x, out var left),
                Games = left
                    .Trim()
                    .Split(';')
                    .Select(y => y.Split(',').Select(z => ParseGameItem(z.TrimStart())).ToList())
                    .ToList()
            });

            return list.Where(x =>
                x.Games.All(y =>
                    y.All(z => !requirement.TryGetValue(z.Color, out var r) // color not required
                        || z.Number <= r)))
                .Select(x => x.Id)
                .Sum()
                .ToString();

            (int Number, string Color) ParseGameItem(string value)
            {
                var stopIndex = value.IndexOf(' ');
                var left = value[(stopIndex + 1)..];
                return (int.Parse(value[..stopIndex]), left);
            }

            int ParseGameId(string value, out string left)
            {
                var stopIndex = value.IndexOf(':');
                left = value[(stopIndex + 1)..];
                return int.Parse(value["Game ".Length..stopIndex]);
            }
        }
    }
}
