<template>
    <template v-if="!isAdmin">
        <MainNavbar />
        <h1 class="display-4 text-center mb-4"><strong>Revisión de ordenes</strong></h1>
        <div class="table-responsive-sm">
            <table class="table table-striped table-hover">
                <thead>
                    <tr>
                        <th scope="col"># de orden</th>
                        <th scope="col">Comprador</th>
                        <th scope="col">Fecha de creación</th>
                        <th scope="col">Monto total</th>
                        <th scope="col">Dirección de entrega</th>
                        <th scope="col">Productos</th>
                        <th scope="col">Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="order in orders" :key="order.orderID">
                        <td>{{ order.orderID }}</td>
                        <td>{{ order.buyer }}</td>
                        <td>{{ formatDate(order.createdDate) }}</td>
                        <td>{{ order.totalAmount }}</td>
                        <td>
                            <button v-on:click="showAddress(order)" class="btn btn-info">Ver dirección</button>
                        </td>
                        <td>
                            <button v-on:click="GetProductsByOrderID(order)" class="btn btn-info">
                                Ver productos
                            </button>
                        </td>
                        <td>
                            <button v-on:click="ApproveOrder(order)" class="btn btn-success">Aprobar</button> <span />
                            <button v-on:click="RejectOrder(order)" class="btn btn-danger">Rechazar</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

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
        <h3 class="text-center mb-4">Página solo administradores <br /><a href="/">Regresar</a></h3>
    </template>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import { BackendUrl } from '../main.js';
    import axios from "axios";

    export default {
        components: {
            MainNavbar,
        },
        data() {
            return {
                ProductsModal: false,
                AddressModal: false,
                selectedProducts: [],
                selectedAddress: null,
                orders: [
                    {
                        orderID: 0,
                        buyer: '',
                        createdDate: '',
                        totalAmount: 0,
                        Address: '',
                        state: '',
                        selectedAction: null,
                        products: [
                            { productName: '', amount: 0 }
                        ],
                    },
                ],
            };
        },
        methods: {
            showAddress(order) {
                this.selectedAddress = order.address;
                this.AddressModal = true;
            },
            GetPendingOrders() {
                axios.get(`${BackendUrl}/Order/GetPendingOrders`)
                .then((response) => {
                    this.orders = response.data;
                    //console.log(this.orders);
                })
                .catch((error) => {
                    console.log(error);
                });
            },
            GetProductsByOrderID(order) {
                axios.get(`${BackendUrl}/Order/GetProductsByOrderID/${order.orderID}`)
                .then((response) => {
                    order.products = response.data;
                    this.selectedProducts = order.products;
                    //console.log(order.products);
                    this.ProductsModal = true;
                })
                .catch((error) => {
                    console.log(error);
                });
            },
            ApproveOrder(order) {
                axios.put(`${BackendUrl}/Order/ApproveOrder/${order.orderID}`)
                .then(() => {
                    //console.log('Orden exitosamente aprobada');
                    window.location.reload();
                })
                .catch((error) => {
                    console.log(error);
                });
            },
            RejectOrder(order) {
                axios.put(`${BackendUrl}/Order/RejectOrder/${order.orderID}`)
                    .then(() => {
                        //console.log('Orden exitosamente rechazada');
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
            this.GetPendingOrders();
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
