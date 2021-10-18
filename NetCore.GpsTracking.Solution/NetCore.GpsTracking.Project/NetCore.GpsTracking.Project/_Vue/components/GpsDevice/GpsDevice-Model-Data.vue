<template>
  <div :id="ContentId" v-disable="Disable">
    <Modelx TBX :model="model" Property="Name" Tip="Tên thiết bị" First Must />
    <Modelx TBX :model="model" Property="Code" Tip="Định danh thiết bị" Must />
    <Modelx TBX :model="model" Property="Phone" Tip="Số điện thoại trên thiết bị" Must />
    <Modelx DDL :model="model" Property="CategoryId" Tip="Đối tượng theo dõi" :Data="Category" />
    <Modelx Any Title="Phân quyền giới hạn địa lý">
      <a-select mode="multiple" :default-value="model.Geofences" style="width: 250px;" placeholder="Phân quyền giới hạn địa lý" @change="GeofenceOnChange">
        <a-select-option v-for="(item, index) in Geofences" :key="(item).toString()">
          {{ (item).toString() }}
        </a-select-option>
      </a-select>
    </Modelx>
    <Modelx Any Title="Phân quyền thông báo">
      <a-select mode="multiple" :default-value="model.Notifications" style="width: 250px;" placeholder="Phân quyền thiết bị" @change="NotificationOnChange">
        <a-select-option v-for="(item, index) in Notifications" :key="(item).toString()">
          {{ (item).toString() }}
        </a-select-option>
      </a-select>
    </Modelx>
  </div>
</template>

<script>
  var lib = window.js.lib
  var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")
  var mixin_Model_Data = lib.GetComponent(window.index.all, "mixin-Model-Data")
  export default {
    mixins: [mixin_lib, mixin_Model_Data],

    data() {
      return {
        Category: [
          { Value: 1, Name: "Default" },
          { Value: 2, Name: "Ôtô" },
          { Value: 3, Name: "Xe máy" },
        ],
        Geofences: [],
        Notifications: [],
      }
    },

    methods: {
      GeofenceOnChange(value) {
        this.model.Geofences = value
      },
      async GetGeofences() {
        try {
          var url = "/Api/GpsTracking/Geofence/GetGeofencesNames"
          var model = {}

          let response = await this.AjaxPOST(url, model)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {
              this.Geofences = response.data.Result.Models
            }
            else {
              this.MessageError("Get", "Geofences")
            }
          }
        }
        catch (err) {
          this.HandleError(err)
        }
      },

      NotificationOnChange(value) {
        this.model.Notifications = value
      },
      async GetNotifications() {
        try {
          var url = "/Api/GpsTracking/Notification/GetNotifications"
          var model = {}

          let response = await this.AjaxPOST(url, model)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {
              this.Notifications = response.data.Result.Models
            }
            else {
              this.MessageError("Get", "Notifications")
            }
          }
        }
        catch (err) {
          this.HandleError(err)
        }
      },

      async ReCorrectModelAfterGet(model) {
        this.Geofences = model.Geofences
        this.Notifications = model.Notifications
      },
      async ReCorrectModelBeforeSave(model) {
      },
      async CheckValidModelBeforeSave(model) {
        var valid = true
        var message = ""
        return {
          Valid: valid, Message: message
        }
      },

      async SetParentModel(parentModel) {
        this.$emit("SetParentModel", parentModel)
      },
      async GetParentModel() {
      },

      async CheckInputed() {
        var inputed = this.Selected(this.model.Name)
          && this.Selected(this.model.Code)
          && this.Selected(this.model.Phone)

        this.$emit("SetInputed", inputed)
        this.Inputed = inputed
      },

      async Prepare() {
      },
      async ReChoice() {
        await this.ReCorrectModelAfterGet(this.model)
      },
    },

    async created() {
      this.GetGeofences()
      this.GetNotifications()
    }
  }
</script>

<style>
</style>
