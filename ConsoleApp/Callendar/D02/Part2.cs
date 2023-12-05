namespace ConsoleApp.Callendar.D02
{
    internal class Part2 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input2");//Result = 62811 - Result in: 00:00:00.0169265
            var list = input.Select(x => new
            {
                Id = ParseGameId(x, out var left),
                Games = left
                    .Trim()
                    .Split(';')
                    .Select(y => y.Split(',').Select(z => ParseGameItem(z.TrimStart()))
                    .ToDictionary(z => z.Color, z => z.Number))
                    .ToList()
            });
            var colors = new List<string>
            {
                "red",
                "green",
                "blue"
            };
            return list
                .Select(gameId => colors
                    .ToDictionary(color => color, color =>
                        gameId.Games.Max(game => game.TryGetValue(color, out var n) ? n : 0)))
                .Select(r => r.Values.Aggregate(1, (acc, val) => acc * val))
                .Sum()
                .ToString();

            int ParseGameId(string value, out string left)
            {
                var stopIndex = value.IndexOf(':');
                left = value[(stopIndex + 1)..];
                return int.Parse(value["Game ".Length..stopIndex]);
            }

            (int Number, string Color) ParseGameItem(string value)
            {
                var stopIndex = value.IndexOf(' ');
                var left = value[(stopIndex + 1)..];
                return (int.Parse(value[..stopIndex]), left);
            }
        }
    }
}
