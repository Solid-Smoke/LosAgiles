<template>
    <MainNavbar />
    <h1 class="display-4 text-center mb-4"><strong>Emprendimientos</strong></h1>
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
                    <th scope="col">Agregar Producto</th>
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
                    <td class="table-cell-button">
                        <a @click="openProductModal(business.businessID)" class="link-blue">Agregar Producto</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <AddProductView ref="addProductModal" />
    <ActionModalConfirm ref="confirmBusinesstModal" />
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import AddProductView from './AddProductView.vue';
    import ActionModalConfirm from './ActionModalConfirm.vue';
    import { BackendUrl } from '../main.js';
    import axios from "axios";

    export default {
        components: {
            MainNavbar,
            AddProductView,
            ActionModalConfirm,
        },
        data() {
            return {
                userID: "1",
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
                const user = JSON.parse(localStorage.getItem('user'));
                const id = Number(user[0].userID);
                axios.get(`${BackendUrl}/Business/Employee/${id}`).then(
                    (response) => {
                        this.businesses = response.data;
                    }
                );
            },
            showContactInfo(business) {
                const message = `Contacto de ${business.name}\n
                                 Correo: ${business.email}\n
                                 Número Telefónico: ${business.telephone}`;
                this.$refs.confirmBusinesstModal.openModal(message);
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
                const message = `Ubicación de ${business.name}\n` +
                                `Provincia: ${this.address.province}\n` +
                                `Cantón: ${this.address.canton}\n` +
                                `Distrito: ${this.address.district}\n` +    
                                `Código Postal: ${this.address.postalCode}\n` +
                                `Otras Señales: ${this.address.otherSigns}`;

                this.$refs.confirmBusinesstModal.openModal(message);
                
            },
            viewInventory(business) {
                this.$router.push({
                    name: 'userBusinessInventory',
                    query: {
                        businessID: business.businessID,
                    },
                });
            },
            openProductModal(businessID) { 
                this.$refs.addProductModal.openModal(businessID);
            },
        },
        created() {
            this.getUserBusiness();
        },
    }
</script>

<style></style>
