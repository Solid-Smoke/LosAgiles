<template>
    <template v-if="isClient">
        <MainNavbar />
        <h1 class="display-4 text-center mb-4"><strong>Mis ordenes</strong></h1>
        <template v-if="hasOrders">
            <div class="table-responsive-sm">
                <table class="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th scope="col"># de orden</th>
                            <th scope="col">Fecha de creación</th>
                            <th scope="col">Fecha de entrega</th>
                            <th scope="col">Monto total</th>
                            <th scope="col">Estado de la orden</th>
                            <th scope="col">Dirección de entrega</th>
                            <th scope="col">Productos</th>
                            <th scope="col">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="order in orders" :key="order.orderID">
                            <td>{{ order.orderID }}</td>
                            <td>{{ formatDate(order.createdDate) }}</td>
                            <td>{{ order.deliveryDate ? formatDate(order.deliveryDate) : '-' }}</td>
                            <td class="text-end pe-5">₡ {{ order.totalAmount }}</td>
                            <td>{{ order.status }}</td>
                            <td>
                                <button v-on:click="showAddress(order)" class="btn btn-info">Ver dirección</button>
                            </td>
                            <td>
                                <button v-on:click="getProductsByOrderID(order)" class="btn btn-info">
                                    Ver productos
                                </button>
                            </td>
                            <td>
                                <button v-on:click="openWarningCancelOrderModal(order)" class="btn btn-danger" :disabled="order.status !== 'Pendiente'">
                                    Cancelar orden
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <ActionModalWarning ref="warningCancelOrderModal" @confirmed="cancelOrder" />
        </template>
        <template v-else>
            <h3 class="text-center mb-4">Actualmente no tiene ordenes registradas</h3>
        </template>

        <b-modal v-model="ProductsModal" centered scrollable hide-footer title="Productos de la orden">
            <template v-if="selectedProducts && selectedProducts.length">
                <ul>
                    <li v-for="product in selectedProducts" :key="product.name">
                        {{ product.productName }} - Cantidad: {{ product.amount }}
                    </li>
                </ul>
            </template>
        </b-modal>

        <b-modal v-model="AddressModal" centered scrollable hide-footer title="Dirección de la orden">
            <template v-if="true">
                <p>{{ selectedAddress }}</p>
            </template>
        </b-modal>
    </template>
    <template v-else>
        <h1 class="display-4 text-center mb-4"><strong>403 Forbidden</strong></h1>
        <h3 class="text-center mb-4">No es un usuario registrado <br /><a href="/">Regresar</a></h3>
    </template>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import ActionModalWarning from './ActionModalWarning.vue';
    import { BackendUrl } from '../main.js';
    import axios from "axios";

    export default {
        components: {
            MainNavbar,
            ActionModalWarning,
        },
        data() {
            return {
                userID: 0,
                ProductsModal: false,
                AddressModal: false,
                selectedProducts: [],
                selectedAddress: null,
                hasOrders: false,
                orderIDToDelete: 0,
                orders: [
                    {
                        orderID: 0,
                        createdDate: '',
                        deliveryDate: '',
                        totalAmount: 0,
                        Address: '',
                        status: '',
                        products: [
                            { productName: '', amount: 0 }
                        ],
                    },
                ],
            };
        },
        methods: {
            getUserId() {
                const user = JSON.parse(localStorage.getItem('user'));
                return Number(user[0].userID);
            },
            showAddress(order) {
                this.selectedAddress = order.address;
                this.AddressModal = true;
            },
            getUserOrders(userID) {
                axios.get(`${BackendUrl}/Order/OrdersByClientID/${userID}`)
                .then((response) => {
                    this.orders = response.data;
                    if (this.orders[0]) {
                        this.hasOrders = true;
                    }
                })
                .catch((error) => {
                    console.log(error);
                });
            },
            getProductsByOrderID(order) {
                axios.get(`${BackendUrl}/Order/ProductsByOrderID/${order.orderID}`)
                .then((response) => {
                    order.products = response.data;
                    this.selectedProducts = order.products;
                    this.ProductsModal = true;
                })
                .catch((error) => {
                    console.log(error);
                });
            },
            openWarningCancelOrderModal(order) {
                this.orderIDToDelete = order.orderID;
                this.$refs.warningCancelOrderModal.openModal("Está seguro de que desea cancelar la orden? (Esta accion es irreversible)");
            },
            cancelOrder() {
                axios.put(`${BackendUrl}/Order/${this.orderIDToDelete}/Rejection`)
                    .then(() => {
                        window.location.reload();
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            },
            formatDate(dateString) {
                const date = new Date(dateString);
                const year = date.getFullYear();
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const day = String(date.getDate()).padStart(2, '0');
                return `${day}/${month}/${year}`;
            }
        },
        mounted() {
            this.userID = this.getUserId();
            this.getUserOrders(this.userID);
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
    }
</script>

<style></style>
