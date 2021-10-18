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
          this.GetHeatmaps()
        }.bind(this));
      },

      SetLocation(title, id, coordinates, category, status) {
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
          this.LastLocations.push({ Id: id, Longitude: coordinates[0], Latitude: coordinates[1] })
          change = true
        }
        else {
          //check change
          for (var i = 0; i < this.LastLocations.length; i++) {
            if (this.LastLocations[i].Id == id && this.LastLocations[i].Longitude == coordinates[0] && this.LastLocations[i].Latitude == coordinates[1]) {
              change = false;
              break;
            }
          }

          //update last location
          for (var i = 0; i < this.LastLocations.length; i++) {
            if (this.LastLocations[i].Id == id) {
              this.LastLocations[i].Longitude = coordinates[0]
              this.LastLocations[i].Latitude = coordinates[1]
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
                  'coordinates': coordinates
                },
                'properties': {
                  'description': '<strong>Thiết bị:</strong>' + title + '<p>Toạ độ:</p>' + coordinates,
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
      SetHeatmap(id, coordinates) {
        id = id + "_Heatmap"
        this.AddedIDs.push({ Id: id, Type: 2 })

        if (map.getSource(id + '_heatmap'))
          return

        map.addSource(id + '_heatmap', {
          'type': 'geojson',
          'data': {
            type: 'FeatureCollection',
            features: [],
          },
          'cluster': true,
          'clusterMaxZoom': 14,
          'clusterRadius': 10,
        });

        map.getSource(id + '_heatmap').setData({
          type: 'FeatureCollection',
          features: coordinates.map(location => ({
            type: 'Feature',
            geometry: {
              type: 'Point',
              coordinates: [location[0], location[1]],
            },
          }))
        });

        map.addLayer({
          id: id + '_clusters',
          type: 'circle',
          source: id + '_heatmap',
          filter: ['has', 'point_count'],
          paint: {
            'circle-color': [
              'step',
              ['get', 'point_count'],
              '#00F000', // Dưới 20 điểm có màu xanh
              20,
              '#F1ED02', // 20 điểm đến 99 có màu vàng
              100,
              '#FF0C01' // 100 điểm trở lên là màu đỏ
            ],
            'circle-radius': [
              'step',
              ['get', 'point_count'],
              20, // Dưới 20 điểm, đường tròn 20
              20,
              40, // Dưới 100 điểm, đường tròn 40
              100,
              80 // Trên 100 điểm, đường tròn 40
            ],
            'circle-blur': 0.5,
            'circle-opacity': [
              'step',
              ['get', 'point_count'],
              0.7,
              20,
              0.8,
              50,
              0.9
            ],
            // 'circle-stroke-width': 2,
            // 'circle-stroke-color': '#000'
          }
        });

        map.addLayer({
          id: id + '_unclustered-point',
          type: 'circle',
          source: id + '_heatmap',
          filter: ['!', ['has', 'point_count']],
          paint: {
            'circle-color': '#00F000',
            'circle-radius': 4,
            'circle-opacity': 0.5,
            // 'circle-stroke-width': 1,
            // 'circle-stroke-color': '#000'
          }
        });

        map.on('click', id + '_clusters', (e) => {
          const features = map.queryRenderedFeatures(e.point, {
            layers: [id + '_clusters']
          });
          const clusterId = features[0].properties.cluster_id;
          map.getSource(id).getClusterExpansionZoom(
            clusterId,
            (err, zoom) => {
              if (err) return;
              map.easeTo({
                center: features[0].geometry.coordinates,
                zoom: zoom
              });
            }
          );
        });

        map.on('mouseenter', id + '_clusters', () => {
          map.getCanvas().style.cursor = 'pointer';
        });

        map.on('mouseleave', id + '_clusters', () => {
          map.getCanvas().style.cursor = '';
        });
      },

      async Search() {
        this.ClearHeatmaps()
        this.GetHeatmaps()
      },
      ClearHeatmaps() {
        for (var i = 0; i < this.AddedIDs.length; i++) {
          var added = this.AddedIDs[i]

          try {
            if (added.Type == 1) {
              map.removeLayer(added.Id + "_layer")
              map.removeSource(added.Id + "_location")
            }
            else {
              map.removeLayer(added.Id + '_unclustered-point')
              map.removeLayer(added.Id + '_clusters')
              map.removeSource(added.Id + '_heatmap')
            }
          }
          catch (ex) {
          }
        }
        this.AddedIDs = []
      },

      async GetHeatmaps() {
        try {
          var url = "/Api/GpsTracking/Location/GetHeatmaps"

          let response = await this.AjaxPOST(url, this.Filter)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {

              var Models = response.data.Result.Models
              for (var i = 0; i < Models.length; i++) {
                var item = Models[i]

                this.SetLocation(item.Name, item.Id, item.Last, item.CategoryId, item.StatusId);
                this.SetHeatmap(item.Id, item.Locations);
              }
            }
            else {
              this.MessageError("Get", "Locations")
            }
          }
        }
        catch (err) {
          this.HandleError(err)
        }

        //setTimeout(function () {
        //  this.GetHeatmaps()
        //}.bind(this), 1000);
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
