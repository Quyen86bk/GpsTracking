<template>
  <div v-show="IsLoggedIn" id="HomeContainer">
    <a-spin v-if="!IsLoggedIn" tip="Loading..." size="large" :style="LoadingStyle()" />
    <template v-else>
      <img src="/Index/img/Home.gif" style="width: 100%;" />
    </template>
  </div>
</template>

<script>
  var lib = window.js.lib
  var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")
  export default {
    mixins: [mixin_lib],

    computed: {
      BGStyle() {
        var imgBGHeight = this.ScreenHeight - 200
        var imgBGWidth = 874 * imgBGHeight / 581

        var paddingLeft = (this.ContainerWidth - imgBGWidth) / 2
        if (paddingLeft < 0)
          paddingLeft = 0

        return {
          'height': imgBGHeight + 'px',
          'padding-left': paddingLeft + 'px'
        }
      }
    },

    data() {
      return {
        ContainerWidth: 0,
        model: {},
      }
    },

    methods: {
      OnLoadedBG() {
        this.ContainerWidth = lib.Width("HomeContainer")
      },

      async GetNews() {
        try {
          if (this.IsLoggedIn) {

            var url = "/Api/GpsTracking/GpsDevice/GetNews"
            let response = await this.AjaxGET(url)

            if (this.HandleResponse(response))
              this.model = response.data.Result.model
          }
        }
        catch (err) {
          this.HandleError(err)
        }
      },
    },

    async created() {
      //this.GetNews()
    }
  }
</script>
