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
                        Seleccionar todos <input type="checkbox" v-model="selectAll" @change="toggleSelectAll">
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
                    <td class="table-cell center-checkbox"><input type="checkbox" v-model="selectedProducts" :value="product.productID"></td>
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
    <div class="cart-buttons fixed-bottom">
        <button class="btn btn-op2" @click="showDeleteModal = true">
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
            <ul>
                <li v-for="product in products" :key="product.productID">-{{product.name}}</li>
            </ul>
        </p>
        <div class="d-flex justify-content-end">
            <b-button variant="danger" @click="deleteSelectedProducts" class="mr-2">Borrar</b-button>
            <b-button variant="secondary" @click="showDeleteModal = false">Cancelar</b-button>
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
            currentBusinessId: "",
            products: [
                {
                    productID: 1,
                    name: "Product 1",
                    description: "Description for Product 1",
                    category: "Category A",
                    price: 100,
                    stock: 20,
                    weight: 1.5,
                    isPerishable: true,
                    dailyAmount: 5,
                    daysAvailable: "Mon-Sun",
                    businessID: null,
                },
                {
                    productID: 2,
                    name: "Product 2",
                    description: "Description for Product 2",
                    category: "Category B",
                    price: 200,
                    stock: 10,
                    weight: 2.0,
                    isPerishable: false,
                    dailyAmount: null,
                    daysAvailable: "Tue-Sat",
                    businessID: null,
                },
            ],
            selectAll: false,
            selectedProducts: []
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
            axios.delete(`${BackendUrl}/Products`, {data: this.selectedProducts})
                .then(() => {
                    this.selectedProducts = []
                })
                .catch((error) => {
                    console.log(error);
                });
        }
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
@import '../styles/CartStyle.css';

.center-checkbox {
    text-align: center;
}
.center-checkbox input {
    margin: 0 auto;
}
.cart-buttons {
    position: fixed;
    bottom: 0;
    width: 100%;
    text-align: center;
    background-color: white;
    padding: 1rem;
    box-shadow: 0 -2px 5px rgba(0, 0, 0, 0.1);
    z-index: 1000;
}
.table-responsive-sm {
    margin-bottom: 100px;
}
</style>
