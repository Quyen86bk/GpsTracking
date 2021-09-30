using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.GpsTrackingModule.Data._Migrations
{
    public partial class new13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeofenceMapping",
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
                    ProfileInfoId = table.Column<Guid>(nullable: true),
                    GeofenceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeofenceMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeofenceMapping_Geofence_GeofenceId",
                        column: x => x.GeofenceId,
                        principalTable: "Geofence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GeofenceMapping_ProfileInfo_ProfileInfoId",
                        column: x => x.ProfileInfoId,
                        principalTable: "ProfileInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GpsDeviceMapping",
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
                    ProfileInfoId = table.Column<Guid>(nullable: true),
                    GpsDeviceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpsDeviceMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GpsDeviceMapping_GpsDevice_GpsDeviceId",
                        column: x => x.GpsDeviceId,
                        principalTable: "GpsDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GpsDeviceMapping_ProfileInfo_ProfileInfoId",
                        column: x => x.ProfileInfoId,
                        principalTable: "ProfileInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "NotificationMapping",
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
                    ProfileInfoId = table.Column<Guid>(nullable: true),
                    NotificationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationMapping_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_NotificationMapping_ProfileInfo_ProfileInfoId",
                        column: x => x.ProfileInfoId,
                        principalTable: "ProfileInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notification_ProfileInfoId",
            //    table: "Notification",
            //    column: "ProfileInfoId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Geofence_ProfileInfoId",
            //    table: "Geofence",
            //    column: "ProfileInfoId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_GeofenceMapping_GeofenceId",
            //    table: "GeofenceMapping",
            //    column: "GeofenceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_GeofenceMapping_ProfileInfoId",
            //    table: "GeofenceMapping",
            //    column: "ProfileInfoId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_GpsDeviceMapping_GpsDeviceId",
            //    table: "GpsDeviceMapping",
            //    column: "GpsDeviceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_GpsDeviceMapping_ProfileInfoId",
            //    table: "GpsDeviceMapping",
            //    column: "ProfileInfoId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_NotificationMapping_NotificationId",
            //    table: "NotificationMapping",
            //    column: "NotificationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_NotificationMapping_ProfileInfoId",
            //    table: "NotificationMapping",
            //    column: "ProfileInfoId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Geofence_ProfileInfo_ProfileInfoId",
            //    table: "Geofence",
            //    column: "ProfileInfoId",
            //    principalTable: "ProfileInfo",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.NoAction);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Notification_ProfileInfo_ProfileInfoId",
            //    table: "Notification",
            //    column: "ProfileInfoId",
            //    principalTable: "ProfileInfo",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Geofence_ProfileInfo_ProfileInfoId",
                table: "Geofence");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_ProfileInfo_ProfileInfoId",
                table: "Notification");

            migrationBuilder.DropTable(
                name: "GeofenceMapping");

            migrationBuilder.DropTable(
                name: "GpsDeviceMapping");

            migrationBuilder.DropTable(
                name: "NotificationMapping");

            migrationBuilder.DropIndex(
                name: "IX_Notification_ProfileInfoId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Geofence_ProfileInfoId",
                table: "Geofence");

            migrationBuilder.DropColumn(
                name: "ProfileInfoId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "ProfileInfoId",
                table: "Geofence");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ProfileInfo",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "GeofenceId",
                table: "ProfileInfo",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GpsDeviceId",
                table: "ProfileInfo",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "ProfileInfo",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInfo_GeofenceId",
                table: "ProfileInfo",
                column: "GeofenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInfo_GpsDeviceId",
                table: "ProfileInfo",
                column: "GpsDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInfo_NotificationId",
                table: "ProfileInfo",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInfo_Geofence_GeofenceId",
                table: "ProfileInfo",
                column: "GeofenceId",
                principalTable: "Geofence",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInfo_GpsDevice_GpsDeviceId",
                table: "ProfileInfo",
                column: "GpsDeviceId",
                principalTable: "GpsDevice",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInfo_Notification_NotificationId",
                table: "ProfileInfo",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
