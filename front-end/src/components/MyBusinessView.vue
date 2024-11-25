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
                    <th scope="col">Panel</th>
                    <th scope="col"></th>
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
                    <td class="table-cell-button">
                        <button @click="goToBusinessPanel(business.businessID)" class="btn-op-close">Panel</button> 
                    </td>
                    <td class="table-cell-button">
                        <b-button variant="danger" v-on:click="showConfirmDeleteBusinessModal(business)">Eliminar</b-button> 
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <AddProductView ref="addProductModal" />
    <ActionModalConfirm ref="confirmBusinesstModal" />
    <b-modal v-model="showDeleteModal" centered hide-footer>
        <template #title>
            <div class="text-center">
                &#128465; ¡ATENCIÓN!
            </div>
        </template>
        <p class="my-4">
            ¿Seguro que quieres eliminar el emprendimiento {{ businessToDelete.name }}?
        </p>
        <div class="d-flex justify-content-end">
            <b-button variant="danger" @click="deleteBusiness" class="mr-2">Borrar</b-button>
            <b-button variant="secondary" @click="showDeleteModal = false">Cancelar</b-button>
        </div>
    </b-modal>
    <b-modal v-model="showSuccessModal" centered hide-footer>
        <template #title>
            <div class="text-center">
                &#9989; Emprendimiento borrado
            </div>
        </template>
        <p class="my-4">
            &#9989; El borrado fue exitoso.
        </p>
        <div class="d-flex justify-content-end">
            <b-button variant="btn btn-success btn-block" @click="refreshPage">Aceptar</b-button>
        </div>
    </b-modal>
    <b-modal v-model="showErrorModal" centered hide-footer>
        <template #title>
            <div class="text-center">
                &#10060; Error al borrar emprendimiento
            </div>
        </template>
        <p class="my-4" v-if="failedToDeleteProducts.length > 0">
            No se pudo realizar el borrado porque los siguientes productos del emprendimiento están asociados a órdenes activas:
        </p>
        <ul v-if="failedToDeleteProducts.length > 0">
            <li v-for="product in failedToDeleteProducts" :key="product.productID">{{ product.name }}</li>
        </ul>
        <p class="my-4" v-if="failedToDeleteProducts.length > 0">
            Complete o cancele las órdenes activas del emprendimiento a borrar y vuelva a intentarlo.
        </p>
        <p class="my-4" v-else-if="isNetworkError">
            No se pudo conectar con el servidor. Por favor, verifique su conexión a internet o intente de nuevo más tarde.
        </p>
        <p class="my-4" v-else>
            Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.
        </p>
        <div class="d-flex justify-content-end">
            <b-button variant="btn btn-success btn-block" @click="refreshPage">Aceptar</b-button>
        </div>
    </b-modal>
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
            showDeleteModal: false,
            showSuccessModal: false,
            showErrorModal: false,
            businessToDelete: {},
            failedToDeleteProducts: []
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
                             , Correo: ${business.email}\n
                             , Número Telefónico: ${business.telephone}`;
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
                            `, Provincia: ${this.address.province}\n` +
                            `, Cantón: ${this.address.canton}\n` +
                            `, Distrito: ${this.address.district}\n` +    
                            `, Código Postal: ${this.address.postalCode}\n` +
                            `, Otras Señales: ${this.address.otherSigns}`;
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
        goToBusinessPanel(businessID) {
            this.$router.push({ name: 'HomePageEmprendimiento', params: { businessID } });
        },
        showConfirmDeleteBusinessModal(business) {
            this.businessToDelete = business;
            this.showDeleteModal = true;
        },
        async deleteBusiness() {
            try {
                await axios.delete(`${BackendUrl}/Business/${this.businessToDelete.businessID}`);
                this.showDeleteModal = false;
                this.showSuccessModal = true;
            } catch (error) {
                console.error("Error al borrar el negocio: ", error);
                if (error.response) {
                    if (error.response.status === 409) {
                        const failedProductIds = error.response.data.productsIdsFailedToDelete;
                        const response = await axios.get(`${BackendUrl}/Products/Business/${this.businessToDelete.businessID.toString()}`);
                        const allProducts = response.data;
                        this.failedToDeleteProducts = allProducts.filter(product => failedProductIds.includes(product.productID));
                    } else if (error.response.status === 500) {
                        this.failedToDeleteProducts = [];
                    }
                } else if (error.message && error.message.includes('Network Error')) {
                    this.isNetworkError = true;
                }
                this.showDeleteModal = false;
                this.showErrorModal = true;
            }
        },
        refreshPage() {
            window.location.reload();
        },
    },
    created() {
        this.getUserBusiness();
    },
};
</script>
