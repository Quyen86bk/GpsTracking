<template>
    <div :id="ContentId" v-disable="Disable">
        <Modelx TBX :model="model" Property="Name" :Tip="Lang('ReportLocation')" First Must />
    </div>
</template>

<script>
    var lib = window.js.lib
    var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")
    var mixin_Model_Data = lib.GetComponent(window.index.all, "mixin-Model-Data")
    export default {
        mixins: [mixin_lib, mixin_Model_Data],

        methods: {
            async CheckInputed() {
                var inputed = this.Selected(this.model.Name)

                this.$emit("SetInputed", inputed)
                this.Inputed = inputed
            },
        },
    }
</script>

<style>
</style>
