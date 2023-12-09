using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace ConsoleApp.Callendar.D08
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input1");//Result = 16697 - Result in: 00:00:00.0219262
            var reg = new Regex("^(?<start>[A-Z]{3}) = \\((?<left>[A-Z]{3}), (?<right>[A-Z]{3})\\)$");
            var directions = input[0].ToList();
            var data = input.Skip(2)
                .Select(x => reg.Match(x))
                .Select(x => new
                {
                    Start = x.Groups["start"].Value,
                    Left = x.Groups["left"].Value,
                    Right = x.Groups["right"].Value
                }).ToFrozenDictionary(x => x.Start, x => (x.Left, x.Right));

            return Move(directions, data).ToString();
        }

        private static int Move(List<char> directions, FrozenDictionary<string, (string Left, string Right)> data)
        {
            var pos = "AAA";
            var steps = 0;
            while (true)
            {
                foreach (var direction in directions)
                {
                    steps++;
                    pos = direction == 'L'
                        ? data[pos].Left
                        : data[pos].Right;
                    if (pos == "ZZZ")
                        return steps;
                }
            }
        }
    }
}
