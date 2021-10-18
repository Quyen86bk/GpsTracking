using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.GpsTrackingModule.Data._Migrations
{
    public partial class new26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventName",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "EventTime",
                table: "Event");

            migrationBuilder.AlterColumn<Guid>(
                name: "LocationId",
                table: "Event",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "GeofenceId",
                table: "Event",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GpsDeviceId",
                table: "Event",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Event_GeofenceId",
                table: "Event",
                column: "GeofenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_GpsDeviceId",
                table: "Event",
                column: "GpsDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LocationId",
                table: "Event",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Geofence_GeofenceId",
                table: "Event",
                column: "GeofenceId",
                principalTable: "Geofence",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_GpsDevice_GpsDeviceId",
                table: "Event",
                column: "GpsDeviceId",
                principalTable: "GpsDevice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Location_LocationId",
                table: "Event",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Geofence_GeofenceId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_GpsDevice_GpsDeviceId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_Location_LocationId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_GeofenceId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_GpsDeviceId",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Event_LocationId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "GpsDeviceId",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Event");

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
                table: "NotificatorMapping",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Notificator",
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

            migrationBuilder.AlterColumn<string>(
                name: "LocationId",
                table: "Event",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "GeofenceId",
                table: "Event",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Event",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventTime",
                table: "Event",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Command",
                nullable: false,
                defaultValue: "");
        }
    }
}
