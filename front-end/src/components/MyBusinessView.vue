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
    import axios from "axios";
    export default {
        components: {
            MainNavbar
        },
        data() {
            return {
                userID: "3",
                businesses: [
                    {
                        UserID: 1,
                        BusinessID: 101,
                        Name: 'Business Name 1',
                        IDNumber: '123456789',
                        Email: 'email@example.com',
                        Telephone: '123-456-7890',
                        Permissions: 'Admin, Manager',
                        Province: 'San José',
                        Canton: 'Central',
                        District: 'Carmen',
                        PostalCode: '10101',
                        OtherSigns: 'Some landmark details'
                    },
                ],
            }
        },

        methods: {
            getUserBusiness() {
                axios.get("https://localhost:7168/api/BusinessDataByEmployeeID", {
                    params: { EmployeeID: this.userID, },
                }).then(
                    (response) => {
                        this.businesses = response.data;
                    });
            },
            showContactInfo(business) {
                // Emitimos el prop o acción para mostrar la información de contacto
                alert(`Contacto de ${business.Name}: ${business.Email}, Tel: ${business.Telephone}`);
            },
            showLocation(business) {
                // Emitimos el prop o acción para mostrar la ubicación
                alert(`Ubicación de ${business.Name}: ${business.Province}, ${business.Canton}, ${business.District}`);
            },
            viewInventory(business) {
                // Lógica para ver el inventario del negocio
                alert(`Inventario de ${business.Name}`);
            },
        },

        created: function () {
            this.getUserBusiness();
        }
    };
</script>

<style></style>