<template>
  <center>
    <a-card style="margin-top: 20px;">
      <a-button @click="SendToEmail">Gửi báo cáo đến email</a-button>
    </a-card>
  </center>
</template>

<script>
  var lib = window.js.lib
  var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")
  var mixin_Utility = lib.GetComponent(window.index.all, "mixin-Utility")
  export default {
    mixins: [mixin_lib, mixin_Utility],

    methods: {
      SendToEmail() {
        this.SetFilterValue("SendEmail", true)
        this.ReLoadPage(0.5)
      },
    },
  }
</script>
