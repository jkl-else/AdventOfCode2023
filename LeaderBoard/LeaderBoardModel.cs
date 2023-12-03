using System.Text.Json.Serialization;

namespace LeaderBoard
{
    /// <summary>
    /// Model for api
    /// </summary>
    internal class LeaderBoardModel
    {
        [JsonPropertyName("owner_id")]
        public int OwnerId { get; set; }
        [JsonPropertyName("event")]
        public int Event { get; set; }
        [JsonPropertyName("members")]
        public Dictionary<int, MemberModel> Members { get; set; } = new();

        internal class MemberModel
        {
            [JsonPropertyName("global_score")]
            public int GlobalScore { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; } = null!;
            [JsonPropertyName("id")]
            public int Id { get; set; }
            [JsonPropertyName("completion_day_level")]
            public Dictionary<int, Dictionary<int, SolutionModel>> CompletionDayLevel { get; set; } = new();
            [JsonPropertyName("local_score")]
            public int LocalScore { get; set; }
            [JsonPropertyName("stars")]
            public long Stars { get; set; }
            [JsonPropertyName("last_star_ts")]
            public long LastStarTs { get; set; }

            internal class SolutionModel
            {
                [JsonPropertyName("get_star_ts")]
                public long GetStarTs { get; set; }
                [JsonPropertyName("star_index")]
                public long StarIndex { get; set; }
            }
        }
    }
}
