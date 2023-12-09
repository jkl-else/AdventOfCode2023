using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace ConsoleApp.Callendar.D08
{
    internal class Part2 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input2"); //Result = 10668805667831 - Result in: 00:00:13.6460141
            var reg = new Regex("^(?<start>[A-Z0-9]{3}) = \\((?<left>[A-Z0-9]{3}), (?<right>[A-Z0-9]{3})\\)$");
            var dir = input[0].ToList();
            var data = input.Skip(2)
                .Select(x => reg.Match(x))
                .Select(x => new
                {
                    Start = x.Groups["start"].Value,
                    Left = x.Groups["left"].Value,
                    Right = x.Groups["right"].Value
                }).ToFrozenDictionary(x => x.Start, x => (x.Left, x.Right));
            var positions = data.Keys.Where(x => x.EndsWith('A'))
                .Select(x => new Position(x, data, dir))
                .ToList();
            long steps = 0;
            var maxSteps = positions.Max(x => x.Steps);
            while (true)
            {
                steps += maxSteps;
                if (positions.All(x => (double)steps / x.Steps % 1 == 0))
                    break;
            }

            return steps.ToString();
        }

        internal class Position
        {
            private string _pos;
            private readonly FrozenDictionary<string, (string Left, string Right)> _map;
            private readonly List<char> _dirs;
            public int Steps { get; }
            public Position(string pos, FrozenDictionary<string, (string Left, string Right)> map, List<char> dirs)
            {
                _pos = pos;
                _map = map;
                _dirs = dirs;
                Steps = CalculateSteps();
            }

            private int CalculateSteps()
            {
                int steps = 0;
                while (true)
                {
                    foreach (var dir in _dirs)
                    {
                        steps++;
                        _pos = dir == 'L'
                            ? _map[_pos].Left
                            : _map[_pos].Right;
                        if (_pos.EndsWith('Z'))
                            return steps;
                    }
                }
            }
        }
    }
}
