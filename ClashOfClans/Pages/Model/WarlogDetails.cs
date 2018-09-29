using System.Collections.Generic;
using JetBrains.Annotations;

namespace ClashOfClans.Pages.Model
{
    [UsedImplicitly]
    public class WarlogDetails
    {
        public IReadOnlyList<WarlogDetail> Items { get; set; }
    }

    [UsedImplicitly]
    public class WarlogDetail
    {
        public WarResult Result { get; set; }
        public string EndTime { get; set; }
        public int TeamSize { get; set; }
        public ClanDetails Clan { get; set; }
        public ClanDetails Opponent { get; set; }
    }

    public enum WarResult
    {
        Win,
        Lose,
        Tie,
    }
}