using Microsoft.EntityFrameworkCore;
using NetCore.Library;
using NetCore.GpsTrackingModule.Controllers;
using NetCore.GpsTrackingModule.Data;
using NetCore.Websites;
using NetCore.Websites.Models;
using NetCore.Websites.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.Websites.Data.dbLogin;
using NetCore.ManageModule.Models;
using NetCore.Websites.Services.Account;
using NetCore.ManageModule;
using NetCore.ManageModule.Data;

namespace NetCore.GpsTrackingModule.Services
{
    public partial interface IProfileInfoService
    {
        Task Save(Pipeline pipeline, ProfileInfo model);

        Task Get(Pipeline pipeline, Guid id);

        Task GetPage(Pipeline pipeline, PageFilterVM filter);

        Task GetList(Pipeline pipeline, ListFilterVM filter);

        Task Delete(Pipeline pipeline, List<Guid> IDs);
    }

    public partial class ProfileInfoService : _ServiceGpsTracking, IProfileInfoService
    {
        public ProfileInfoService(IServiceProvider _ServiceProvider)
            : base(_ServiceProvider)
        {
        }

        public async Task SaveUser(Pipeline pipeline, UserVM model, CRUDtype CRUD)
        {
            model.UserName = lib.Trim2Space(model.UserName);

            bool validModel = false;
            if (CRUD == CRUDtype.Create)
            {
                if (lib.ValidUserName(model.UserName, MembershipConfig.UseEmail)
                    && lib.ValidPassword(model.Password)

                    && lib.ValidName(model.FirstName)
                    && lib.ValidName(model.LastName)
                    && lib.ValidAliasName(model.AliasName))
                {
                    validModel = true;
                }
            }
            else
            {
                if (model.IsReset == true)
                {
                    if (lib.ValidUserName(model.UserName, MembershipConfig.UseEmail)
                        && lib.ValidPassword(model.Password)

                        && lib.ValidName(model.FirstName)
                        && lib.ValidName(model.LastName)
                        && lib.ValidAliasName(model.AliasName))
                    {
                        validModel = true;
                    }
                }
                else if (lib.ValidUserName(model.UserName, MembershipConfig.UseEmail)

                    && lib.ValidName(model.FirstName)
                    && lib.ValidName(model.LastName)
                    && lib.ValidAliasName(model.AliasName))
                {
                    validModel = true;
                }
            }

            //validAll
            if (!validModel)
            {
                pipeline.Status = ResponseStatus.InvalidModel;
            }
            else
            {
                var validExists = !LoginDBs.User.Query.Any(x => x.Id != model.Id && x.UserName == model.UserName);
                if (!validExists)
                {
                    pipeline.Status = ResponseStatus.DataExists;
                }
                else
                {
                    User User = null;
                    if (CRUD == CRUDtype.Create)
                    {
                        User = new User
                        {
                            Id = model.Id,
                            IsActived = model.IsActived,

                            UserName = model.UserName,
                            Email = lib.ValidEmail(model.UserName) ? model.UserName : null,
                            ManualHash = MembershipConfig.ManualHash ? lib.HashMD5(model.Password) : null,

                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            AliasName = model.AliasName
                        };

                        var registerResult = await CoreServices.UserManager.CreateAsync(User, model.Password);
                        if (registerResult.Succeeded)
                        {
                            await CoreServices.UserManager.AddToRoleAsync(User, lib.NewRegister);
                            ManageRepository.GetCacheUsers(LoginDBs, CRUDtype.Create, new List<Guid> { User.Id }, true);

                            if (lib.Selected(model.UserGroupId))
                            {
                                var newMapping = new UserGroupMapping
                                {
                                    UserGroupId = lib.ToGuid(model.UserGroupId),
                                    UserId = User.Id,
                                };
                                _ServiceManage.DBs.UserGroupMapping.InsertNow(newMapping);
                                ManageRepository.GetCacheUserGroupMappings(_ServiceManage.DBs, CRUDtype.Create, new List<Guid> { newMapping.Id });
                            }

                            //Result
                            pipeline.Status = ResponseStatus.Successful;
                        }
                    }
                    else
                    {
                        User = await LoginDBs.User.QueryNotAdmin.FirstOrDefaultAsync(x => x.Id == model.Id);
                        if (User != null)
                        {
                            User.IsActived = model.IsActived;
                            if (User.IsActived == true)
                            {
                                User.AccessFailedCount = 0;
                                User.LockoutEnd = null;
                            }

                            User.UserName = model.UserName;
                            User.Email = lib.ValidEmail(model.UserName) ? model.UserName : null;

                            User.FirstName = model.FirstName;
                            User.LastName = model.LastName;
                            User.AliasName = model.AliasName;

                            if (model.IsReset == true)
                            {
                                string resetToken = await CoreServices.UserManager.GeneratePasswordResetTokenAsync(User);
                                await CoreServices.UserManager.ResetPasswordAsync(User, resetToken, model.Password);

                                User.ManualHash = MembershipConfig.ManualHash ? lib.HashMD5(model.Password) : null;
                                User.NeedChangePassword = true;
                            }

                            await CoreServices.UserManager.UpdateAsync(User);
                            ManageRepository.GetCacheUsers(LoginDBs, CRUDtype.Edit, new List<Guid> { User.Id }, true);

                            if (lib.Selected(model.UserGroupId))
                            {
                                var mapping = _ServiceManage.DBs.UserGroupMapping.Query.FirstOrDefault(x => x.UserId == User.Id);
                                if (mapping == null)
                                {
                                    mapping = new UserGroupMapping
                                    {
                                        UserGroupId = lib.ToGuid(model.UserGroupId),
                                        UserId = User.Id,
                                    };
                                    _ServiceManage.DBs.UserGroupMapping.InsertNow(mapping);
                                    ManageRepository.GetCacheUserGroupMappings(_ServiceManage.DBs, CRUDtype.Create, new List<Guid> { mapping.Id });
                                }
                                else
                                {
                                    mapping.UserGroupId = lib.ToGuid(model.UserGroupId);
                                    await DBs.Save();
                                    ManageRepository.GetCacheUserGroupMappings(_ServiceManage.DBs, CRUDtype.Edit, new List<Guid> { mapping.Id });
                                }
                            }

                            //Result
                            pipeline.Status = ResponseStatus.Successful;
                        }
                    }
                }
            }

            //Result
            pipeline.Result = new
            {
                model
            };
        }

        public async Task Save(Pipeline pipeline, ProfileInfo model)
        {
            model.Email = lib.Trim2Space(model.Email);

            bool validModel = false;
            if (lib.ValidUserName(model.Email, MembershipConfig.UseEmail)
                    && lib.ValidPassword(model.Password)
                    && lib.ValidName(model.Name))
            {
                validModel = true;
            }

            if (!validModel)
            {
                pipeline.Status = ResponseStatus.InvalidModel;
                return;
            }

            ProfileInfo ProfileInfo = null;
            if (lib.NotSelected(model.Id))
            {
                CRUD = CRUDtype.Create;
                model.Id = Guid.NewGuid();
                ProfileInfo = new ProfileInfo
                {
                    Id = model.Id,
                    CreatedById = pipeline.UserId
                };
                DBs.ProfileInfo.Insert(ProfileInfo);
            }
            else
            {
                CRUD = CRUDtype.Edit;
                ProfileInfo = await DBs.ProfileInfo.Query
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
            }

            if (ProfileInfo != null)
            {
                //UpdatedBy
                ProfileInfo.UpdatedById = pipeline.UserId;
                ProfileInfo.UpdatedTime = lib.Time;

                //Set
                ProfileInfo.Name = model.Name;
                ProfileInfo.Email = model.Email;
                ProfileInfo.Password = model.Password;
                ProfileInfo.Phone = model.Phone;
                ProfileInfo.Admin = model.Admin;

                //user
                if (CRUD == CRUDtype.Create)
                {
                    var group = _ServiceManage.DBs.UserGroup.Query.FirstOrDefault(x => x.Name == "Default");
                    if (group != null)
                    {
                        var user = new UserVM
                        {
                            Id = model.Id,
                            UserGroupId = group.Id,

                            FirstName = model.Name,
                            LastName = model.Name,

                            UserName = model.Email,
                            Password = model.Password,

                            IsFirstNameFirst = true,
                            IsFirstNameShort = true,

                            IsActived = true,
                        };
                        await SaveUser(new Pipeline(pipeline).Default, user, CRUD);
                    }
                }

                //Remove GpsDeviceMappings

                var currentGpsDeviceMappings = DBs.GpsDeviceMapping.Query.Include(x => x.GpsDevice).Where(x => x.ProfileInfoId == ProfileInfo.Id).ToList();
                var newGpsDeviceMappings = model.GpsDevices;
                var removedGpsDeviceMappings = currentGpsDeviceMappings.Where(x => !newGpsDeviceMappings.Any(y => y == x.GpsDevice.Name)).ToList();
                foreach (var item in removedGpsDeviceMappings)
                {
                    DBs.GpsDeviceMapping.Delete(item);
                }

                //Add new GpsDeviceMappings

                foreach (var item in model.GpsDevices)
                {
                    var gpsDevice = DBs.GpsDevice.Query.FirstOrDefault(x => x.Name == item);
                    if (gpsDevice != null)
                    {
                        var mapping = DBs.GpsDeviceMapping.Query.FirstOrDefault(x => x.GpsDeviceId == gpsDevice.Id && x.ProfileInfoId == ProfileInfo.Id);
                        if (mapping == null)
                        {
                            mapping = new GpsDeviceMapping
                            {
                                GpsDeviceId = gpsDevice.Id,
                                ProfileInfoId = ProfileInfo.Id,
                            };
                            DBs.GpsDeviceMapping.Insert(mapping);
                        }
                    }
                }

                //Remove GeofenceMappings

                var currentGeofenceMappings = DBs.GeofenceMapping.Query.Include(x => x.Geofence).Where(x => x.ProfileInfoId == ProfileInfo.Id).ToList();
                var newGeofenceMappings = model.Geofences;
                var removedGeofenceMappings = currentGeofenceMappings.Where(x => !newGeofenceMappings.Any(y => y == x.Geofence.Name)).ToList();
                foreach (var item in removedGeofenceMappings)
                {
                    DBs.GeofenceMapping.Delete(item);
                }

                //Add new GeofenceMappings

                foreach (var item in model.Geofences)
                {
                    var geofence = DBs.Geofence.Query.FirstOrDefault(x => x.Name == item);
                    if (geofence != null)
                    {
                        var mapping = DBs.GeofenceMapping.Query.FirstOrDefault(x => x.GeofenceId == geofence.Id && x.ProfileInfoId == ProfileInfo.Id);
                        if (mapping == null)
                        {
                            mapping = new GeofenceMapping
                            {
                                GeofenceId = geofence.Id,
                                ProfileInfoId = ProfileInfo.Id,
                            };
                            DBs.GeofenceMapping.Insert(mapping);
                        }
                    }
                }

                //Remove NotificationMappings

                var currentNotificationMappings = DBs.NotificationMapping.Query.Include(x => x.Notification).Where(x => x.ProfileInfoId == ProfileInfo.Id).ToList();
                var newNotificationMappings = model.Notifications;
                var removedNotificationMappings = currentNotificationMappings.Where(x => !newNotificationMappings.Any(y => y == x.Notification.Name)).ToList();
                foreach (var item in removedNotificationMappings)
                {
                    DBs.NotificationMapping.Delete(item);
                }

                //Add new NotificationMappings

                foreach (var item in model.Notifications)
                {
                    var notification = DBs.Notification.Query.FirstOrDefault(x => x.Name == item);
                    if (notification != null)
                    {
                        var mapping = DBs.NotificationMapping.Query.FirstOrDefault(x => x.NotificationId == notification.Id && x.ProfileInfoId == ProfileInfo.Id);
                        if (mapping == null)
                        {
                            mapping = new NotificationMapping
                            {
                                NotificationId = notification.Id,
                                ProfileInfoId = ProfileInfo.Id,
                            };
                            DBs.NotificationMapping.Insert(mapping);
                        }
                    }
                }

                //Save
                await DBs.Save();
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                model
            };
        }

        public async Task Get(Pipeline pipeline, Guid id)
        {
            var model = await DBs.ProfileInfo.Query
                .Include(x => x.GpsDeviceMappings).ThenInclude(x => x.GpsDevice)
                .Include(x => x.GeofenceMappings).ThenInclude(x => x.Geofence)
                .Include(x => x.NotificationMappings).ThenInclude(x => x.Notification)
                .FirstOrDefaultAsync(x => x.Id == id);

            model.GpsDevices = model.GpsDeviceMappings.Select(x => x.GpsDevice.Name).ToList();
            model.Geofences = model.GeofenceMappings.Select(x => x.Geofence.Name).ToList();
            model.Notifications = model.NotificationMappings.Select(x => x.Notification.Name).ToList();

            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new { model };
        }

        public async Task GetPage(Pipeline pipeline, PageFilterVM filter)
        {
            //Select
            var ProfileInfos = DBs.ProfileInfo.Query;

            //FilterData

            ////filter.RelationId
            //if (lib.Selected(filter.RelationId))
            //{
            //    if (filter.Option == 2)
            //        ProfileInfos = ProfileInfos.Where(x => x.ParentId == filter.RelationId);
            //    else
            //        ProfileInfos = ProfileInfos.Where(x => x.ParentId != filter.RelationId);
            //}

            //filter.Keyword
            if (lib.Selected(filter.Keyword))
                ProfileInfos = ProfileInfos.Where(x => x.Name.Contains(filter._Keyword));

            //filter.Date
            if (filter.HaveDate())
                ProfileInfos = ProfileInfos.Where(x => x.CreatedTime >= filter.FromDate && x.CreatedTime <= filter.ToDate);

            //Order
            ProfileInfos = ProfileInfos.OrderByDescending(x => x.CreatedTime);

            //Models
            var Total = ProfileInfos.Count();
            var Models = await ProfileInfos
                .Include(x => x.GpsDeviceMappings).ThenInclude(x => x.GpsDevice)
                .Include(x => x.GeofenceMappings).ThenInclude(x => x.Geofence)
                .Include(x => x.NotificationMappings).ThenInclude(x => x.Notification)
                .Skip(filter.From).Take(Config.MaxSelectPage(filter.From, filter.To))
                .ToListAsync();

            foreach (var model in Models)
            {
                //GpsDeviceMappings
                foreach (var mapping in model.GpsDeviceMappings.Select(x => x.GpsDevice.Name).ToList())
                    model.ListGpsDevices += mapping + ", ";
                model.ListGpsDevices = lib.RemoveLast(model.ListGpsDevices, ",");

                //GeofenceMappings
                foreach (var mapping in model.GeofenceMappings.Select(x => x.Geofence.Name).ToList())
                    model.ListGeofences += mapping + ", ";
                model.ListGeofences = lib.RemoveLast(model.ListGeofences, ",");

                //NotificationMappings
                foreach (var mapping in model.NotificationMappings.Select(x => x.Notification.Name).ToList())
                    model.ListNotifications += mapping + ", ";
                model.ListNotifications = lib.RemoveLast(model.ListNotifications, ",");
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models,
                Total,

                FilterData = new
                {
                }
            };
        }

        public async Task GetList(Pipeline pipeline, ListFilterVM filter)
        {
            //Select
            var ProfileInfos = DBs.ProfileInfo.Query;

            //filter
            if (lib.Selected(filter.Id))
                ProfileInfos = ProfileInfos.Where(x => x.Id == filter.Id);

            //if (lib.Selected(filter.ParentId))
            //ProfileInfos = ProfileInfos.Where(x => x.ParentId == filter.ParentId);

            if (lib.Selected(filter.ExcludeId))
                ProfileInfos = ProfileInfos.Where(x => x.Id != filter.ExcludeId);

            if (lib.Selected(filter.Keyword))
                ProfileInfos = ProfileInfos.Where(x => x.Name.Contains(filter._Keyword));

            //Order
            ProfileInfos = ProfileInfos.OrderBy(x => x.Name);

            //Models
            var List = await ProfileInfos
                .Take(Config.MaxSelectList(filter.Keyword))
                .ToListAsync();

            var Models = new List<ListVM>();
            foreach (var item in List)
            {
                Models.Add(new ListVM
                {
                    Value = item.Id,
                    Name = item.Name,
                    Info = item.Name,
                });
            }

            //Result
            pipeline.Status = ResponseStatus.Successful;
            pipeline.Result = new
            {
                Models
            };
        }

        public async Task Delete(Pipeline pipeline, List<Guid> IDs)
        {
            var ProfileInfos = DBs.ProfileInfo.Query.Where(x => IDs.Any(y => y == x.Id));
            await ProfileInfos.ForEachAsync(x =>
            {
                x.IsDeleted = true;
                x.DeletedById = pipeline.UserId;
                x.DeletedTime = lib.Time;
            });

            await DBs.Save();
            pipeline.Status = ResponseStatus.Successful;
        }
    }
}
