using System.Collections.Frozen;

namespace ConsoleApp.Callendar.D04
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input1");//24160
            var cards = input.Select((x, i) => new
            {
                Card = x.Split(':')[1][1..].Split('|')
                    .Select(r => r.Split(' ').Where(n => n.Length > 0).Select(int.Parse).ToList())
                    .ToList(),
                Id = i
            }).ToFrozenDictionary(x => x.Id, x => new Card(x.Id, x.Card[0], x.Card[1]));
            var sum = 0;
            foreach (var card in cards.Values)
            {
                var winningNumbers = card.ScratchResult;
                var score = 1;
                for (int i = 1; i < winningNumbers; i++)
                    score *= 2;
                if (winningNumbers > 0)
                    sum += score;
            }
            return sum.ToString();
        }

        internal record Card(int Id, List<int> WinningNumbers, List<int> Numbers)
        {
            public int ScratchResult { get; } = Numbers.Count(WinningNumbers.Contains);
        }
    }
}
