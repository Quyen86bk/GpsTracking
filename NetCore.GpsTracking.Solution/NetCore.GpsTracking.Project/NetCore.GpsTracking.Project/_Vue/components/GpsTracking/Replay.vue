<template>
  <div>
    <div id="menu" :style="MenuStyle">
      <table>
        <tr>
          <td align="center" valign="middle" style="padding-top: 12px;">
            <input id="styleGoogleMaps()" type="radio" name="rtoggle" @click="SetupMap(1)" />
            <label for="styleGoogleMaps()">Google Maps</label>
            <input id="styleGoogleMapsSatellite()" type="radio" name="rtoggle" @click="SetupMap(2)" />
            <label for="styleGoogleMapsSatellite()">Google Maps vệ tinh</label>
          </td>
          <td align="center" valign="middle">
            <DDL :model="Filter" Property="GpsDeviceId" Tip="Thiết bị" Width="120" Top="0" Left="0" Module="GpsTracking" Entity="GpsDevice" />
          </td>
          <td align="center" valign="middle">
            <DDL :model="Filter" Property="GroupId" Tip="Nhóm thiết bị" Width="120" Top="0" Left="0" Module="GpsTracking" Entity="Group" />
          </td>
          <td align="center" valign="middle">
            <DDL :model="Filter" Property="TimeRange" Tip="Thời gian" Width="120" Top="0" Left="0" :Data="TimeRanges" />
          </td>
          <td align="center" valign="middle">
            <a-button type="primary" @click="Search"><a-icon type="search"></a-icon></a-button>
          </td>
          <td>
            <div style="background-color: royalblue; color: white; border-radius: 7px;">
              <table>
                <tr>
                  <td>
                    <a-icon type="minus" @click="x1" />
                  </td>
                  <td>
                    <a @click="PlayPause" title="Play/Pause">
                      <svg width="18px" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
                        <path v-if="!Playing" fill="currentColor" d="M15,10.001c0,0.299-0.305,0.514-0.305,0.514l-8.561,5.303C5.51,16.227,5,15.924,5,15.149V4.852c0-0.777,0.51-1.078,1.135-0.67l8.561,5.305C14.695,9.487,15,9.702,15,10.001z" />
                        <path v-else fill="currentColor" d="M15,3h-2c-0.553,0-1,0.048-1,0.6v12.8c0,0.552,0.447,0.6,1,0.6h2c0.553,0,1-0.048,1-0.6V3.6C16,3.048,15.553,3,15,3z M7,3H5C4.447,3,4,3.048,4,3.6v12.8C4,16.952,4.447,17,5,17h2c0.553,0,1-0.048,1-0.6V3.6C8,3.048,7.553,3,7,3z" />
                      </svg>
                    </a>
                  </td>
                  <td>
                    <a-icon type="plus" @click="x2" />
                  </td>
                  <td align="center" style="width: 50px;">
                    <span>{{Step}}x</span>
                  </td>
                  <td style="width: 400px;">
                    <a-slider v-model="PointIndex" :min="-1" :max="Points.length - 1" :step="1" :tip-formatter="TipFormatter" />
                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
      </table>
    </div>
    <div id='map' :style="MapStyle">
    </div>
  </div>
</template>

<script>
  var lib = window.js.lib
  var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")

  var map;
  var mapboxgl = require('mapbox-gl/dist/mapbox-gl.js');
  mapboxgl.accessToken = 'pk.eyJ1IjoiZHVjaHV5Yms5NyIsImEiOiJja3NocTdiMHMxeDA4MnRuNWs3MnFtanowIn0.ecGxERlGXkTbndJDRPCEIQ';

  var styleCustom = (url, attribution) => ({
    version: 8,
    sources: {
      osm: {
        type: 'raster',
        tiles: [url],
        attribution: attribution,
        tileSize: 256,
      },
    },
    glyphs: 'https://cdn.traccar.com/map/fonts/{fontstack}/{range}.pbf',
    layers: [{
      id: 'osm',
      type: 'raster',
      source: 'osm',
    }],
  });
  var styleGoogleMaps = () => styleCustom(
    'https://mt0.google.com/vt/lyrs=m&hl=en&x={x}&y={y}&z={z}&s=Ga',
    '© <a target="_top" rel="noopener" href="https://www.google.com/maps/">Google Maps</a>',
  );
  var styleGoogleMapsSatellite = () => styleCustom(
    'https://mt0.google.com/vt/lyrs=y&hl=en&x={x}&y={y}&z={z}&s=Ga',
    '© <a target="_top" rel="noopener" href="https://www.google.com/maps/">Google Maps</a>',
  );

  export default {
    mixins: [mixin_lib],

    data() {
      return {
        MapStyle: {
          width: '100%', //width: (lib.ScreenWidth() - 216) + 'px',
          height: lib.ScreenHeight() + 'px',
        },
        MenuStyle: {
          position: 'fixed',
          top: 70 + 'px',
          "z-index": 999999,
        },

        Filter: {
          GpsDeviceId: '',
          GroupId: '',
          TimeRange: '',
        },

        TimeRanges: [
          { Value: 1, Name: "Hôm nay" },
          { Value: 2, Name: "Hôm qua" },
          { Value: 3, Name: "Tuần này" },
          { Value: 4, Name: "Tháng này" },
        ],

        AddedIDs: [],
        LastLocations: [],

        Routes: [],
        Points: [],
        PointIndex: -1,

        Playing: false,
        TimerPlay: null,
        Step: 1,
      }
    },

    methods: {
      AutoRun() {
        var width = lib.ScreenWidth()
        if (!this.$store.state.Collapsed) {
          width -= 216
          this.MenuStyle.top = 70 + "px"
        }
        else {
          width -= 50
          this.MenuStyle.top = 0 + "px"
        }

        var height = lib.ScreenHeight()
        if (!this.$store.state.Collapsed)
          height -= 80

        this.MapStyle = {
          width: '100%', //width: width + 'px',
          height: height + 'px',
        }
      },
      GetGuid() {
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
          (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        );
      },

      SetupMap(typeId) {

        //Map type
        if (typeId == 1)
          map = new mapboxgl.Map({
            container: 'map',
            style: styleGoogleMaps(),
            center: [105.7703151, 21.0337226],
            zoom: 15,
          });
        else if (typeId == 2)
          map = new mapboxgl.Map({
            container: 'map',
            style: styleGoogleMapsSatellite(),
            center: [105.7703151, 21.0337226],
            zoom: 15,
          });

        //onload
        map.on('load', function (e) {
          this.GetReplays()
        }.bind(this));
      },

      SetLocation(title, id, longitude, latitude, category, status) {
        id = id + "_Location"
        this.AddedIDs.push({ Id: id, Type: 1 })

        // Display Update

        var exists = false
        var change = true

        for (var i = 0; i < this.LastLocations.length; i++) {
          if (this.LastLocations[i].Id == id) {
            exists = true;
            break;
          }
        }

        if (!exists) {
          //not exists - add new
          this.LastLocations.push({ Id: id, Longitude: longitude, Latitude: latitude })
          change = true
        }
        else {
          //check change
          for (var i = 0; i < this.LastLocations.length; i++) {
            if (this.LastLocations[i].Id == id && this.LastLocations[i].Longitude == longitude && this.LastLocations[i].Latitude == latitude) {
              change = false;
              break;
            }
          }

          //update last location
          for (var i = 0; i < this.LastLocations.length; i++) {
            if (this.LastLocations[i].Id == id) {
              this.LastLocations[i].Longitude = longitude
              this.LastLocations[i].Latitude = latitude
              break;
            }
          }
        }

        if (change) {
          if (map.getSource(id + '_location')) {
            map.removeLayer(id + "_layer")
            map.removeSource(id + '_location')
          }
        }
        else {
          return
        }

        // Display Icon

        var imgId = id + this.GetGuid()
        var categoryImg = '/Index/img/Icon/' + category + '-' + status + '.png'
        map.loadImage(categoryImg, (err, image) => {
          if (err) {
            return;
          }
          try {
            map.addImage(imgId + '_image', image);
          }
          catch (ex) {
          }
        });

        map.addSource(id + '_location', {
          'type': 'geojson',
          'data': {
            'type': 'FeatureCollection',
            'features': [
              {
                'type': 'Feature',
                'geometry': {
                  'type': 'Point',
                  'coordinates': [longitude, latitude],
                },
                'properties': {
                  'description': '<strong>Thiết bị:</strong>' + title + '<p>Toạ độ:</p>' + [longitude, latitude],
                  'title': title
                }
              }
            ]
          }
        });

        map.addLayer({
          'id': id + '_layer',
          'type': 'symbol',
          'source': id + '_location',
          'layout': {
            'icon-image': imgId + '_image',
            'icon-size': 0.15,
            'text-allow-overlap': true,
            'text-anchor': 'bottom',
            'text-field': ['get', 'title'],
            'text-font': ['Roboto Regular'],
            'text-offset': [0, -2],
            'text-size': 12,
          },
          paint: {
            'text-halo-color': 'white',
            'text-halo-width': 1,
          },
        });

        // Display Popup

        map.on('click', id + '_layer', (e) => {
          const coordinates = e.features[0].geometry.coordinates.slice();
          const description = e.features[0].properties.description;

          while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
            coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
          }

          new mapboxgl.Popup()
            .setLngLat(coordinates)
            .setHTML(description)
            .addTo(map);
        });
      },
      SetRoute(id, coordinates) {
        id = id + "_Replay"
        this.AddedIDs.push({ Id: id, Type: 2 })

        if (map.getSource(id + '_route')) {
          map.removeLayer(id + '_direction_layer')
          map.removeLayer(id + '_layer')
          map.removeSource(id + '_route')
        }

        map.addSource(id + '_route', {
          'type': 'geojson',
          'data': {
            'type': 'Feature',
            'properties': {},
            'geometry': {
              'type': 'LineString',
              'coordinates': coordinates,
            }
          },
        });

        map.addLayer({
          'id': id + '_layer',
          'type': 'line',
          'source': id + '_route',
          'layout': {
            'line-join': 'round',
            'line-cap': 'round'
          },
          'paint': {
            'line-color': '#4287F5',
            'line-width': {
              'base': 1.5,
              'stops': [[1, 0.5], [8, 3], [15, 6], [22, 8]],
            },
          }
        });

        // Add direction
        var imgId = id + this.GetGuid()
        map.loadImage('/Index/img/Icon/direction.png', (err, image) => {
          if (err) {
            return;
          }
          map.addImage(imgId + '_direction', image);

          map.addLayer({
            'id': id + '_direction_layer',
            'type': 'symbol',
            'source': id + '_route',
            'layout': {
              'icon-allow-overlap': true,
              'symbol-placement': 'line',
              'symbol-spacing': 1,
              'icon-image': imgId + '_direction',
              'icon-size': 0.1
            }
          });
        });
      },

      TipFormatter(value) {
        var result = null
        if (value > -1 && value < this.Points.length) {
          result = this.Points[value].Time
        }
        return result
      },

      x1() {
        if (this.Step > 0.125)
          this.Step = this.Step / 2
        this.ResetInterval()
      },
      x2() {
        if (this.Step < 128)
          this.Step = this.Step * 2
        this.ResetInterval()
      },

      PlayPause() {
        this.Playing = !this.Playing

        if (this.Playing) {
          for (var i = 0; i < this.Routes.length; i++) {
            var item = this.Routes[i]
            this.SetRoute(item.Id, item.Locations);
          }

          if (this.PointIndex == this.Points.length - 1)
            this.PointIndex = -1

          this.ResetInterval()
        }
        else {
          if (lib.NotNull(this.TimerPlay))
            clearInterval(this.TimerPlay)
        }
      },

      ResetInterval() {
        if (lib.NotNull(this.TimerPlay))
          clearInterval(this.TimerPlay)

        this.TimerPlay = setInterval(function () {
          if (this.PointIndex < this.Points.length - 1)
            this.PointIndex++

          var point = this.Points[this.PointIndex]
          this.SetLocation(point.Name, point.Id, [point.Longitude, point.Latitude], point.CategoryId, point.StatusId);

          if (this.PointIndex == this.Points.length - 1) {
            this.Playing = false
            clearInterval(this.TimerPlay)
          }
        }.bind(this), 1000 / this.Step);
      },

      async Search() {
        this.Playing = false

        if (lib.NotNull(this.TimerPlay))
          clearInterval(this.TimerPlay)

        this.PointIndex = -1

        this.ClearReplays()
        await this.GetReplays()

        this.PlayPause()
      },
      ClearReplays() {
        for (var i = 0; i < this.AddedIDs.length; i++) {
          var added = this.AddedIDs[i]

          try {
            if (added.Type == 1) {
              if (map.getSource(added.Id + '_location')) {
                map.removeLayer(added.Id + "_layer")
                map.removeSource(added.Id + "_location")
              }
            }
            else {
              if (map.getSource(added.Id + '_route')) {
                map.removeLayer(added.Id + '_direction_layer')
                map.removeLayer(added.Id + '_layer')
                map.removeSource(added.Id + '_route')
              }
            }
          }
          catch (ex) {
          }
        }
        this.AddedIDs = []
      },

      async GetReplays() {
        try {
          var url = "/Api/GpsTracking/Location/GetReplays"

          let response = await this.AjaxPOST(url, this.Filter)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {
              this.Points = response.data.Result.Points
              this.Routes = response.data.Result.Routes
            }
            else {
              this.MessageError("Get", "Locations")
            }
          }
        }
        catch (err) {
          this.HandleError(err)
        }
      },
    },

    async created() {
      this.StartAutoRun()
    },
    mounted: function () {
      this.SetupMap(1);
    }
  }
</script>
