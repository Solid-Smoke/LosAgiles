<template>
    <MainNavbar />
        <h1 class="display-4 text-center mb-4"><strong>Emprendimientos</strong></h1>
        <b-container class="d-flex justify-content-center">
            <b-row cols="6">
                <b-col md="auto">Nombre</b-col>
                <b-col md="auto">Cédula Asociada</b-col>
                <b-col md="auto">Información de Contacto</b-col>
                <b-col md="auto">Permisos</b-col>
                <b-col md="auto">Ubicación</b-col>
                <b-col md="auto">Ver Inventario</b-col>
            </b-row>

            <b-row v-for="business in businesses" :key="business.BusinessID" class="mb-2">
                <b-col>{{ business.BusinessID }}</b-col>
                <b-col>{{ business.Name }}</b-col>
                <b-col>{{ business.IDNumber }}</b-col>

                <b-col>
                    <b-button @click="showContactInfo(business)" variant="primary">Ver Contacto</b-button>
                </b-col>

                <b-col>{{ business.Permissions }}</b-col>

                <b-col>
                    <b-button @click="showLocation(business)" variant="success">Ver Ubicación</b-button>
                </b-col>

                <b-col>
                    <b-button @click="viewInventory(business)" variant="info">Inventario</b-button>
                </b-col>

            </b-row>
        </b-container>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    export default {
        components: {
            MainNavbar
        },
        data() {
            return {
                userID: 1,
                items: [],
            }
        },

        methods: {
            showContactInfo(business) {
                // Emitimos el prop o acción para mostrar la información de contacto
                alert(`Contacto de ${business.Name}: ${business.Email}, Tel: ${business.Telephone}`);
            },
            showLocation(business) {
                // Emitimos el prop o acción para mostrar la ubicación
                alert(`Ubicación de ${business.Name}: ${business.address.Province}, ${business.address.Canton}, ${business.address.District}`);
            },
            viewInventory(business) {
                // Lógica para ver el inventario del negocio
                alert(`Inventario de ${business.Name}`);
            },
        },
    };
</script>

<style></style>