<template>
  <div :id="ContentId" v-disable="Disable">
    <Modelx TBX :model="model" Property="Protocol" Tip="Giao thức" />
    <Modelx TBX :model="model" Property="DeviceTime" Tip="Cập nhật lần cuối" />
    <Modelx TBX :model="model" Property="Address" Tip="Địa chỉ" />
    <Modelx TBX :model="model" Property="Latitude" Tip="Vĩ độ" />
    <Modelx TBX :model="model" Property="Longitude" Tip="Kinh độ" />
    <Modelx TBX :model="model" Property="Speed" Tip="Tốc độ" />
    <Modelx TBX :model="model" Property="Course" Tip="Hướng di chuyển" />
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
        var inputed = this.Selected(this.model.Address)
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
