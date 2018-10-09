using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace ClashOfClans.Models
{
    public class ClashContext : DbContext
    {
        public ClashContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<ClanSnapshot> ClanSnapshots { get; set; }
        public DbSet<PlayerSnapshot> PlayerSnapshots { get; set; }
    }

    public class ClanSnapshot
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string LocationName { get; set; }
        public bool LocationIsCountry { get; set; }
        public string BadgeUrlSmall { get; set; }
        public string BadgeUrlLarge { get; set; }
        public string BadgeUrlMedium { get; set; }
        public int ClanLevel { get; set; }
        public int ClanPoints { get; set; }
        public int ClanVersusPoints { get; set; }
        public int Members { get; set; }
        public string Type { get; set; }
        public int RequiredTrophies { get; set; }
        public string WarFrequency { get; set; }
        public int WarWinStreak { get; set; }
        public int WarWins { get; set; }
        public int WarTies { get; set; }
        public int WarLosses { get; set; }
        public bool IsWarLogPublic { get; set; }
        public string Description { get; set; }
        public List<PlayerSnapshot> MemberList { get; set; }
        public DateTimeOffset SnapshotTime { get; set; }

        private sealed class ClanSnapshotEqualityComparer : IEqualityComparer<ClanSnapshot>
        {
            public bool Equals(ClanSnapshot x, ClanSnapshot y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Tag, y.Tag)
                       && string.Equals(x.Name, y.Name)
                       && string.Equals(x.LocationName, y.LocationName)
                       && x.LocationIsCountry == y.LocationIsCountry
                       && string.Equals(x.BadgeUrlSmall, y.BadgeUrlSmall)
                       && string.Equals(x.BadgeUrlLarge, y.BadgeUrlLarge)
                       && string.Equals(x.BadgeUrlMedium, y.BadgeUrlMedium)
                       && x.ClanLevel == y.ClanLevel
                       && x.ClanPoints == y.ClanPoints
                       && x.ClanVersusPoints == y.ClanVersusPoints
                       && x.Members == y.Members
                       && string.Equals(x.Type, y.Type)
                       && x.RequiredTrophies == y.RequiredTrophies
                       && string.Equals(x.WarFrequency, y.WarFrequency)
                       && x.WarWinStreak == y.WarWinStreak
                       && x.WarWins == y.WarWins
                       && x.WarTies == y.WarTies
                       && x.WarLosses == y.WarLosses
                       && x.IsWarLogPublic == y.IsWarLogPublic
                       && string.Equals(x.Description, y.Description)
                       && MemberListEquals(x.MemberList, y.MemberList);
            }

            private bool MemberListEquals(IList<PlayerSnapshot> list1, IList<PlayerSnapshot> list2)
            {
                if (list1.Count != list2.Count)
                {
                    return false;
                }

                var list2Dict = list2.ToDictionary(snapshot => snapshot.Tag);

                foreach (var snapshot1 in list1)
                {
                    if (!list2Dict.TryGetValue(snapshot1.Tag, out var snapshot2) || !PlayerSnapshot.PlayerSnapshotComparer.Equals(snapshot1, snapshot2))
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(ClanSnapshot obj)
            {
                throw new NotSupportedException();
            }
        }

        public static IEqualityComparer<ClanSnapshot> ClanSnapshotComparer { get; } = new ClanSnapshotEqualityComparer();
    }

    public class PlayerSnapshot
    {
        public int Id { get; set; }
        public bool FullSnapshot { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public int ExpLevel { get; set; }
        public string LeagueName { get; set; }
        public string LeagueSmallIcon { get; set; }
        public string LeagueLargeIcon { get; set; }
        public string LeagueMediumIcon { get; set; }
        public int Trophies { get; set; }
        public int VersusTrophies { get; set; }
        public string Role { get; set; }
        public int ClanRank { get; set; }
        public int PreviousClanRank { get; set; }
        public int Donations { get; set; }
        public int DonationsReceived { get; set; }

        public int? BestTrophies { get; set; }
        public int? BestVersusTrophies { get; set; }
        public int? AttackWins { get; set; }
        public int? DefenseWins { get; set; }
        public int? WarStars { get; set; }
        public int? TownHallLevel { get; set; }
        public int? BuilderHallLevel { get; set; }
        public int? VersusBattleWins { get; set; }
        public DateTimeOffset SnapshotTime { get; set; }

        private sealed class PlayerSnapshotEqualityComparer : IEqualityComparer<PlayerSnapshot>
        {
            public bool Equals(PlayerSnapshot x, PlayerSnapshot y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null) return false;
                if (y is null) return false;
                if (x.GetType() != y.GetType()) return false;
                if (x.FullSnapshot != y.FullSnapshot) return false;

                return string.Equals(x.Tag, y.Tag)
                       && string.Equals(x.Name, y.Name)
                       && x.ExpLevel == y.ExpLevel
                       && string.Equals(x.LeagueName, y.LeagueName)
                       && string.Equals(x.LeagueSmallIcon, y.LeagueSmallIcon)
                       && string.Equals(x.LeagueLargeIcon, y.LeagueLargeIcon)
                       && string.Equals(x.LeagueMediumIcon, y.LeagueMediumIcon)
                       && x.Trophies == y.Trophies
                       && x.VersusTrophies == y.VersusTrophies
                       && string.Equals(x.Role, y.Role)
                       && x.ClanRank == y.ClanRank
                       && x.PreviousClanRank == y.PreviousClanRank
                       && x.Donations == y.Donations
                       && x.DonationsReceived == y.DonationsReceived
                       && (!x.FullSnapshot
                           || x.BestTrophies == y.BestTrophies
                           && x.BestVersusTrophies == y.BestVersusTrophies
                           && x.AttackWins == y.AttackWins
                           && x.DefenseWins == y.DefenseWins
                           && x.WarStars == y.WarStars
                           && x.TownHallLevel == y.TownHallLevel
                           && x.BuilderHallLevel == y.BuilderHallLevel
                           && x.VersusBattleWins == y.VersusBattleWins);
            }

            public int GetHashCode(PlayerSnapshot obj)
            {
                throw new NotSupportedException();
            }
        }

        public static IEqualityComparer<PlayerSnapshot> PlayerSnapshotComparer { get; } = new PlayerSnapshotEqualityComparer();
    }
}