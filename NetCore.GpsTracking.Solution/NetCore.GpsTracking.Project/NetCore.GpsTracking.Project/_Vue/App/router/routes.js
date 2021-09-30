var lib = window.js.lib
var session = window.js.session
var isLoggedIn = session.Token() != null

import Home from 'components/Home'
import Login from 'components/Account/Login'
var ChangePassword = lib.GetComponent(window.index.all, "ChangePassword")
import Profile from 'components/Account/Profile'

var Error403 = lib.GetComponent(window.index.all, "Error403")
var Error404 = lib.GetComponent(window.index.all, "Error404")
var Index = lib.GetComponent(window.index.all, "Index")

import GpsTracking from 'components/GpsTracking/GpsTracking'

export const routes = [
  { name: 'Home', path: '/', component: Home, title: 'Home', icon: 'home', show: true, enabled: true },
  { name: 'Login', path: '/Login', component: Login, title: 'Login', icon: 'lock', show: !isLoggedIn, enabled: true },
  { name: 'ChangePassword', path: '/ChangePassword', component: ChangePassword, title: 'Change Password', icon: 'lock', show: false, enabled: true },
  { name: 'Profile', path: '/Profile', component: Profile, title: 'Profile', icon: 'user', show: false, enabled: true },

  { name: 'Error403', path: '/Error403', component: Error403, title: 'Error403', icon: 'lock', show: false, enabled: true },
  { name: 'Error404', path: '/Error404', component: Error404, title: 'Error404', icon: 'lock', show: false, enabled: true },
  { name: 'Index', path: '/Index/:Module?/:Entity?/:CMD?/:Id?', component: Index, title: 'Index', icon: 'star', show: false, enabled: true },

  { name: 'GpsTracking', path: '/GpsTracking', component: GpsTracking, title: 'Gps Tracking', icon: 'star', show: true, enabled: true },
]

export const ManualMenus = []
