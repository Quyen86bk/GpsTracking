<template>
  <div :id="ContentId" v-disable="Disable">
    <Modelx TBX :model="model" Property="Name" Tip="Tên nhóm" First Must />
    <Modelx Any Title="Thiết bị thuộc nhóm">
      <a-select mode="multiple" :default-value="model.GpsDevices" style="width: 250px;" placeholder="Phân quyền thiết bị" @change="GpsDeviceOnChange">
        <a-select-option v-for="(item, index) in GpsDevices" :key="(item).toString()">
          {{ (item).toString() }}
        </a-select-option>
      </a-select>
    </Modelx>
    <Modelx Any Title="Thông báo của nhóm">
      <a-select mode="multiple" :default-value="model.Notifications" style="width: 250px;" placeholder="Phân quyền thiết bị" @change="NotificationOnChange">
        <a-select-option v-for="(item, index) in Notifications" :key="(item).toString()">
          {{ (item).toString() }}
        </a-select-option>
      </a-select>
    </Modelx>
    <Modelx Any Title="Giới hạn địa lý">
      <a-select mode="multiple" :default-value="model.Geofences" style="width: 250px;" placeholder="Giới hạn địa lý" @change="GeofenceOnChange">
        <a-select-option v-for="(item, index) in Geofences" :key="(item).toString()">
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
        GpsDevices: [],
        Notifications: [],
        Geofences: [],
      }
    },

    methods: {
      GpsDeviceOnChange(value) {
        this.model.GpsDevices = value
      },
      async GetGpsDevices() {
        try {
          var url = "/Api/GpsTracking/GpsDevice/GetGpsDevices"
          var model = {}

          let response = await this.AjaxPOST(url, model)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {
              this.GpsDevices = response.data.Result.Models
            }
            else {
              this.MessageError("Get", "GpsDevices")
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

      async ReCorrectModelAfterGet(model) {
        this.GpsDevices = model.GpsDevices
        this.Notifications = model.Notifications
        this.Geofences = model.Geofences
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
      this.GetGpsDevices()
      this.GetNotifications()
      this.GetGeofences()
    }
  }
</script>

<style>
</style>
