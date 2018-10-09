using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClashOfClans.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClanSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tag = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    LocationName = table.Column<string>(nullable: true),
                    LocationIsCountry = table.Column<bool>(nullable: false),
                    BadgeUrlSmall = table.Column<string>(nullable: true),
                    BadgeUrlLarge = table.Column<string>(nullable: true),
                    BadgeUrlMedium = table.Column<string>(nullable: true),
                    ClanLevel = table.Column<int>(nullable: false),
                    ClanPoints = table.Column<int>(nullable: false),
                    ClanVersusPoints = table.Column<int>(nullable: false),
                    Members = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    RequiredTrophies = table.Column<int>(nullable: false),
                    WarFrequency = table.Column<string>(nullable: true),
                    WarWinStreak = table.Column<int>(nullable: false),
                    WarWins = table.Column<int>(nullable: false),
                    WarTies = table.Column<int>(nullable: false),
                    WarLosses = table.Column<int>(nullable: false),
                    IsWarLogPublic = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SnapshotTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullSnapshot = table.Column<bool>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ExpLevel = table.Column<int>(nullable: false),
                    LeagueName = table.Column<string>(nullable: true),
                    LeagueSmallIcon = table.Column<string>(nullable: true),
                    LeagueLargeIcon = table.Column<string>(nullable: true),
                    LeagueMediumIcon = table.Column<string>(nullable: true),
                    Trophies = table.Column<int>(nullable: false),
                    VersusTrophies = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    ClanRank = table.Column<int>(nullable: false),
                    PreviousClanRank = table.Column<int>(nullable: false),
                    Donations = table.Column<int>(nullable: false),
                    DonationsReceived = table.Column<int>(nullable: false),
                    BestTrophies = table.Column<int>(nullable: true),
                    BestVersusTrophies = table.Column<int>(nullable: true),
                    AttackWins = table.Column<int>(nullable: true),
                    DefenseWins = table.Column<int>(nullable: true),
                    WarStars = table.Column<int>(nullable: true),
                    TownHallLevel = table.Column<int>(nullable: true),
                    BuilderHallLevel = table.Column<int>(nullable: true),
                    VersusBattleWins = table.Column<int>(nullable: true),
                    SnapshotTime = table.Column<DateTimeOffset>(nullable: false),
                    ClanSnapshotId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSnapshots_ClanSnapshots_ClanSnapshotId",
                        column: x => x.ClanSnapshotId,
                        principalTable: "ClanSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSnapshots_ClanSnapshotId",
                table: "PlayerSnapshots",
                column: "ClanSnapshotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerSnapshots");

            migrationBuilder.DropTable(
                name: "ClanSnapshots");
        }
    }
}
