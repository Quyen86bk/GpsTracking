//func
var lib = window.js.lib

//Vue
import Vue from 'vue'
import appLayout from './app.vue'
import router from './router/index'

Vue.prototype.TestMode = false
Vue.config.productionTip = false;

//Ant
import Antd from 'ant-design-vue';
import 'ant-design-vue/dist/antd.css';
Vue.use(Antd);

import { message } from 'ant-design-vue'
Vue.prototype.message = message

//icons
Vue.component('icon', window.js.icon.FontAwesomeIcon)

//VueScrollbar
import VueScrollbar from 'vue2-scrollbar';
require("vue2-scrollbar/dist/style/vue2-scrollbar.css")
Vue.component('IQ-Scroll', VueScrollbar)

//store
import store from './store'
import { sync } from 'vuex-router-sync'
sync(store, router)

//components
const requireComponent = require.context(
  '../components',
  true,
  /.(vue)$/i
)

requireComponent.keys().forEach(fileName => {
  const componentConfig = requireComponent(fileName)

  var array = fileName.split('/')
  let componentName = array[array.length - 1]
  componentName = componentName.replace(".vue", "")

  //console.log(componentName)
  //console.log(componentConfig)

  try {
    Vue.component(
      componentName,
      componentConfig.default || componentConfig
    )
  }
  catch (err) {
    console.log(componentName, err)
  }
})

//console.log(window.index.all)
for (var i = 0; i < window.index.all.length; i++) {
  if (!lib.StartsWith(window.index.all[i].name, "mixin-")) {
    try {
      Vue.component(
        window.index.all[i].name,
        window.index.all[i].com.default || window.index.all[i].com
      )
    }
    catch (err) {
      console.log(window.index.all[i].name, err)
    }
  }
}

//DirectiveDisable
Vue.directive("disable", {
  bind: function (el, binding) {
    lib.DirectiveDisable(el, binding);
  },
  update: function (el, binding) {
    lib.DirectiveDisable(el, binding);
  },
  componentUpdated: function (el, binding) {
    lib.DirectiveDisable(el, binding);
  }
});

//app
const app = new Vue({
  store,
  router,
  ...appLayout
})

//export
export {
  app,
  router,
  store
}
