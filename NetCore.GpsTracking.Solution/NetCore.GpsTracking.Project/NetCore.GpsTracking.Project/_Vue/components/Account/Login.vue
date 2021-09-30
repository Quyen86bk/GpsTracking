<template>
    <center>
        <div class="container-scroller">
            <div class="container-fluid page-body-wrapper full-page-wrapper">
                <div class="content-wrapper auth p-0 theme-two">
                    <div class="row d-flex align-items-stretch">
                        <div class="col-md-4 banner-section d-none d-md-flex align-items-stretch justify-content-center" style="padding: 0px;">
                            <div class="slide-content bg-1" :style="{ height: ScreenHeight + 'px' }"></div>
                        </div>
                        <div class="col-12 col-md-8 h-100 bg-white" style="padding: 0px;">
                            <div class="auto-form-wrapper d-flex align-items-center justify-content-center flex-column" :style="{ height: ScreenHeight + 'px' }">

                                <a-form id="LoginForm" :form="form" @submit.prevent="Submit()" style="text-align: left;">
                                    <img :src="UI.Logo.Login" />
                                    <br />
                                    <br />
                                    <br />
                                    <a-form-item>
                                        <a-input v-model="UserName" placeholder="User" v-decorator="['UserName', { rules: UserNameRules }]">
                                            <a-icon slot="prefix" type="user" style="color:rgba(0,0,0,.25)" />
                                        </a-input>
                                    </a-form-item>

                                    <a-form-item>
                                        <a-input type="password" v-model="Password" placeholder="Password" maxlength="30" v-decorator="['Password', { rules: PasswordRules }]">
                                            <a-icon slot="prefix" type="lock" style="color:rgba(0,0,0,.25)" />
                                        </a-input>
                                    </a-form-item>

                                    <a-form-item>
                                        <a-spin v-if="Submiting" tip="Loading..." />
                                        <a-button v-else type="primary" html-type="submit">Login</a-button>
                                    </a-form-item>

                                    <div class="wrapper mt-5 text-gray">
                                        <p class="footer-text">
                                            Copyright © 2021 by {{ UI.Copyright }}. All rights reserved.
                                        </p>
                                    </div>
                                </a-form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </center>
</template>

<script>
    var lib = window.js.lib
    var session = window.js.session

    var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")

    const formItemLayout = {
        labelCol: { span: 6 },
        wrapperCol: { span: 12 },
    };
    const formTailLayout = {
        labelCol: { span: 6 },
        wrapperCol: { span: 30, offset: 6 },
    };

    export default {
        mixins: [mixin_lib],

        data() {
            return {
                formLayout: 'horizontal',
                form: this.$form.createForm(this, { name: 'coordinated' }),

                UserNameRules: [
                    { required: true, message: 'Please input your User !' },
                    lib.GetAppConfig().UseEmail ? { type: 'email', message: 'The input is not valid E-mail !' } : {},
                ],
                PasswordRules: [
                    { required: true, message: 'Please input your Password !' },
                    { min: 6, message: 'Password must >= 6 characters !' },
                    { max: 30, message: 'Password must <= 30 characters !' },
                ],

                formItemLayout,
                formTailLayout,

                Submiting: false,

                UserName: "",
                Password: "",
            }
        },

        methods: {
            async Submit() {
                try {
                    var valid = false
                    this.form.validateFields((err, values) => {
                        valid = !err
                    });

                    if (valid) {
                        this.Submiting = true;

                        var url = "/Api/Manage/Account/Login"
                        var model = { UserName: this.UserName, Password: this.Password }

                        let response = await this.AjaxPOST(url, model)
                        this.Submiting = false;

                        if (this.HandleResponse(response)) {

                            if (response.data.Succeeded) {
                                localStorage.setItem('UserLoggedIn', JSON.stringify(response.data.UserLoggedIn))

                                if (response.data.UserLoggedIn.NeedChangePassword)
                                    lib.GoChangePassword()
                                else
                                    lib.GoBack()
                            }
                            else {
                                this.MessageErrorAny(this.Lang("Incorrect: User or Password..."))
                            }
                        }
                    }
                }
                catch (err) {
                    this.HandleError(err)
                    this.Submiting = false;
                }
            },
        },

        async created() {
            this.HideLoading()
            session.Logout()
        }
    }
</script>

<style>
    .auth.theme-two .banner-section {
        padding-right: 0;
    }

        .auth.theme-two .banner-section .slide-content {
            width: 100%;
        }

            .auth.theme-two .banner-section .slide-content.bg-1 {
                background: url("/Index/img/Login.jpg?1") no-repeat center center;
                background-size: cover;
                background-position: inherit !important;
            }
</style>
