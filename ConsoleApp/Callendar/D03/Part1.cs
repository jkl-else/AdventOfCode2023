using System.Drawing;
using System.Text;

namespace ConsoleApp.Callendar.D03
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input1");
            HashSet<(char Char, Point Coordinate)> symbols = input.SelectMany((line, y) =>
                    line.Select((c, x) => (c, new Point(x, y)))
                        .Where(o => !char.IsDigit(o.c) && o.c != '.'))
                .ToHashSet();
            var numbers = input.SelectMany(GetNumbers).ToHashSet();
            var result = new List<int>();
            foreach (var number in numbers)
            {
                var adjecentSymbol = symbols.Where(x =>
                    x.Coordinate.X >= number.Coordinate.X - 1 && x.Coordinate.X <= number.Coordinate.X + number.Length
                    && x.Coordinate.Y >= number.Coordinate.Y - 1 && x.Coordinate.Y <= number.Coordinate.Y + 1);
                if (adjecentSymbol.Any())
                    result.Add(number.Value);
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
