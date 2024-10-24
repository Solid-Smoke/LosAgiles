<template>
    <b-modal 
             v-model="showModal" 
             centered scrollable hide-footer
             title="Registrar Empresa">
            <h3 class="text-center">Agregar dirección</h3>
            <form @submit.prevent="emitCoordinates">
                <div id="mapContainer" style="display: flex;
                    flex-direction: column; align-items: center;">
                    <div id="map"></div>
                </div>
                <div>
                    <button type="submit" class="btn btn-success btn-block">
                        Agregar
                    </button>
                </div>
            </form>
    </b-modal>
</template>
<script>
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
    ({key: "mykeyjose", v: "weekly"});
    console.log("api key cargada");
}


    export default {
        data() {
            return {
                showModal: false,
                coordinates: [],
                mapData: [99.999, 88.888]
            }
        },
        methods: {
            openModal() {
                this.showModal = true;
            },
            closeModal() {
                this.showModal = false;
            },
            assignCoordinates(coordinatesToAssign) {
                this.coordinates = coordinatesToAssign;
            },
            emitCoordinates() {
                this.$emit('selectedCoordinates', this.coordinates);
                this.closeModal();
            },
            async initMap() {
                const { Map } = await google.maps.importLibrary("maps");
                const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
                const myLatlng = { lat: -25.363, lng: 131.044 };
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
            }
        },
        mounted() {
            loadGoogleMapsApiKey();
            this.initMap();
        } 
    }
</script>

<style>
    #map {
      /* height: 66%; */
      width: 500px;
      height: 300px;
      margin-top: 20px;
      border: 1px solid #ccc;
    }
</style>