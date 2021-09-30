<template>
  <div>
    <center>
      <h1>Cài đặt tài khoản</h1>
      <div style="width: 560px;">
        <Modelx TBX :model="model" Property="Name" Tip="Tên người dùng"  First />
        <Modelx TBX :model="model" Property="Email" Tip="Email đăng nhập" First />
        <Modelx TBX :model="model" Property="Phone" Tip="Số điện thoại" First />
        <Modelx Any Title="@No@" Width1="100">
          <a-spin v-if="Submiting" tip="Loading..." />
          <a-button v-else :disabled="Done" type="primary" @click="Submit">{{ Lang(!Done ? 'Thay đổi thông tin' : 'Thay đổi thông tin thành công!') }}</a-button>
        </Modelx>
      </div>
    </center>
  </div>
</template>

<script>
  var lib = window.js.lib
  var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")
  var mixin_Model_Data = lib.GetComponent(window.index.all, "mixin-Model-Data")
  export default {
    mixins: [mixin_lib],

    data() {
      return {
        model: {
          Name: "",
          Email: "",
          Phone: "",
        },

        Submiting: false,
        Done: false,
      }
    },

    methods: {

      async Submit() {
        try {
          var valid = true
          var message = ""

          if (!lib.ValidPhone(this.model.Phone)) {
            valid = false
            message += this.Lang("Lỗi nhập") + "\r\n"
          }

          if (!valid)
            this.MessageErrorAny(message)
          else {
            this.Submiting = true
            var url = "/Api/Manage/Account/ChangePassword"

            let response = await this.AjaxPOST(url, this.model)
            this.Submiting = false

            if (this.HandleResponse(response)) {

              if (response.data.Status == 1) {
                this.MessageOK("", "")
                this.Done = true
              }
              else {
                this.MessageError("", "")
              }
            }
          }
        }
        catch (err) {
          this.HandleError(err)
          this.Submiting = false
        }
      },

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
  }
</script>
