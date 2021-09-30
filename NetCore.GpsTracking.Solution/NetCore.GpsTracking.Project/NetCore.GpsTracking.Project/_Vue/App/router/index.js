import Vue from 'vue'
import VueRouter from 'vue-router'
import { routes } from './routes'

Vue.use(VueRouter)
let router = new VueRouter({
  mode: 'history',
  routes
})

var lib = window.js.lib
var session = window.js.session

router.beforeEach((to, from, next) => {

  const publicPages = ['/Login', '/Register'];//'/',
  const authRequired = !publicPages.includes(to.path);

  if (authRequired && session.Token() == null) {
    lib.GoLogin()
  }
  next();
})

export default router
