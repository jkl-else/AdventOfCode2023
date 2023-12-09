namespace ConsoleApp.Callendar.D09
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input1");//Result = 1782868781 - Result in: 00:00:00.0264136
            var data = input.Select(x => x.Split(' ').Select(int.Parse).ToList()).ToList();
            var result = 0;
            foreach (var line in data)
            {
                var itterations = new List<List<int>>();
                var currentItteration = line;
                while (true)
                {
                    itterations.Add(currentItteration);
                    currentItteration = Itterate(currentItteration).ToList();
                    if (currentItteration.All(x => x == 0))
                        break;
                }

                var addition = 0;
                for (var i = itterations.Count - 1; i > -1; i--)
                {
                    addition = itterations[i][^1] + addition;
                }
                result += addition;
            }
            return result.ToString();
        }

        private IEnumerable<int> Itterate(IEnumerable<int> list)
        {
            int? value = null;
            foreach (var item in list)
            {
                if (!value.HasValue)
                {
                    value = item;
                    continue;
                }

                yield return item - value.Value;
                value = item;
            }
        }
    }
}
