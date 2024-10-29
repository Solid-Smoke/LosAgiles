<template>
    <AdminNavbar />
    <h1 class="display-4 text-center mb-4"><strong>Emprendimientos Registrados</strong></h1>
    <div class="table-responsive-sm">
        <table class="table-custom table-striped">
            <thead class="table-header">
                <tr>
                    <th scope="col">Nombre</th>
                    <th scope="col">Cédula Asociada</th>
                    <th scope="col">Información de Contacto</th>
                    <th scope="col">Permisos</th>
                    <th scope="col">Ubicación</th>
                    <th scope="col">Ver Inventario</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="business in businesses" :key="business.businessID">
                    <td class="table-cell">{{ business.name }}</td>
                    <td class="table-cell">{{ business.idNumber }}</td>
                    <td class="table-cell-button">
                        <button v-on:click="showContactInfo(business)" class="btn-op-close">Ver Contacto</button>
                    </td>
                    <td class="table-cell">{{ business.permissions }}</td>
                    <td class="table-cell-button">
                        <button v-on:click="showLocation(business)" class="btn-op-close">Ver Ubicación</button>
                    </td>
                    <td class="table-cell-button">
                        <a @click="viewInventory(business)" class="link-blue">Inventario</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import { BackendUrl } from '../main.js';
    import AdminNavbar from './AdminNavbar.vue';
    import axios from "axios";

    export default {
        components: {
            AdminNavbar,
        },
        data() {
            return {
                userID: "3",
                address: [
                    {
                        businessID: 0,
                        province: '',
                        canton: '',
                        district: '',
                        postalCode: '',
                        otherSigns: '',
                    },
                ],
                businesses: [
                    {
                        businessID: 0,
                        name: '',
                        idNumber: '',
                        email: '',
                        telephone: '',
                        permissions: '',
                    },
                ],
            };
        },
        methods: {
            getUserBusiness() {
                axios.get(`${BackendUrl}/Business`).
                    then((response) => {
                        this.businesses = response.data;
                    }
                );
            },
            showContactInfo(business) {
                alert("Contacto de " + business.name + "\n\t" +
                        "Correo: " + business.email + "\n\t" +
                        "Número Telefónico: " +business.telephone);
            },
            async loadLocation(business) {
                try {
                    const response = await axios.get(`${BackendUrl}/Business/${business.businessID}/Addresses`, {});
                    this.address = response.data[0];
                } catch (error) {
                    console.error("Error al cargar la ubicación: ", error);
                }
            },
            async showLocation(business) {
                await this.loadLocation(business);
                console.log(this.address);
                alert("Ubicación de " + business.name + "\n\t" +
                    "Provincia: " + this.address.province + "\n\t" +
                    "Canton: " + this.address.canton + "\n\t" +
                    "Distrito: " + this.address.district + "\n\t" +
                    "Codigo Postal: " + this.address.postalCode + "\n\t" +
                    "Otras Señales: " + this.address.otherSigns);

            },
            viewInventory(business) {
                this.$router.push({
                    name: 'userBusinessInventory',
                    query: {
                        businessID: business.businessID,
                    },
                });
            },
        },
        created() {
            this.getUserBusiness();
        },
    }
</script>

<style></style>
