<template>
  <div>
    <div id="menu">
      <input id="styleGoogleMaps()" type="radio" name="rtoggle" @click="SetupMap(1)" />
      <label for="styleGoogleMaps()">Google Maps</label>
      <input id="styleGoogleMapsSatellite()" type="radio" name="rtoggle" @click="SetupMap(2)" />
      <label for="styleGoogleMapsSatellite()">Google Maps vệ tinh</label>
    </div>
    <div id='map' :style="MapStyle">
    </div>
    <a-modal v-model="ModalVisible" :title="ModalTitle" on-ok="ModalOnSubmit">
      <template slot="footer">
        <a-button key="back" @click="ModalOnCancel">
          Cancel
        </a-button>
        <a-button key="submit" type="primary" :loading="ModalSubmitting" @click="ModalOnSubmit">
          OK
        </a-button>
      </template>
      <a-input v-model="model.Name" placeholder="Tên"></a-input>
      <br /><br />
      <a-input v-model="model.Note" placeholder="Ghi chú"></a-input>
    </a-modal>
  </div>
</template>

<script>
  var lib = window.js.lib
  var mixin_lib = lib.GetComponent(window.index.all, "mixin-lib")

  import 'mapbox-gl/dist/mapbox-gl.css';
  import MapboxDraw from "@mapbox/mapbox-gl-draw";
  import '@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css';
  import {
    CircleMode,
    DragCircleMode,
    DirectMode,
    SimpleSelectMode
  } from 'mapbox-gl-draw-circle';
  import theme from '@mapbox/mapbox-gl-draw/src/lib/theme';
  import 'font-awesome/css/font-awesome.min.css';
  import { type } from 'jquery';

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

        ModalTitle: false,
        ModalVisible: false,
        ModalSubmitting: false,

        model: {
          Id: lib.Id0,
          Name: "",
          Note: "",
          Geofences: [],
        },
        GeofencesId: "",
      }
    },

    methods: {
      AutoRun() {
        var width = lib.ScreenWidth()
        if (!this.$store.state.Collapsed)
          width -= 216
        else
          width -= 50

        var height = lib.ScreenHeight()
        if (!this.$store.state.Collapsed)
          height -= 80

        this.MapStyle = {
          width: '100%', //width: width + 'px',
          height: height + 'px',
        }
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
          this.GetLocations()
          this.GetGeofences()
        }.bind(this));

        class extendDrawBar {
          constructor(opt) {
            let ctrl = this;
            ctrl.draw = opt.draw;
            ctrl.buttons = opt.buttons || [];
            ctrl.onAddOrig = opt.draw.onAdd;
            ctrl.onRemoveOrig = opt.draw.onRemove;
          }

          onAdd(map) {
            let ctrl = this;
            ctrl.map = map;
            ctrl.elContainer = ctrl.onAddOrig(map);
            ctrl.buttons.forEach((b) => {
              ctrl.addButton(b);
            });
            return ctrl.elContainer;
          }

          onRemove(map) {
            let ctrl = this;
            ctrl.buttons.forEach((b) => {
              ctrl.removeButton(b);
            });
            ctrl.onRemoveOrig(map);
          }

          addButton(opt) {
            let ctrl = this;
            var elButton = document.createElement('button');
            elButton.className = 'fa fa-circle o';
            if (opt.classes instanceof Array) {
              opt.classes.forEach((c) => {
                elButton.classList.add(c);
              });
            }
            elButton.addEventListener(opt.on, opt.action);
            ctrl.elContainer.appendChild(elButton);
            opt.elButton = elButton;
          }

          removeButton(opt) {
            opt.elButton.removeEventListener(opt.on, opt.action);
            opt.elButton.remove();
          }
        }

        const draw = new MapboxDraw({
          displayControlsDefault: false,
          controls: {
            polygon: true,
            trash: true,
          },
          styles: theme,
          modes: {
            ...MapboxDraw.modes,
            draw_circle: CircleMode,
            drag_circle: DragCircleMode,
            direct_select: DirectMode,
            simple_select: SimpleSelectMode
          },
          defaultMode: "simple_select",
          userProperties: true
        });

        var drawBar = new extendDrawBar({
          draw: draw,
          buttons: [
            {
              on: 'click',
              action: function circle() {
                draw.changeMode('drag_circle');
              },
              classes: []
            }
          ]
        });

        map.addControl(drawBar);

        //map.on('draw.delete', updateArea);
        //map.on('draw.update', updateArea);

        map.on('draw.create', function (e) {
          this.GeofencesId = e.features[0].id
          this.model.Geofences = e.features[0].geometry.coordinates[0]

          this.ModalTitle = "Giới hạn Địa lý !"
          this.ModalVisible = true
        }.bind(this));
      },
      SetLocation(title, id, coordinates) {
        map.loadImage('/Index/img/Icon/car.png', (err, image) => {
          if (err) {
            return;
          }
          map.addImage(id + '_image', image);

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
                    'title': title
                  }
                }
              ]
            }
          });
          map.addLayer({
            'id': id + '_image_layer',
            'type': 'symbol',
            'source': id + '_location',
            'layout': {
              'icon-image': id + '_image',
              'icon-size': 0.2,
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
        });
      },
      SetRoute(id, coordinates) {
        map.addSource(id + '_route', {
          'type': 'geojson',
          'data': {
            'type': 'Feature',
            'properties': {},
            'geometry': {
              'type': 'LineString',
              'coordinates': coordinates
            }
          }
        });
        map.addLayer({
          'id': id + '_route_layer',
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

        //Direction
        map.loadImage('/Index/img/Icon/direction.png', (err, image) => {
          if (err) {
            return;
          }
          map.addImage(id + '_direction', image);

          map.addLayer({
            'id': id + '_direction_layer',
            'type': 'symbol',
            'source': id + '_route',
            'layout': {
              'icon-allow-overlap': true,
              'symbol-placement': 'line',
              'symbol-spacing': 1,
              'icon-image': id + '_direction',
              'icon-size': 0.1
            }
          });
        });
      },
      SetGeofence(id, name, coordinates) {
        map.addSource(id, {
          'type': 'geojson',
          'data': {
            'type': 'Feature',
            'geometry': {
              'type': 'Polygon',
              'coordinates': [coordinates]
            }
          }
        });

        map.addLayer({
          'id': id,
          'type': 'fill',
          'source': id,
          'layout': {},
          'paint': {
            'fill-color': '#0080ff',
            'fill-opacity': 0.5
          }
        });

        map.addLayer({
          'id': id + '_outline',
          'type': 'line',
          'source': id,
          'layout': {},
          'paint': {
            'line-color': '#000',
            'line-width': 1
          }
        });

        map.addLayer({
          'source': id,
          'id': id + '_title',
          'type': 'symbol',
          'layout': {
            'text-field': name,
            'text-font': ['Roboto Regular'],
            'text-size': 18,
          },
          'paint': {
            'text-halo-color': 'white',
            'text-halo-width': 1,
          },
        });
      },

      async GetLocations() {
        var haveData = false

        try {
          var url = "/Api/GpsTracking/Location/GetLocations"
          var model = {}

          let response = await this.AjaxPOST(url, model)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {
              //console.log(response.data.Result.Models)
              haveData = true

              var Models = response.data.Result.Models
              for (var i = 0; i < Models.length; i++) {
                var item = Models[i]

                this.SetLocation(item.Code, item.Id, item.Last);
                this.SetRoute(item.Id, item.Locations);
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

        setTimeout(function () {
          this.GetLocations()
        }.bind(this), haveData ? 60000 : 1000);
      },
      async GetGeofences() {
        try {
          var url = "/Api/GpsTracking/Geofence/GetGeofences"
          var model = {}

          let response = await this.AjaxPOST(url, model)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {
              //console.log(response.data.Result.Models)

              var Models = response.data.Result.Models
              for (var i = 0; i < Models.length; i++) {
                var item = Models[i]
                this.SetGeofence(item.Id, item.Name, item.Geofences)
              }
            }
            else {
              this.MessageError("Get", "Geofences")
            }
          }
        }
        catch (err) {
          this.HandleError(err)
        }
      },

      async ModalOnSubmit(e) {
        try {
          var url = "/Api/GpsTracking/Geofence/Save"

          let response = await this.AjaxPOST(url, this.model)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {
              this.MessageOK("Save", "Geofence")
              this.ModalVisible = false;
            }
            else {
              this.MessageError("Save", "Geofence")
            }
          }
        }
        catch (err) {
          this.HandleError(err)
        }
      },
      ModalOnCancel(e) {
        this.ModalVisible = false;
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
