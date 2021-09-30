<template>
  <xApp-Layout :routes="routes" :ManualMenus="ManualMenus" />
</template>

<script>
  import { routes, ManualMenus } from './router/routes'
  import xApp_Layout from 'components/App-Layout'

  import Vue from 'vue'
  Vue.component('xApp-Layout', xApp_Layout)

  export default {
    data() {
      return {
        routes,
        ManualMenus
      };
    },
  }
</script>
