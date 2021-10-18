<template>
  <div>
    <div style="position: fixed; top: 16px; right: 250px; background-color: white; z-index: 999">
      <a-button @click="Add" :disabled="this.Tabs.length >= 10" type="primary"><a-icon type="plus-circle" style="font-size: 16px;" />Add more tabs: ({{this.Tabs.length}})</a-button>
      Size: <a-input-number :min="2" :max="3" v-model="X" style="width: 50px;"></a-input-number>
      x
      <a-input-number :min="1" :max="3" v-model="Y" style="width: 50px;"></a-input-number>
    </div>
    <div>
      <template v-for="(tab, index) in Tabs">
        <div :key="tab.Number" :style="TabStyle">
          <iframe :src="'/GpsTracking?noSlider=1&TabNumber=' + tab.Number + '&Width=' + (TabWidth - 2) + '&Height=' + (TabHeight - 2)"
                  :width="TabWidth - 2" :height="TabHeight - 2"
                  style="border: none;" scrolling="no"></iframe>
          <button :id="'btnCloseTab' + tab.Number" @click="CloseTab(tab.Number)" style="display:none;"></button>
        </div>
      </template>
    </div>
  </div>
</template>

<script>
  var lib = window.js.lib
  var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")
  import GpsTracking from 'components/GpsTracking/GpsTracking'

  export default {
    mixins: [mixin_lib],

    computed: {
      TabWidth() {
        var result = 500

        var width = lib.ScreenWidth()
        if (!this.$store.state.Collapsed)
          width -= 250
        else
          width -= 50

        result = width / this.X
        return result
      },
      TabHeight() {
        var result = 500

        var height = lib.ScreenHeight()
        if (!this.$store.state.Collapsed)
          height -= 86

        result = height / this.Y
        return result
      },
      TabStyle() {
        return {
          "float": "left",
          "padding": "1px",
          "background-color": "dodgerblue",
          width: this.TabWidth + "px",
          height: this.TabHeight + "px",
        }
      },
    },

    data() {
      return {
        Tabs: [],
        X: 2,
        Y: 2,
      }
    },

    methods: {
      Add() {
        this.Tabs.push({ Number: this.Tabs.length + 1 })
      },

      CloseTab(tabNumber) {
        for (var i = 0; i < this.Tabs.length; i++) {
          if (this.Tabs[i].Number == tabNumber) {
            this.Tabs.splice(i, 1)
          }
        }
      },
    },

    async created() {
    },
    mounted: function () {
      const plugin = document.createElement("script");
      plugin.setAttribute(
        "src",
        "/Index/js.js?" + new Date()
      );
      document.head.appendChild(plugin);

      this.Add()
      var autoAdd = setInterval(function () {
        if (this.Tabs.length < 4)
          this.Add()
        else
          clearInterval(autoAdd)
      }.bind(this), 1000);
    }
  }
</script>

<style>
</style>
