<template>
  <div :id="ContentId" v-disable="Disable">
    <!--<Modelx TBX :model="model" Property="Name" Tip="Thông báo" First Must />-->
    <Modelx DDL :model="model" Property="Name" Tip="Thông báo" First :Data="Names" />
    <Modelx Any Title="Phương thức">
      <a-select mode="multiple" :default-value="model.Notificators" style="width: 250px;" placeholder="Phương thức" @change="NotificatorOnChange">
        <a-select-option v-for="(item, index) in Notificators" :key="(item).toString()">
          {{ (item).toString() }}
        </a-select-option>
      </a-select>
    </Modelx>
    <Modelx Any Title="@NO@">
      <a-checkbox v-model="model.Distribution">Phân quyền cho mọi thiết bị</a-checkbox>
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
        Names: [
          { Value: "Trạng thái online", Name: "Trạng thái online" },
          { Value: "Trạng thái offline", Name: "Trạng thái offline" },
          { Value: "Đi vào khu vực", Name: "Đi vào khu vực" },
          { Value: "Ra khỏi khu vực", Name: "Ra khỏi khu vực" },
          { Value: "Hai đối tượng tiếp cận nhau", Name: "Hai đối tượng tiếp cận nhau" },
        ],
        Notificators: [],
      }
    },

    methods: {
      NotificatorOnChange(value) {
        this.model.Notificators = value
      },
      async GetNotificators() {
        try {
          var url = "/Api/GpsTracking/Notificator/GetNotificators"
          var model = {}

          let response = await this.AjaxPOST(url, model)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {
              this.Notificators = response.data.Result.Models
            }
            else {
              this.MessageError("Get", "Notificators")
            }
          }
        }
        catch (err) {
          this.HandleError(err)
        }
      },

      async ReCorrectModelAfterGet(model) {
        this.Notificators = model.Notificators
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
      this.GetNotificators()
    }
  }
</script>

<style>
</style>
