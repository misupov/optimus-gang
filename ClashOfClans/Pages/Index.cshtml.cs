﻿using System;
using System.IO.Enumeration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClashOfClans.Pages.Model;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClashOfClans.Pages
{
    public class IndexModel : PageModel
    {
        [UsedImplicitly]
        public async Task OnGet()
        {
            using (var httpClient = new HttpClient())
            {
                var id = "Q8GRU2LR";

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6IjhiMjQwMTI5LTYzNTMtNDRkOC05ZDNmLWZkN2E2ZWU5ZDlkOSIsImlhdCI6MTUzODE2MjY4Mywic3ViIjoiZGV2ZWxvcGVyLzc3OTFkNTU4LTRhOTgtY2I3NS1kNjJkLWIxNTdiMThiN2E0MyIsInNjb3BlcyI6WyJjbGFzaCJdLCJsaW1pdHMiOlt7InRpZXIiOiJkZXZlbG9wZXIvc2lsdmVyIiwidHlwZSI6InRocm90dGxpbmcifSx7ImNpZHJzIjpbIjE5NS4xMzMuNzMuMjMiLCI0Ni4zOS4yNDguNjAiXSwidHlwZSI6ImNsaWVudCJ9XX0.kdDD02LNuzY2VY8pKzFmIoVzYQXWlxNBtr_5oiGjXO3NF7NWwitypb7YdvXZGxGPcVSIpIRofLZ0s3Psb8fzhA");
                var clan = await httpClient.GetAsync($"https://api.clashofclans.com/v1/clans/%23{id}");
                clan.EnsureSuccessStatusCode();
                var clanDetails = await clan.Content.ReadAsAsync<ClanDetails>();
                clanDetails.BadgeUrls = FixUrls(clanDetails.BadgeUrls);
                foreach (var member in clanDetails.MemberList)
                {
                    member.League.IconUrls = FixUrls(member.League.IconUrls);
                }
                ClanDetails = clanDetails;
                var warlog = await httpClient.GetAsync($"https://api.clashofclans.com/v1/clans/%23{id}/warlog");
                warlog.EnsureSuccessStatusCode();
                var warlogDetails = await warlog.Content.ReadAsAsync<WarlogDetails>();
                foreach (var detail in warlogDetails.Items)
                {
                    detail.Clan.BadgeUrls = FixUrls(detail.Clan.BadgeUrls);
                }
                foreach (var detail in warlogDetails.Items)
                {
                    detail.Opponent.BadgeUrls = FixUrls(detail.Opponent.BadgeUrls);
                }
                WarlogDetails = warlogDetails;
            }
        }

        private IconUrls FixUrls(IconUrls urls)
        {
            return FixUrl.Fix(urls);
        }

        public ClanDetails ClanDetails { get; private set; }

        public WarlogDetails WarlogDetails { get; private set; }
    }
}