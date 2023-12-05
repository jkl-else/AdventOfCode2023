namespace ConsoleApp.Callendar.D01
{
    internal class Part2 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input2");//Result = 54845 - Result in: 00:00:00.0678899
            var values = new Dictionary<string, char>
            {
                ["one"] = '1',
                ["two"] = '2',
                ["three"] = '3',
                ["four"] = '4',
                ["five"] = '5',
                ["six"] = '6',
                ["seven"] = '7',
                ["eight"] = '8',
                ["nine"] = '9',
                ["zero"] = '0',
            };
            return input.Select(x => new
            {
                FirstNumber = x
                    .Select((c, i) => new
                    {
                        Digit = char.IsDigit(c), // for filter
                        Value = c,
                        Index = i // index for order compared to text
                    })
                    .FirstOrDefault(y => y.Digit),
                LastNumber = x
                    .Select((c, i) => new
                    {
                        Digit = char.IsDigit(c),
                        Value = c,
                        Index = i
                    })
                    .LastOrDefault(y => y.Digit),
                FirstText = values.Keys.Where(x.Contains)
                    .Select(v => new
                    {
                        Value = values[v],
                        Index = x.ToLower().IndexOf(v, StringComparison.Ordinal)
                    }).MinBy(v => v.Index),
                LastText = values.Keys.Where(x.Contains)
                    .Select(v => new
                    {
                        Value = values[v],
                        Index = x.ToLower().LastIndexOf(v, StringComparison.Ordinal)
                    }).MaxBy(v => v.Index),
            })
            .Select(x => new string(new[] {
                x.FirstNumber == null && x.FirstText == null
                    ? throw new Exception("Can't find First") // Need to filter?
                    : x.FirstNumber == null
                        ? x.FirstText!.Value // no number, only text
                        : x.FirstText == null
                            ? x.FirstNumber!.Value // no text, only number
                            : x.FirstNumber.Index < x.FirstText.Index ? x.FirstNumber.Value : x.FirstText.Value, // order by Index
                x.LastNumber == null && x.LastText == null
                    ? throw new Exception("Can't find Last") // Need to filter?
                    : x.LastNumber == null
                        ? x.LastText!.Value // no number, only text
                        : x.LastText == null
                            ? x.LastNumber!.Value // no text, only number
                            : x.LastNumber.Index > x.LastText.Index ? x.LastNumber.Value : x.LastText.Value // order by Index
                            }))
                .Select(int.Parse)
                .Sum()
                .ToString();
        }
    }
}
