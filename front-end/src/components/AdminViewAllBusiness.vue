<template>
    <MainNavbar />
    <h1 class="display-4 text-center mb-4"><strong>Emprendimientos</strong></h1>
    <div class="table-responsive-sm">
        <table class="table table-striped table-hover">
            <thead>
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
                    <td>{{ business.name }}</td>
                    <td>{{ business.idNumber }}</td>
                    <td>
                        <button v-on:click="showContactInfo(business)" class="btn btn-primary">Ver Contacto</button>
                    </td>
                    <td>{{ business.permissions }}</td>
                    <td>
                        <button v-on:click="showLocation(business)" class="btn btn-success">Ver Ubicación</button>
                    </td>
                    <td>
                        <button v-on:click="viewInventory(business)" class="btn btn-info">Inventario</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import axios from "axios";

    export default {
        components: {
            MainNavbar,
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
                currentBusiness: {
                    businessID: 0,
                    name: '',
                    idNumber: '',
                    email: '',
                    telephone: '',
                    permissions: '',
                },
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
                axios.get("https://localhost:7168/api/BusinessDataByEmployeeID", {
                    params: { EmployeeID: this.userID },
                }).then(
                    (response) => {
                        this.businesses = response.data;
                        console.log(this.businesses);
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
                    const response = await axios.get("https://localhost:7168/api/BusinesessAddresessByBusinessID", {
                        params: { BusinessID: business.businessID },
                    });
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
                        name: business.name,
                        idNumber: business.idNumber,
                        email: business.email,
                        telephone: business.telephone,
                        permissions: business.permissions,
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
