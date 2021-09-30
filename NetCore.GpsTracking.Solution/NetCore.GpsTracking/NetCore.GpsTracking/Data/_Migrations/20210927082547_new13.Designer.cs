﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetCore.GpsTrackingModule.Data;

namespace NetCore.GpsTrackingModule.Data._Migrations
{
    [DbContext(typeof(DBGpsTracking))]
    [Migration("20210927082547_new13")]
    partial class new13
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Command", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("Param1");

                    b.Property<string>("Param2");

                    b.Property<string>("Param3");

                    b.Property<string>("Param4");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("Command");

                    b.HasDiscriminator().HasValue("Command");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<string>("EventName");

                    b.Property<string>("EventTime");

                    b.Property<string>("GeofenceId");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("LocationId");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("Event");

                    b.HasDiscriminator().HasValue("Event");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Geofence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("Note");

                    b.Property<Guid?>("ProfileInfoId");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("ProfileInfoId");

                    b.ToTable("Geofence");

                    b.HasDiscriminator().HasValue("Geofence");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.GeofenceDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<Guid>("GeofenceId");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<float>("Latitude");

                    b.Property<float>("Longitude");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("GeofenceId");

                    b.ToTable("GeofenceDetail");

                    b.HasDiscriminator().HasValue("GeofenceDetail");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.GeofenceMapping", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<Guid?>("GeofenceId");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<Guid?>("ProfileInfoId");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("GeofenceId");

                    b.HasIndex("ProfileInfoId");

                    b.ToTable("GeofenceMapping");

                    b.HasDiscriminator().HasValue("GeofenceMapping");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.GpsDevice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("Code");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<string>("Geofences");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("Notifications");

                    b.Property<string>("Phone");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("GpsDevice");

                    b.HasDiscriminator().HasValue("GpsDevice");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.GpsDeviceMapping", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<Guid?>("GpsDeviceId");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<Guid?>("ProfileInfoId");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("GpsDeviceId");

                    b.HasIndex("ProfileInfoId");

                    b.ToTable("GpsDeviceMapping");

                    b.HasDiscriminator().HasValue("GpsDeviceMapping");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.GpsTrackingMany", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApplicationId");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<bool?>("NoDelete");

                    b.Property<bool?>("NoEdit");

                    b.Property<int>("Seq");

                    b.Property<Guid?>("TypeGuid");

                    b.Property<int>("TypeId");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("GpsTrackingMany");

                    b.HasDiscriminator().HasValue("GpsTrackingMany");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.GpsTrackingNameValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApplicationId");

                    b.Property<string>("Code");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<string>("FullName");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDefault");

                    b.Property<bool?>("IsDeleted");

                    b.Property<bool?>("IsSpecial");

                    b.Property<string>("Name");

                    b.Property<bool?>("NoDelete");

                    b.Property<bool?>("NoEdit");

                    b.Property<string>("OldCode");

                    b.Property<long?>("OldId");

                    b.Property<DateTime?>("OldUpdatedTime");

                    b.Property<Guid?>("ParentId");

                    b.Property<int>("Seq");

                    b.Property<string>("Title");

                    b.Property<Guid?>("TypeGuid");

                    b.Property<int>("TypeId");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("GpsTrackingNameValue");

                    b.HasDiscriminator().HasValue("GpsTrackingNameValue");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<string>("Devices");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("Notification");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("Group");

                    b.HasDiscriminator().HasValue("Group");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<float>("Course");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<DateTime>("DeviceTime");

                    b.Property<Guid>("GpsDeviceId");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<float>("Latitude");

                    b.Property<float>("Longitude");

                    b.Property<string>("Protocol");

                    b.Property<float>("Speed");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("GpsDeviceId");

                    b.ToTable("Location");

                    b.HasDiscriminator().HasValue("Location");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<bool>("Distribution");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("Notificator");

                    b.Property<Guid?>("ProfileInfoId");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("ProfileInfoId");

                    b.ToTable("Notification");

                    b.HasDiscriminator().HasValue("Notification");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.NotificationMapping", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<Guid?>("NotificationId");

                    b.Property<Guid?>("ProfileInfoId");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.HasIndex("NotificationId");

                    b.HasIndex("ProfileInfoId");

                    b.ToTable("NotificationMapping");

                    b.HasDiscriminator().HasValue("NotificationMapping");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<bool>("ReadOnly");

                    b.Property<bool>("Registration");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("Permission");

                    b.HasDiscriminator().HasValue("Permission");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.ProfileInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Admin");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime?>("CreatedTime");

                    b.Property<Guid?>("DataId");

                    b.Property<Guid?>("DeletedById");

                    b.Property<DateTime?>("DeletedTime");

                    b.Property<string>("Email");

                    b.Property<bool?>("IsActived");

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<float>("Phone");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime?>("UpdatedTime");

                    b.HasKey("Id");

                    b.ToTable("ProfileInfo");

                    b.HasDiscriminator().HasValue("ProfileInfo");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Geofence", b =>
                {
                    b.HasOne("NetCore.GpsTrackingModule.Data.ProfileInfo")
                        .WithMany("Geofences")
                        .HasForeignKey("ProfileInfoId");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.GeofenceDetail", b =>
                {
                    b.HasOne("NetCore.GpsTrackingModule.Data.Geofence", "Geofence")
                        .WithMany("Details")
                        .HasForeignKey("GeofenceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.GeofenceMapping", b =>
                {
                    b.HasOne("NetCore.GpsTrackingModule.Data.Geofence", "Geofence")
                        .WithMany("GeofenceMappings")
                        .HasForeignKey("GeofenceId");

                    b.HasOne("NetCore.GpsTrackingModule.Data.ProfileInfo", "ProfileInfo")
                        .WithMany()
                        .HasForeignKey("ProfileInfoId");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.GpsDeviceMapping", b =>
                {
                    b.HasOne("NetCore.GpsTrackingModule.Data.GpsDevice", "GpsDevice")
                        .WithMany("GpsDeviceMappings")
                        .HasForeignKey("GpsDeviceId");

                    b.HasOne("NetCore.GpsTrackingModule.Data.ProfileInfo", "ProfileInfo")
                        .WithMany("GpsDeviceMappings")
                        .HasForeignKey("ProfileInfoId");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Location", b =>
                {
                    b.HasOne("NetCore.GpsTrackingModule.Data.GpsDevice", "GpsDevice")
                        .WithMany()
                        .HasForeignKey("GpsDeviceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.Notification", b =>
                {
                    b.HasOne("NetCore.GpsTrackingModule.Data.ProfileInfo")
                        .WithMany("Notifications")
                        .HasForeignKey("ProfileInfoId");
                });

            modelBuilder.Entity("NetCore.GpsTrackingModule.Data.NotificationMapping", b =>
                {
                    b.HasOne("NetCore.GpsTrackingModule.Data.Notification", "Notification")
                        .WithMany("NotificationMappings")
                        .HasForeignKey("NotificationId");

                    b.HasOne("NetCore.GpsTrackingModule.Data.ProfileInfo", "ProfileInfo")
                        .WithMany()
                        .HasForeignKey("ProfileInfoId");
                });
#pragma warning restore 612, 618
        }
    }
}
