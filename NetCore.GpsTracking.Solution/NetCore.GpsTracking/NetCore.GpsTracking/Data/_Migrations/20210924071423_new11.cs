using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.GpsTrackingModule.Data._Migrations
{
    public partial class new11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeofenceDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataId = table.Column<Guid>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedById = table.Column<Guid>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    IsActived = table.Column<bool>(nullable: true),
                    DeletedById = table.Column<Guid>(nullable: true),
                    DeletedTime = table.Column<DateTime>(nullable: true),
                    GeofenceId = table.Column<Guid>(nullable: false),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeofenceDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeofenceDetail_Geofence_GeofenceId",
                        column: x => x.GeofenceId,
                        principalTable: "Geofence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeofenceDetail_GeofenceId",
                table: "GeofenceDetail",
                column: "GeofenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeofenceDetail");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Permission",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Notification",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Location",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Group",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "GpsTrackingNameValue",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "GpsTrackingMany",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "GpsDevice",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Geofence",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Geofence",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Geofence",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Event",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Command",
                nullable: false,
                defaultValue: "");
        }
    }
}
