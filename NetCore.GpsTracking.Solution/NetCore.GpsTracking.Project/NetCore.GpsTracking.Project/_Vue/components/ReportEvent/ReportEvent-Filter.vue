<template>
  <Index-Filter :Config="Config" :model="Filter" @Search="Search" @Reset="Reset" @SetFilterValue="SetFilterValue">
    <template slot="TopLeft">
      <td align="center">
        <DDL :model="Filter" Property="GpsDeviceId" Tip="Thiết bị" Width="120" Top="0" Left="0" Module="GpsTracking" Entity="GpsDevice" />
      </td>
      <td align="center">
        <DDL :model="Filter" Property="GroupId" Tip="Nhóm thiết bị" Width="120" Top="0" Left="0" Module="GpsTracking" Entity="Group" />
      </td>
      <td align="center">
        <DDL :model="Filter" Property="TypeId" Tip="Type" Width="200" Top="0" Left="0" :Data="Types" />
      </td>
      <td align="center">
        <DDL :model="Filter" Property="TimeRange" Tip="Time" Width="120" Top="0" Left="0" :Data="TimeRanges" />
      </td>
    </template>
  </Index-Filter>
</template>

<script>
  var lib = window.js.lib
  var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")
  var mixin_Filter = lib.GetComponent(window.index.all, "mixin-Filter")
  export default {
    mixins: [mixin_lib, mixin_Filter],

    data() {
      return {
        DefaultFilter: {
          GpsDeviceId: '',
          GroupId: '',
          TypeId: '',
          TimeRange: '',
          SendEmail: false,
        },

        TimeRanges: [
          { Value: 1, Name: "Hôm nay" },
          { Value: 2, Name: "Hôm qua" },
          { Value: 3, Name: "Tuần này" },
          { Value: 4, Name: "Tháng này" },
        ],
        Types: [
          { Value: 1, Name: "Online" },
          { Value: 2, Name: "Offline" },
          { Value: 3, Name: "Đi vào Khu vực" },
          { Value: 4, Name: "Ra khỏi Khu vực" },
          { Value: 5, Name: "Tiếp cận Thiết bị khác" },
        ],
      }
    },

    methods: {
      ReCorrectData() {
      },
      SetFilter() {
        let filterTemp = lib.CopyObject(this.Filter)
        this.$emit("SetFilter", filterTemp)
      },
    },
  }
</script>
