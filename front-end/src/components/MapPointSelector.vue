<template>
    <div style="align-items: center;">
        <div style="display: flex; flex-direction: column; align-items: center;">
            <div id="mapContainer" :style="mapVisibilizationStyle">
                <div id="map"></div>
            </div>
            <div style="display: flex; justify-content: center; margin-top: 10px;">
                <button type="submit" class="btn btn-success btn-block" v-if="mapIsShown" @click="submitDeliveryCoordinates">
                    Confirmar ubicación de entrega
                </button>
                <button v-if="!mapIsShown" @click="showMap" class="btn btn-success btn-block">
                    Elegir ubicación de entrega
                </button>
            </div>
        </div>
    </div>
</template>

<script>
    import { LosAgilesMapsApiKey } from '@/main';

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
        data() {
            return {
                coordinates: [],
                mapIsShown: false,
                mapIsShownStyle: {
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center'
                },
                mapIsHideStyle: {
                    display: 'none'
                },
                mapVisibilizationStyle: this.mapIsHideStyle,
            }
        },
        methods: {
            assignCoordinates(coordinatesToAssign) {
                this.coordinates = coordinatesToAssign;
            },
            submitDeliveryCoordinates() {
                this.$emit('selectedCoordinates', this.coordinates);
                this.$emit('deliveryCost', this.calculateDeliveryCost(this.coordinates, DeliveryStationCoordinates));
                this.hideMap();
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
                const myLatlng = { lat: DeliveryStationCoordinates[0], lng: DeliveryStationCoordinates[1] };
                const map = new google.maps.Map(document.getElementById("map"), {
                    zoom: 4,
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
            calculateDeliveryCost(deliveryCoordinates, deliveryCentralCoordinates) {
                return Math.sqrt((deliveryCoordinates[0] - deliveryCentralCoordinates[0])**2
                    + (deliveryCoordinates[1] - deliveryCentralCoordinates[1])**2);
            }
        },
        mounted() {
            loadGoogleMapsApiKey();
            this.initMap();
            this.mapVisibilizationStyle = this.mapIsHideStyle;
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
</style>