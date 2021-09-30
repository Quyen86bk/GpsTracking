using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.GpsTrackingModule.Data._Migrations
{
    public partial class new9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GpsDeviceId",
                table: "Location",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Location_GpsDeviceId",
                table: "Location",
                column: "GpsDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_GpsDevice_GpsDeviceId",
                table: "Location",
                column: "GpsDeviceId",
                principalTable: "GpsDevice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_GpsDevice_GpsDeviceId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_GpsDeviceId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Course",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "DeviceTime",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "GpsDeviceId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Protocol",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "GpsDevice");

            migrationBuilder.DropColumn(
                name: "Geofences",
                table: "GpsDevice");

            migrationBuilder.DropColumn(
                name: "Notifications",
                table: "GpsDevice");

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

            migrationBuilder.AddColumn<float>(
                name: "X",
                table: "Location",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Y",
                table: "Location",
                nullable: false,
                defaultValue: 0f);

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
                name: "Discriminator",
                table: "Geofence",
                nullable: false,
                defaultValue: "");

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
