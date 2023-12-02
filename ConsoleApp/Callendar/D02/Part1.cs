namespace ConsoleApp.Callendar.D02
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input1");
            string GetGameId(string value, out string left)
            {
                var stopIndex = value.IndexOf(':');
                left = value[(stopIndex + 1)..];
                return value["Game ".Length..stopIndex];
            }
            string GetFirstNumber(string value, out string left)
            {
                var stopIndex = value.IndexOf(' ');
                left = value[(stopIndex + 1)..];
                return value[..stopIndex];
            }

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
                            }).ToList()
                    ).ToList()
            }).ToList();
            var requirement = new Dictionary<string, int>
            {
                ["red"] = 12,
                ["green"] = 13,
                ["blue"] = 14
            };

            var ids = list.Where(x =>
                x.Games.All(y =>
                    y.All(z => !requirement.TryGetValue(z.Color, out var r) // color not required
                        || z.Number <= r)))
                .Select(x => x.Id)
                .ToList();
            return ids.Sum().ToString();
        }
    }
}
