using System.Collections.Frozen;

namespace ConsoleApp.Callendar.D04
{
    internal class Part2 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input2");//5659035
            var cards = input.Select((x, i) => new
            {
                Card = x.Split(':')[1][1..].Split('|')
                    .Select(r => r.Split(' ').Where(n => n.Length > 0).Select(int.Parse).ToList())
                    .ToList(),
                Id = i
            }).ToFrozenDictionary(x => x.Id, x => new Card(x.Id, x.Card[0], x.Card[1]));
            Queue<Card> pile = new(cards.Values);
            var totalCards = pile.Count;
            while (pile.TryDequeue(out var card))
            {
                foreach (var newCard in GetNewCards(cards, card.ScratchResult))
                {
                    pile.Enqueue(newCard);
                    totalCards++;
                }
            }
            return totalCards.ToString();
            IEnumerable<Card> GetNewCards(IDictionary<int, Card> deck, IEnumerable<int> cardIds) => cardIds.Select(x => deck[x]);
        }

        internal record Card(int Id, List<int> WinningNumbers, List<int> Numbers)
        {
            public List<int> ScratchResult { get;} = Enumerable.Range(Id + 1, Numbers.Count(WinningNumbers.Contains)).ToList();
        }
    }
}
