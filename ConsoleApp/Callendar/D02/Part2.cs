namespace ConsoleApp.Callendar.D02
{
    internal class Part2 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input2");
            var list = input.Select(x => new
            {
                Id = int.Parse(GetGameId(x, out var left)),
                Games = left
                    .Trim()
                    .Split(';')
                    .Select(y => y.Split(',')
                            .Select(z => new {
                                Number = int.Parse(GetFirstNumber(z.Trim(), out var left2)),
                                Color = left2.Trim()
                            }).ToDictionary(z => z.Color, z => z.Number)
                        ).ToList()
            }).ToList();
            var colors = new List<string>
            {
                "red",
                "green",
                "blue"
            };
            var result = new List<Dictionary<string, int>>();
            foreach (var gameId in list)
            {
                var s = new Dictionary<string, int>();
                foreach (var color in colors)
                {
                    int max = 0;
                    foreach (var game in gameId.Games)
                    {
                        if (!game.TryGetValue(color, out var n))
                            continue;
                        if (n > max)
                            max = n;
                    }
                    s.Add(color, max);
                }
                result.Add(s);
            }

            int res = 0;
            foreach (var r in result)
            {
                int game = 1;
                foreach (var c in r)
                {
                    game *= c.Value;
                }
                res += game;
            }
            return res.ToString();
        }

        private static string GetGameId(string value, out string left)
        {
            var stopIndex = value.IndexOf(':');
            left = value[(stopIndex + 1)..];
            return value["Game ".Length..stopIndex];
        }

        private static string GetFirstNumber(string value, out string left)
        {
            var stopIndex = value.IndexOf(' ');
            left = value[(stopIndex + 1)..];
            return value[..stopIndex];
        }
    }
}
