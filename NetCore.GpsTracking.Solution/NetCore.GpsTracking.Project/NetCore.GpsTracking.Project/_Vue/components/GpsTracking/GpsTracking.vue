<template>
  <div>
    <div style="position: relative; z-index: 9; background-color: white; border-radius: 7px; width: 480px;">
      <table>
        <tr>
          <td align="center" valign="middle" style="padding-top: 12px;">
            <a-icon v-if="Selected(TabNumber)" :onclick="'parent.CloseTab(' + this.TabNumber + ');'" type="close-circle" style="font-size: 16px; color: red; padding-right: 5px; padding-left: 5px;" />
            <input id="styleGoogleMaps" type="radio" checked name="rtoggle" @click="SetMapStyle(1)" />
            <label for="styleGoogleMaps">Google Maps</label>
            <input id="styleGoogleMapsSatellite" type="radio" name="rtoggle" @click="SetMapStyle(2)" />
            <label for="styleGoogleMapsSatellite">Vệ tinh</label>
            <a-switch default-checked v-model="IsLiveRoute" @change="LiveRouteOnChange" style="font-size: 16px; padding-right: 5px; padding-left: 15px;"></a-switch> <span class="Blue">Live Route</span>
          </td>
          <td align="center" valign="middle">
            <DDL :model="Filter" Property="GpsDeviceId" Tip="Thiết bị" Width="120" Top="0" Left="0" Module="GpsTracking" Entity="GpsDevice" @OnChange="GetLocationsByDevice" style="padding-right: 5px; padding-left: 15px;"/>
          </td>
        </tr>
      </table>
    </div>
    <div :id="'map' + TabNumber" :style="MapStyle">
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
          'position': 'relative',
          'top': '-44px',
          width: lib.NotSelected(this.Width) ? '100%' : this.Width + 'px', //width: (lib.ScreenWidth() - 216) + 'px',
          height: lib.NotSelected(this.Height) ? lib.ScreenHeight() + 'px' : this.Height + 'px',
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

        AddedIDs: [],
        LastLocations: [],
        LastEvents: [],
        LastEventGeofences: [],
        LastEventDistances: [],

        IsLiveRoute: false,

        Filter: {
          TabNumber: 0,
          GpsDeviceId: '',
          GroupId: '',
        },

        MapTypeId: 1,
        CenterLongitude: 105.7703151,
        CenterLatitude: 21.0337226,

        TabNumber: null,
        Width: null,
        Height: null,
      }
    },

    methods: {
      AutoRun() {
        var width = this.Width
        if (lib.NotSelected(this.Width)) {
          width = lib.ScreenWidth()
          if (!this.$store.state.Collapsed)
            width -= 236
          else
            width -= 50
        }

        var height = this.Height
        if (lib.NotSelected(this.Height)) {
          height = lib.ScreenHeight()
          if (!this.$store.state.Collapsed)
            height -= 80
        }

        this.MapStyle.width = width + 'px'
        this.MapStyle.height = height + 'px'
      },
      GetGuid() {
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
          (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        );
      },

      SetMapStyle(typeId) {
        this.MapTypeId = typeId
        if (typeId == 1)
          map.setStyle(styleGoogleMaps());
        else if (typeId == 2)
          map.setStyle(styleGoogleMapsSatellite());
      },
      SetupMap(typeId, getData) {
        this.MapTypeId = typeId

        //Map type
        if (typeId == 1)
          map = new mapboxgl.Map({
            container: 'map' + this.TabNumber,
            style: styleGoogleMaps(),
            center: [this.CenterLongitude, this.CenterLatitude],
            zoom: 15,
          });
        else if (typeId == 2)
          map = new mapboxgl.Map({
            container: 'map' + this.TabNumber,
            style: styleGoogleMapsSatellite(),
            center: [this.CenterLongitude, this.CenterLatitude],
            zoom: 15,
          });

        if (getData)
          map.on('style.load', function () {

            const waiting = function () {
              if (!map.isStyleLoaded()) {
                setTimeout(waiting, 500);
              } else {
                this.GetLocations()
                this.GetGeofences()
              }
            }.bind(this);

            waiting();
          }.bind(this));

        if (lib.NotSelected(this.TabNumber)) {
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

          map.on('draw.create', function (e) {
            this.GeofencesId = e.features[0].id
            this.model.Geofences = e.features[0].geometry.longitude

            this.ModalTitle = "Giới hạn Địa lý !"
            this.ModalVisible = true
          }.bind(this));
        }
      },
      LiveRouteOnChange() {
        if (!this.IsLiveRoute)
          this.ClearRoute()
      },

      EventMessage(id, name, eventname) {

        var eventsExists = false
        var eventsChange = true

        for (var i = 0; i < this.LastEvents.length; i++) {
          if (this.LastEvents[i].Id == id) {
            eventsExists = true;
            break;
          }
        }

        if (!eventsExists) {
          //not exists - add new
          this.LastEvents.push({ Id: id, Name: name, EventTypeName: eventname })
          eventsChange = true
        }

        else {
          //check change
          for (var i = 0; i < this.LastEvents.length; i++) {
            if (this.LastEvents[i].Id == id && this.LastEvents[i].Name == name && this.LastEvents[i].EventTypeName == eventname) {
              eventsChange = false;
              break;
            }
          }

          //update last event
          for (var i = 0; i < this.LastEvents.length; i++) {
            if (this.LastEvents[i].Id == id) {
              this.LastEvents[i].Name = name
              this.LastEvents[i].EventTypeName = eventname
              break;
            }
          }
        }

        if (eventsExists && eventsChange)
          this.$message.info(name + ": " + eventname)
      },

      EventGeofenceMessage(id, name, eventGeofences) {

        var message = name + ":\r\n";

        var eventsExists = false
        var eventsChange = false

        for (var i = 0; i < this.LastEventGeofences.length; i++) {
          if (this.LastEventGeofences[i].Id == id) {
            eventsExists = true;
            break;
          }
        }

        if (!eventsExists) {
          //not exists - add new
          this.LastEventGeofences.push({ Id: id, Name: name, EventGeofences: eventGeofences })

          for (var k = 0; k < eventGeofences.length; k++) {
            message += " - " + eventGeofences[k].Event + ": " + eventGeofences[k].Geofence + "\r\n"
          }

          eventsChange = true
        }

        else {
          //check change
          for (var i = 0; i < this.LastEventGeofences.length; i++) {
            if (this.LastEventGeofences[i].Id == id && this.LastEventGeofences[i].Name == name) {

              for (var j = 0; j < this.LastEventGeofences[i].EventGeofences.length; j++) {
                for (var k = 0; k < eventGeofences.length; k++) {

                  if (this.LastEventGeofences[i].EventGeofences[j].Geofence == eventGeofences[k].Geofence) {
                    if (this.LastEventGeofences[i].EventGeofences[j].Event != eventGeofences[k].Event) {
                      message += " - " + eventGeofences[k].Event + ": " + eventGeofences[k].Geofence + "\r\n"
                      eventsChange = true
                    }

                    break;
                  }
                }
              }

              break;
            }
          }

          //update last event
          for (var i = 0; i < this.LastEventGeofences.length; i++) {
            if (this.LastEventGeofences[i].Id == id) {
              this.LastEventGeofences[i].Name = name
              this.LastEventGeofences[i].EventGeofences = eventGeofences

              break;
            }
          }
        }

        if (eventsExists && eventsChange) {
          this.$notification.open({
            message: 'Thông báo trạng thái',
            duration: 3,
            description: x => {
              return x(
                'BRx',
                {
                  props: {
                    Content: message,
                  },
                },
              )
            },
          })
        }
      },

      SetLocation(title, id, longitude, latitude, category, status, address) {
        id = id + "_Location_" + this.TabNumber
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
                  'coordinates': [longitude, latitude]
                },
                'properties': {
                  'description': '<strong>Thiết bị: </strong>' + title
                    + '<p><strong>Toạ độ: </strong>' + [longitude, latitude]
                    + '<p><strong>Địa chỉ: </strong>' + address,
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

          while (Math.abs(e.lngLat.lng - longitude) > 180) {
            longitude += e.lngLat.lng > longitude ? 360 : -360;
          }

          new mapboxgl.Popup()
            .setLngLat(coordinates)
            .setHTML(description)
            .addTo(map);
        });
      },

      SetRoute(id, longitude, latitude, locations) {
        id = id + "_Route_" + this.TabNumber
        this.AddedIDs.push({ Id: id, Type: 2 })

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
          if (map.getSource(id + '_route')) {
            map.removeLayer(id + '_direction_layer')
            map.removeLayer(id + '_layer')
            map.removeSource(id + '_route')
          }
        }
        else {
          return
        }

        map.addSource(id + '_route', {
          'type': 'geojson',
          'data': {
            'type': 'Feature',
            'properties': {},
            'geometry': {
              'type': 'LineString',
              'coordinates': locations,
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
      ClearRoute() {
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
        this.LastLocations = []
      },

      SetGeofence(id, name, coordinates) {
        id = id + "_Geofence_" + this.TabNumber
        this.AddedIDs.push({ Id: id, Type: 3 })

        if (map.getSource(id + '_geofence'))
          return

        map.addSource(id + '_geofence', {
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
          'id': id + '_layer',
          'type': 'fill',
          'source': id + '_geofence',
          'layout': {},
          'paint': {
            'fill-color': '#0080ff',
            'fill-opacity': 0.5
          }
        });

        map.addLayer({
          'id': id + '_outline',
          'type': 'line',
          'source': id + '_geofence',
          'layout': {},
          'paint': {
            'line-color': '#000',
            'line-width': 1
          }
        });

        map.addLayer({
          'id': id + '_title',
          'source': id + '_geofence',
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

      async GetLocationsByDevice() {
        this.ClearRoute()
      },

      async GetLocations() {
        var haveData = false

        try {
          var url = "/Api/GpsTracking/Location/GetLocations"

          let response = await this.AjaxPOST(url, this.Filter)
          if (this.HandleResponse(response)) {

            if (response.data.Status == 1) {
              haveData = true

              var Models = response.data.Result.Models
              if (Models.length == 1) {

                if (this.CenterLongitude != Models[0].LastLongitude || this.CenterLatitude != Models[0].LastLatitude) {
                  this.CenterLongitude = Models[0].LastLongitude
                  this.CenterLatitude = Models[0].LastLatitude

                  map.flyTo({
                    center: [
                      this.CenterLongitude,
                      this.CenterLatitude
                    ],
                    essential: true
                  });
                }
              }

              for (var i = 0; i < Models.length; i++) {
                var item = Models[i]

                this.SetLocation(item.Name, item.Id, item.LastLongitude, item.LastLatitude, item.CategoryId, item.StatusId, item.Address);

                if (this.IsLiveRoute)
                  this.SetRoute(item.Id, item.LastLongitude, item.LastLatitude, item.Locations);

                this.EventMessage(item.Id, item.Name, item.EventTypeName);
                this.EventGeofenceMessage(item.Id, item.Name, item.EventGeofences);
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
        }.bind(this), !haveData ? 60000 : 1000);
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

      let params = (new URL(document.location)).searchParams;
      this.TabNumber = lib.ToNumber(params.get("TabNumber"));
      this.Width = lib.ToNumber(params.get("Width"));
      this.Height = lib.ToNumber(params.get("Height"));
    },
    mounted: function () {
      this.SetupMap(1, true);
    }
  }
</script>

<style>
  body {
    padding-top: 0px !important;
  }

  .mapboxgl-popup {
    max-width: 330px !important;
  }
</style>
