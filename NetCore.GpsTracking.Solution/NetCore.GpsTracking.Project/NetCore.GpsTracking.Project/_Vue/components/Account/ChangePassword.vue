<template>
    <div>
        <center>
            <h1>{{Lang('Change password')}}.</h1>
            <div style="width: 480px;">
                <Modelx LBL Title="Full name" :Label="User.FullName" Width1="220" First />
                <Modelx LBL Title="User name" :Label="User.UserName" Width1="220" />
                <Modelx TBX :model="model" Property="Password" Title="Current password" Width1="220" Must Min="6" Max="30" Type="New-Password" />
                <Modelx TBX :model="model" Property="NewPassword" Title="New password (6-30 characters)" Width1="220" Must Min="6" Max="30" Type="New-Password" />
                <Modelx TBX :model="model" Property="RetypeNewPassword" Title="Retype new password" Width1="220" Must Min="6" Max="30" Type="New-Password" /> <!--EQName="New password" :EQValue="model.NewPassword"-->
                <Modelx Any Title="@No@" Width1="220">
                    <a-spin v-if="Submiting" tip="Loading..." />
                    <a-button v-else :disabled="Done" type="primary" @click="Submit">{{ Lang(!Done ? 'Change password...' : 'Password changed successfully !') }}</a-button>
                </Modelx>
            </div>
        </center>
    </div>
</template>

<script>
    var lib = window.js.lib
    var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")
    export default {
        mixins: [mixin_lib],

        data() {
            return {
                model: {
                    Password: "",
                    NewPassword: "",
                    RetypeNewPassword: "",
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

                    if (!lib.ValidPassword(this.model.Password) || !lib.ValidPassword(this.model.NewPassword) || !lib.ValidPassword(this.model.RetypeNewPassword)) {
                        valid = false
                        message += this.Lang("Must input Password from 6 to 30 characters.") + "\r\n"
                    }

                    if (this.model.NewPassword != this.model.RetypeNewPassword) {
                        valid = false
                        message += this.Lang("Retype new password must equal New password.") + "\r\n"
                    }

                    if (!valid)
                        this.MessageErrorAny(message)
                    else {
                        this.Submiting = true
                        var url = "/Api/Manage/Account/ChangePassword"

                        let response = await this.AjaxPOST(url, this.model)
                        this.Submiting = false

                        if (this.HandleResponse(response)) {

                            if (response.data.Result.Status == -1) {
                                this.MessageErrorAny(this.Lang("Error !"))
                            }
                            else
                                if (response.data.Result.Status == 0) {
                                    this.MessageErrorAny(this.Lang("Incorrect current password !"))
                                }
                                else if (response.data.Result.Status == 1) {
                                    this.MessageOKAny(this.Lang("Password changed !\r\nAuto go back in 3 seconds..."))
                                    this.Done = true

                                    setTimeout(function () {
                                        lib.GoBack()
                                    }.bind(this), 1000 * 3)
                                }
                        }
                    }
                }
                catch (err) {
                    this.HandleError(err)
                    this.Submiting = false
                }
            },
        },
    }
</script>
