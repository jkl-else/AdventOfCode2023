namespace ConsoleApp.Callendar.D07
{
    internal class Part1 : Part
    {
        public override async Task<string> GetResultAsync()
        {
            var input = await ReadFileLinesAsync("Input1");//Result = 250602641 - Result in: 00:00:00.0633490
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
            public int Bid { get;}
            public int Points { get;}
            public string SortValue { get; }
            public Hand(string cards, string bid)
            {
                Bid = int.Parse(bid);
                var converter = new[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
                var cardValues = cards.Select(c => Array.IndexOf(converter, c)).ToList();
                SortValue = string.Join("", cardValues.Select(x => x.ToString("00")));
                var ofKinds = cardValues.GroupBy(x => x)
                    .Select(x => x.Count())
                    .OrderByDescending(x => x)
                    .ToList();
                Points = GetPoints(ofKinds[0], ofKinds.Count > 1 ? ofKinds[1] : 0);

                static int GetPoints(int ofKinds1, int ofKinds2) =>
                    ofKinds1 switch
                    {
                        5 => 7,
                        4 => 6,
                        3 when ofKinds2 == 2 => 5,
                        3 => 4,
                        2 when ofKinds2 == 2 => 2,
                        2 => 1,
                        _ => 0,
                    };
            }
        }
    }
}
