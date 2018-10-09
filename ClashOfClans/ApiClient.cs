using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClashOfClans.Models;
using ClashOfClans.Pages;
using ClashOfClans.Pages.Model;

namespace ClashOfClans
{
    public static class ApiClient
    {
        public static async Task<ClanSnapshot> GetClanSnapshot(string tag)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6IjhiMjQwMTI5LTYzNTMtNDRkOC05ZDNmLWZkN2E2ZWU5ZDlkOSIsImlhdCI6MTUzODE2MjY4Mywic3ViIjoiZGV2ZWxvcGVyLzc3OTFkNTU4LTRhOTgtY2I3NS1kNjJkLWIxNTdiMThiN2E0MyIsInNjb3BlcyI6WyJjbGFzaCJdLCJsaW1pdHMiOlt7InRpZXIiOiJkZXZlbG9wZXIvc2lsdmVyIiwidHlwZSI6InRocm90dGxpbmcifSx7ImNpZHJzIjpbIjE5NS4xMzMuNzMuMjMiLCI0Ni4zOS4yNDguNjAiXSwidHlwZSI6ImNsaWVudCJ9XX0.kdDD02LNuzY2VY8pKzFmIoVzYQXWlxNBtr_5oiGjXO3NF7NWwitypb7YdvXZGxGPcVSIpIRofLZ0s3Psb8fzhA");
                var clan = await httpClient.GetAsync($"https://api.clashofclans.com/v1/clans/{tag}");
                clan.EnsureSuccessStatusCode();
                var clanDetails = await clan.Content.ReadAsAsync<ClanDetails>();

                var now = DateTimeOffset.UtcNow;
                var snapshot = new ClanSnapshot
                {
                    Tag = clanDetails.Tag,
                    Name = clanDetails.Name,
                    LocationName = clanDetails.Location.Name,
                    LocationIsCountry = clanDetails.Location.IsCountry,
                    BadgeUrlSmall = clanDetails.BadgeUrls.Small,
                    BadgeUrlLarge = clanDetails.BadgeUrls.Large,
                    BadgeUrlMedium = clanDetails.BadgeUrls.Medium,
                    ClanLevel = clanDetails.ClanLevel,
                    ClanPoints = clanDetails.ClanPoints,
                    ClanVersusPoints = clanDetails.ClanVersusPoints,
                    Members = clanDetails.Members,
                    Type = clanDetails.Type,
                    RequiredTrophies = clanDetails.RequiredTrophies,
                    WarFrequency = clanDetails.WarFrequency,
                    WarWinStreak = clanDetails.WarWinStreak,
                    WarWins = clanDetails.WarWins,
                    WarTies = clanDetails.WarTies,
                    WarLosses = clanDetails.WarLosses,
                    IsWarLogPublic = clanDetails.IsWarLogPublic,
                    Description = clanDetails.Description,
                    MemberList = PopulateMemberList(clanDetails.MemberList, now),
                    SnapshotTime = now
                };
                return snapshot;
            }
        }

        private static List<PlayerSnapshot> PopulateMemberList(IReadOnlyList<Member> memberList, DateTimeOffset now)
        {
            return memberList.Select(member => new PlayerSnapshot
                {
                    FullSnapshot = false,
                    Tag = member.Tag,
                    Name = member.Name,
                    ExpLevel = member.ExpLevel,
                    LeagueName = member.League.Name,
                    LeagueSmallIcon = member.League.IconUrls.Small,
                    LeagueLargeIcon = member.League.IconUrls.Large,
                    LeagueMediumIcon = member.League.IconUrls.Medium,
                    Trophies = member.Trophies,
                    VersusTrophies = member.VersusTrophies,
                    Role = member.Role,
                    ClanRank = member.ClanRank,
                    PreviousClanRank = member.PreviousClanRank,
                    Donations = member.Donations,
                    DonationsReceived = member.DonationsReceived,
                    SnapshotTime = now
                })
                .ToList();
        }
    }
}