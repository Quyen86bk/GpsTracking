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
        <DDL :model="Filter" Property="TimeRange" Tip="Thời gian" Width="120" Top="0" Left="0" :Data="TimeRanges" />
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
          TimeRange: '',
          SendEmail: false,
        },

        TimeRanges: [
          { Value: 1, Name: "Hôm nay" },
          { Value: 2, Name: "Hôm qua" },
          { Value: 3, Name: "Tuần này" },
          { Value: 4, Name: "Tháng này" },
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
