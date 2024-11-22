<template>
    <template v-if="isClient">
        <MainNavbar />
    </template>
    <template v-if="isAdmin">
        <AdminNavbar />
    </template>
    <h1 class="display-4 text-center mb-4"><strong>Inventario</strong></h1>
    <div class="table-responsive-sm" style="margin-bottom: 100px;">
        <table class="table-custom table-striped">
            <thead class="table-header">
                <tr>
                    <th scope="col">
                        Seleccionar todos <input type="checkbox" v-model="selectAll" @change="toggleSelectAll(); handleProductSelection();">
                    </th>
                    <th scope="col">ID del Producto</th>
                    <th scope="col">Nombre</th>
                    <th scope="col">Descripci�n</th>
                    <th scope="col">Precio</th>
                    <th scope="col">Stock</th>
                    <th scope="col">Perecedero</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="product in products" :key="product.productID">
                    <td class="table-cell center-checkbox"><input type="checkbox" v-model="selectedProducts" :value="product.productID"
                        @change="handleProductSelection"></td>
                    <td class="table-cell">{{ product.productID }}</td>
                    <td class="table-cell">{{ product.name }}</td>
                    <td class="table-cell">{{ product.description }}</td>
                    <td class="table-cell">{{ product.price }}</td>
                    <td class="table-cell">{{ product.stock }}</td>
                    <td class="table-cell">{{ product.isPerishable ? 'S�' : 'No' }}</td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="inventory-buttons fixed-bottom">
        <p v-if="showWarning" class="text-danger">Seleccione los productos que desea eliminar</p>
        <button class="btn btn-op2" @click="handleDeleteClick">
            Eliminar productos seleccionados
        </button>
    </div>
    <b-modal v-model="showDeleteModal" centered hide-footer>
        <template #title>
            <div class="text-center">
                &#128465; ¡ATENCIÓN!
            </div>
        </template>
        <p class="my-4">
            ¿Seguro que quieres eliminar los siguientes productos?
        </p>
        <ul>
            <li v-for="product in products.filter(p => selectedProducts.includes(p.productID))" :key="product.productID">{{product.name}}</li>
        </ul>
        
        <div class="d-flex justify-content-end">
            <b-button variant="danger" @click="deleteSelectedProducts" class="mr-2">Borrar</b-button>
            <b-button variant="secondary" @click="showDeleteModal = false">Cancelar</b-button>
        </div>
    </b-modal>
    <b-modal v-model="showSuccessModal" centered hide-footer>
        <template #title>
            <div class="text-center">
                &#9989; Producto borrado
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
                &#10060; Error al borrar productos
            </div>
        </template>
        <p class="my-4" v-if="failedToDeleteProducts.length > 0">
            No se pudo realizar el borrado porque los siguientes productos están asociados a órdenes activas:
        </p>
        <ul v-if="failedToDeleteProducts.length > 0">
            <li v-for="product in failedToDeleteProducts" :key="product.productID">{{product.name}}</li>
        </ul>
        <p class="my-4" v-if="failedToDeleteProducts.length > 0">
            Intente de nuevo sin seleccionar los productos que están asociados a órdenes activas
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
import AdminNavbar from './AdminNavbar.vue';
import { BackendUrl } from '../main.js';
import axios from "axios";

export default {
    components: {
        MainNavbar,
        AdminNavbar
    },
    data() {
        return {
            showDeleteModal: false,
            showSuccessModal: false,
            showErrorModal: false,
            currentBusinessId: "",
            products: [],
            selectAll: false,
            selectedProducts: [],
            showWarning: false,
            failedToDeleteProducts: [],
            isNetworkError: false,
        };
    },
    methods: {
        getBusinessInventory() {
            axios.get(`${BackendUrl}/Products/Business/${this.currentBusinessId}`, {
            }).then(
                (response) => {
                    this.products = response.data;
                }
            );
        },
        toggleSelectAll() {
            this.selectedProducts = this.selectAll ? this.products.map(product => product.productID) : [];
        },
        deleteSelectedProducts() {
            this.showDeleteModal = false;
            axios.delete(`${BackendUrl}/Products/BatchDelete`, {data: this.selectedProducts })
            .then(() => {
                this.showSuccessModal = true;
                this.selectedProducts = []
            })
            .catch((error) => {
                console.log(error);
                if (error.response) {
                    if (error.response.status === 409) {
                        this.failedToDeleteProducts = this.products
                            .filter(product => error.response.data.productsIdsFailedToDelete.includes(product.productID))
                            .sort((a, b) => a.productID - b.productID);
                    } else if (error.response.status === 500) {
                        this.failedToDeleteProducts = [];
                    }
                } else if (error.message && error.message.includes('Network Error')) {
                    this.isNetworkError = true;
                }
                this.showErrorModal = true;
            });
        },
        refreshPage() {
            window.location.reload();
        },
        handleDeleteClick() {
            if (this.selectedProducts.length === 0) {
                this.showWarning = true;
            } else {
                this.showWarning = false;
                this.showDeleteModal = true;
            }
        },
        handleProductSelection() {
            if (this.selectedProducts.length > 0) {
                this.showWarning = false;
            }
        },
    },
    created() {
        this.currentBusinessId = this.$route.query.businessID;
        this.getBusinessInventory();
    },
    props: {
        isAdmin: {
            type: Boolean,
            required: true,
        },
        isClient: {
            type: Boolean,
            required: true,
        },
    },
};
</script>

<style scoped>
@import '../styles/GeneralStyle.css';

.center-checkbox {
    text-align: center;
}
.center-checkbox input {
    margin: 0 auto;
}
.inventory-buttons {
    position: fixed;
    bottom: 0;
    width: 100%;
    text-align: center;
    background-color: white;
    padding: 1rem;
    box-shadow: 0 -2px 5px rgba(0, 0, 0, 0.1);
    z-index: 1000;
}

.inventory-buttons .btn {
    margin: 0 10px;
    padding: 10px 20px;
	border-radius: 20px;
	font-weight: bold;
}

.table-responsive-sm {
    margin-bottom: 100px;
}
</style>
