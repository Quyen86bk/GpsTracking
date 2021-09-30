<template>
  <div :id="ContentId" v-disable="Disable">
    <Modelx Any Title="@NO@">
      <a-checkbox v-model="model.ReadOnly">Chế độ quan sát</a-checkbox>
    </Modelx>
    <Modelx Any Title="@NO@">
      <a-checkbox v-model="model.Registration">Cấp quyền đăng ký tài khoản mới</a-checkbox>
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
        var inputed = this.Selected(this.model.ReadOnly)

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
