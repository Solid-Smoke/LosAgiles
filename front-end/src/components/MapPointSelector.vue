<template>
    <div style="align-items: center;">
        <div style="display: flex;
            flex-direction: column; align-items: center;">
            <div id="mapContainer" :style="mapVisibilizationStyle">
                <div id="map"></div>
            </div>
            <b-dropdown id="dropdown-dropleft" dropleft text="Seleccionar una de mis direcciones de entrega"
                variant="primary" class="m-2">
                <b-dropdown-item href="#" @click="selectAddress(address);"  v-for="(address, index) of addresses" :key="index">
                    {{ address.province }},
                    {{ address.canton }},
                    {{ address.district }},
                    <p>{{ address.otherSigns }}</p>
                </b-dropdown-item>
                <b-button @click="this.$refs.addAddressForm.openModal();" variant="success">Agregar dirección</b-button>
            </b-dropdown>
            <div style="display: flex; justify-content: center; margin-top: 0px;">
                <button type="submit" class="btn btn-op-close btn-block" v-if="mapIsShown" @click="emitCoordinatesAndDistance">
                    Confirmar ubicación exacta de entrega
                </button>
            </div>
        </div>
    </div>
    <AddAddressForm parentRoute="/Orden" ref="addAddressForm" :userId="this.userId" />
</template>
<script>
import { LosAgilesMapsApiKey } from '@/main';
import { BackendAPIAddress } from '@/main';
import axios from 'axios';
import AddAddressForm from './AddAddressForm.vue';

const DeliveryStationCoordinates = [9.934257476114691, -84.08158663635609];

function loadGoogleMapsApiKey() {
    (g=>{var h,a,k,p="The Google Maps JavaScript API",c="google",
    l="importLibrary",q="__ib__",m=document,b=window;
    b=b[c]||(b[c]={});
    var d=b.maps||(b.maps={}),r=new Set,
    e=new URLSearchParams,u=()=>h||
    (h=new Promise(async(f,n)=>{await (a=m.createElement("script"));
    e.set("libraries",[...r]+"");
    for(k in g)
        e.set(k.replace(/[A-Z]/g,t=>"_"+t[0].toLowerCase()),g[k]);
    e.set("callback",c+".maps."+q);
    a.src=`https://maps.${c}apis.com/maps/api/js?`+e;
    d[q]=f;
    a.onerror=()=>h=n(Error(p+" could not load."));
    a.nonce=m.querySelector("script[nonce]")?.nonce||"";
    m.head.append(a)}));
    d[l]?console.warn(p+" only loads once. Ignoring:",g):
        d[l]=(f,...n)=>r.add(f)&&u().then(()=>d[l](f,...n))})
    ({key: LosAgilesMapsApiKey, v: "weekly"});
}


    export default {
        components: {
            AddAddressForm
        },
        data() {
            return {
                addresses: [],
                coordinates: [],
                mapIsShown: true,
                mapIsShownStyle: {
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center'
                },
                mapIsHideStyle: {
                    display: 'none'
                },
                mapVisibilizationStyle: this.mapIsShownStyle,
            }
        },
        methods: {
            selectAddress(address) {
                console.log(address.district);
                this.$emit('selectedAddress', address);
            },
            assignCoordinates(coordinatesToAssign) {
                this.coordinates = coordinatesToAssign;
            },
            emitCoordinatesAndDistance() {
                this.$emit('selectedCoordinates', this.coordinates);
                this.$emit('deliveryDistanceKilometers', this.calculateDeliveryDistanceKilometers(this.coordinates, DeliveryStationCoordinates));
            },
            showMap() {
                this.mapVisibilizationStyle = this.mapIsShownStyle;
                this.mapIsShown = true;
            },
            hideMap() {
                this.mapVisibilizationStyle = this.mapIsHideStyle;
                this.mapIsShown = false;
            },
            async initMap() {
                const { Map } = await google.maps.importLibrary("maps");
                const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
                const myLatlng = { lat: 9.934257476114691, lng: -84.08158663635609 };
                const map = new google.maps.Map(document.getElementById("map"), {
                    zoom: 14,
                    center: myLatlng,
                    mapId: "DEMO_MAP_ID",
                });
                const marker = new google.maps.marker.AdvancedMarkerElement({
                    position: myLatlng,
                    map,
                    title: "Click to zoom",
                });
                map.addListener("click", (event) => {
                    this.assignCoordinates([parseFloat(event.latLng.lat().toString()),
                        parseFloat(event.latLng.lng().toString())]);
                    marker.position = {lat: event.latLng.lat(),
                        lng: event.latLng.lng()};
                });
            },
            getUserId() {
                const user = JSON.parse(localStorage.getItem('user'));
                return Number(user[0].userID);
            },
            getUserAddressList() {
                axios
                .get(BackendAPIAddress +
                    "/getAllClientAddresses/details?userId=" +
                    this.userId)
                .then(
                    (response) => {
                        this.addresses = response.data;
                        if(this.addresses.length == 0) {
                            this.$emit('emptyAddressList');
                        } else {
                            this.$emit('noEmptyAddressList');
                        }
                    })
                .catch(function (error) {
                    console.log(error);
                });
            },
            calculateDeliveryDistanceKilometers(deliveryCoordinates, deliveryCentralCoordinates) {
                let kilometer = Math.sqrt(12910961929) / 13000000;
                return Math.sqrt((deliveryCoordinates[0] - deliveryCentralCoordinates[0])**2
                    + (deliveryCoordinates[1] - deliveryCentralCoordinates[1])**2) / kilometer;
            }
        },
        mounted() {
            this.userId = this.getUserId();
            this.getUserAddressList();
            if(this.addresses.length > 0) {
                this.selectAddress(this.addresses[0]);
            }
            loadGoogleMapsApiKey();
            this.initMap();
            this.mapVisibilizationStyle = this.mapIsShownStyle;
        } 
    }
</script>

<style>
    #map {
      width: 500px;
      height: 300px;
      margin-top: 20px;
      border: 1px solid #ccc;
    }
    .dropdown-item-limited {
        max-width: 200px;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
</style>