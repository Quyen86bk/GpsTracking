using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.GpsTrackingModule.Data._Migrations
{
    public partial class new15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Geofences",
                table: "GpsDevice");

            migrationBuilder.DropColumn(
                name: "Notifications",
                table: "GpsDevice");

            migrationBuilder.AddColumn<Guid>(
                name: "GpsDeviceId",
                table: "NotificationMapping",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GpsDeviceId",
                table: "GeofenceMapping",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationMapping_GpsDeviceId",
                table: "NotificationMapping",
                column: "GpsDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_GeofenceMapping_GpsDeviceId",
                table: "GeofenceMapping",
                column: "GpsDeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeofenceMapping_GpsDevice_GpsDeviceId",
                table: "GeofenceMapping",
                column: "GpsDeviceId",
                principalTable: "GpsDevice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationMapping_GpsDevice_GpsDeviceId",
                table: "NotificationMapping",
                column: "GpsDeviceId",
                principalTable: "GpsDevice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeofenceMapping_GpsDevice_GpsDeviceId",
                table: "GeofenceMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationMapping_GpsDevice_GpsDeviceId",
                table: "NotificationMapping");

            migrationBuilder.DropIndex(
                name: "IX_NotificationMapping_GpsDeviceId",
                table: "NotificationMapping");

            migrationBuilder.DropIndex(
                name: "IX_GeofenceMapping_GpsDeviceId",
                table: "GeofenceMapping");

            migrationBuilder.DropColumn(
                name: "GpsDeviceId",
                table: "NotificationMapping");

            migrationBuilder.DropColumn(
                name: "GpsDeviceId",
                table: "GeofenceMapping");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ProfileInfo",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Permission",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "NotificationMapping",
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
                table: "GpsDeviceMapping",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "GpsDevice",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Geofences",
                table: "GpsDevice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notifications",
                table: "GpsDevice",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "GeofenceMapping",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "GeofenceDetail",
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
