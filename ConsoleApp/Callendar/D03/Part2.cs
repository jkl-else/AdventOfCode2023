using System.Drawing;
using System.Text;

namespace ConsoleApp.Callendar.D03
{
    internal class Part2 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input2");//Result = 84495585 - Result in: 00:00:00.0569643
            HashSet<(char Char, Point Coordinate)> gears = input.SelectMany((line, y) =>
                    line.Select((c, x) => (c, new Point(x, y)))
                        .Where(o => o.c == '*'))
                .ToHashSet();
            var numbers = input.SelectMany(GetNumbers).ToHashSet();
            var result = new List<int>();
            foreach (var gear in gears)
            {
                var adjecentNumbers = numbers.Where(x =>
                    gear.Coordinate.X >= x.Coordinate.X -1 && gear.Coordinate.X <= x.Coordinate.X + x.Length
                    && gear.Coordinate.Y >= x.Coordinate.Y - 1 && gear.Coordinate.Y <= x.Coordinate.Y + 1)
                    .ToList();
                if (adjecentNumbers.Count == 2)
                    result.Add(adjecentNumbers[0].Value * adjecentNumbers[1].Value);
            }
            return result.Sum().ToString();

            IEnumerable<(int Value, Point Coordinate, int Length)> GetNumbers(string line, int y)
            {
                StringBuilder value = new();
                for (var i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))
                    {
                        value.Append(line[i]);
                        continue;
                    }

                    if (value.Length == 0) continue;
                    yield return (int.Parse(value.ToString()), new Point(i - value.Length, y), value.Length);
                    value.Clear();
                }
                if (value.Length > 0)
                    yield return (int.Parse(value.ToString()), new Point(line.Length - value.Length, y), value.Length);
            }
        }
    }
}
