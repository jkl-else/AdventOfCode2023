namespace ConsoleApp.Callendar.D07
{
    internal class Part2 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input2");//Result = 251037509 - Result in: 00:00:00.0248639
            return input.Select(x =>
            {
                var row = x.Split(' ');
                return new Hand(row[0], row[1]);
            }).GroupBy(x => x.Points)
            .OrderBy(x => x.Key)
            .SelectMany(x => x.OrderBy(hand => hand.SortValue))
            .Select((hand, i) => hand.Bid * (i + 1))
            .Sum()
            .ToString();
        }

        internal class Hand
        {
            public int Bid { get; }
            public int Points { get; }
            public string SortValue { get; }
            public int Jokers { get; }
            public Hand(string cards, string bid)
            {
                Bid = int.Parse(bid);
                var converter = new[] { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };
                var cardValues = cards.Select(c => Array.IndexOf(converter, c)).ToList();
                SortValue = string.Join("", cardValues.Select(x => x.ToString("00")));
                Jokers = cardValues.Count(c => c == 0);
                var ofKinds = cardValues.Where(c => c > 0) // No Jokers
                    .GroupBy(x => x)
                    .Select(x => x.Count())
                    .OrderByDescending(x => x)
                    .ToList();
                var highestKinds = ofKinds.Count > 0 ? ofKinds[0] : 0;
                var secondHighestKinds = ofKinds.Count > 1 ? ofKinds[1] : 0;
                switch (highestKinds + Jokers)
                {
                    case >= 5:// Five of a Kind
                        Points = 7; break;
                    case 4:// Four of a Kind (- 3 Jokers)
                        Points = 6; break;
                    case 3 when secondHighestKinds == 2:// Full House (2 of kind + 2 jokers = four of a kind)
                        Points = 5; break;
                    case 3:// Three of a Kind (- 2 Jokers)
                        Points = 4; break;
                    default:
                    {
                        if (highestKinds == 2 && secondHighestKinds + Jokers == 2) // Two Pair (2 Jokers will make 3 of a kind)
                            Points = 2;
                        else if (highestKinds + Jokers == 2) // Pair
                            Points = 1;
                        break;
                    }
                }
            }
        }
    }
}
