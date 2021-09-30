<template>
  <div :id="ContentId" v-disable="Disable">
    <Modelx DDL :model="model" Property="Name" Tip="Danh sách lệnh điều khiển" First Must :Data="Names" />
    <Modelx TBX :model="model" Property="Param1"/>
    <Modelx TBX :model="model" Property="Param2"/>
    <Modelx TBX :model="model" Property="Param3"/>
    <Modelx TBX :model="model" Property="Param4"/>
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
          { Value: "Lệnh đăng nhập thiết bị", Name: "Lệnh đăng nhập thiết bị" },
          { Value: "Lệnh thay đổi mật khẩu", Name: "Lệnh thay đổi mật khẩu" },
          { Value: "Lệnh reset cài đặt gốc", Name: "Lệnh reset cài đặt gốc" },
          { Value: "Lệnh reset thiết bị GPS", Name: "Lệnh reset thiết bị GPS" },
          { Value: "Kích hoạt chế độ ngủ", Name: "Kích hoạt chế độ ngủ" },
          { Value: "Thoát khỏi chế độ ngủ", Name: "Thoát khỏi chế độ ngủ" },
          { Value: "Lệnh đổi ID thiết bị", Name: "Lệnh đổi ID thiết bị" },
          { Value: "Lệnh khai báo APN", Name: "Lệnh khai báo APN" },
          { Value: "Lệnh khai báo server", Name: "Lệnh khai báo server" },
          { Value: "Lệnh thay đổi server AGPS", Name: "Lệnh thay đổi server AGPS" },
          { Value: "Lệnh cài đặt gửi tín hiệu", Name: "Lệnh cài đặt gửi tín hiệu" },
          { Value: "Lệnh cài đặt LTE", Name: "Lệnh cài đặt LTE" },
          { Value: "Lệnh cài đặt GPS", Name: "Lệnh cài đặt GPS" },
        ],
      }
    },

    methods: {
      async ReCorrectModelAfterGet(model) {
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
    }
  }
</script>

<style>
</style>
