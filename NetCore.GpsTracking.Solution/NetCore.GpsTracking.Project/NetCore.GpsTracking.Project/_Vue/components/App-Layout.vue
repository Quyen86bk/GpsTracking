<template>
  <div>
    <a-spin v-if="Loading" tip="Loading..." size="large" :style="LoadingStyle()" />
    <template v-if="!IsLoggedIn">
      <router-view></router-view>
    </template>
    <template v-else>
      <a-layout :style="{ minHeight: ScreenHeight + 'px' }">
        <a-layout-sider class="SiderBG" v-model="Collapsed" collapsible @collapse="OnCollapse">

          <div id="Logo">
            <a href="/">
              <template v-if="NotNull(UI.Logo.Image.Full)">
                <img v-if="!Collapsed" :src="UI.Logo.Image.Full">
                <img v-else :src="UI.Logo.Image.Short">
              </template>
              <template v-else>
                <span v-if="!Collapsed">{{UI.Logo.Full}}</span>
                <span v-else>{{UI.Logo.Short}}</span>
              </template>
            </a>
          </div>
          <Menux :Routes="NiceRoutes" :Menus="Menus" @OnRouteClick="OnRouteClick" />
        </a-layout-sider>
        <a-layout>
          <a-layout-header v-show="!Collapsed" style="background: #fff; padding: 0;">
            <div style="float: left">
              <div class="TableCell">
                <a-icon id="btnToggleCollapse" :type="Collapsed ? 'double-right' : 'double-left'" @click="ToggleCollapsed" style="display: none;" />
              </div>
              <div id="AppName" class="TableCell S20" style="padding-left: 16px;">
                {{Lang(UI.AppName)}} {{ Lang('version') }} {{UI.Version}}
              </div>
            </div>

            <div style="float: right">
              <div class="TableCell">
                <a-dropdown v-if="User" style="padding-right: 20px;">
                  <a class="ant-dropdown-link" href="#" onclick="return false;">
                    <a-avatar id="Avatar" style="background-color:#87d068; margin-bottom: 5px;" icon="user" />
                    <template v-if="NotNull(User.FullName)">
                      {{ User.FullName }}
                    </template>
                    <template v-else>
                      {{ User.UserName }}
                    </template>
                  </a>
                  <a-menu slot="overlay">
                    <a-menu-item>
                      <a @click="Profile">
                        <a-icon type="user" /><span style="padding-left: 5px;">{{ Lang('Profile') }}</span>
                      </a>
                    </a-menu-item>
                    <a-menu-item>
                      <a @click="ChangePassword">
                        <a-icon type="lock" /><span style="padding-left: 5px;">{{ Lang('Change password') }}</span>
                      </a>
                    </a-menu-item>
                    <a-menu-item>
                      <a @click="Logout">
                        <a-icon type="poweroff" /><span style="padding-left: 5px;">{{ Lang('Logout') }}...</span>
                      </a>
                    </a-menu-item>
                  </a-menu>
                </a-dropdown>

                <a-dropdown style="padding-right: 20px;">
                  <a class="ant-dropdown-link" href="#" onclick="return false;">
                    <img :src="GetLang().Icon" style="width: 32px !important; padding-bottom: 5px !important"> Language <!--{{GetLang().Name}}-->
                  </a>
                  <a-menu slot="overlay">
                    <a-menu-item v-for="(item) in Langs">
                      <a @click="SetLang(item)">
                        <span style="padding-left: 5px;">{{ item.Name }}</span>
                      </a>
                    </a-menu-item>
                  </a-menu>
                </a-dropdown>
              </div>
            </div>
          </a-layout-header>
          <a-layout-content :style="ContentStyle">
            <router-view :key="RouterViewRenderKey"></router-view>
            <div id="EndPage"></div>
          </a-layout-content>
        </a-layout>
      </a-layout>
    </template>
    <div id="btnHidden" style="display:none">
      <div id="btnWindowResize" @click="OnWindowResize"></div>
      <div id="btnWindowUnload" @click="OnWindowUnload"></div>
      <div id="btnFullScreen" @click="FullScreen"></div>
      <div id="btnShowLoading" @click="ShowLoading"></div>
      <div id="btnHideLoading" @click="HideLoading"></div>
    </div>
  </div>
</template>

<script>
  var lib = window.js.lib
  var session = window.js.session

  var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")
  var mixin_App_Layout = lib.GetComponent(window.index.all, "mixin-App-Layout")
  export default {
    mixins: [mixin_lib, mixin_App_Layout],

    computed: {
      ContentStyle() {
        var result = {
          margin: '0px 0px 0px 0px',
          padding: '16px 0px 0px 16px',
        }

        if (this.Collapsed)
          result = {
            margin: '0px 0px 0px 0px',
            padding: '0px 0px 0px 16px',
          }

        return result
      },
    },

    data() {
      return {
      }
    },

    methods: {
      AutoRun() {
        this.$store.commit('OnCollapse', this.Collapsed)
      },

      Logout() {
        session.Logout()
        lib.GoHome()
      },
      ChangePassword() {
        lib.GoChangePassword()
      },

      Profile() {
        lib.GoTo("/Profile")
      },
    },

    async created() {
      this.StartAutoRun();
    },
  }
</script>
