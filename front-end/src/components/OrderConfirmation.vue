﻿<template>
    <template v-if="isAdmin">
        <AdminNavbar />
        <h1 class="display-4 text-center mb-4"><strong>Revisión de ordenes</strong></h1>
        <h2 class="text-center mb-3"><strong>Órdenes Pendientes</strong></h2>
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
                    <tr v-for="order in pendingOrders" :key="order.orderID">
                        <td>{{ order.orderID }}</td>
                        <td>{{ order.buyer }}</td>
                        <td>{{ formatDate(order.createdDate) }}</td>
                        <td class="text-end pe-5">₡ {{ order.totalAmount }}</td>
                        <td>
                            <button v-on:click="showAddress(order)" class="btn btn-info">Ver dirección</button>
                        </td>
                        <td>
                            <button v-on:click="getProductsByOrderID(order)" class="btn btn-info">
                                Ver productos
                            </button>
                        </td>
                        <td>
                            <button v-on:click="approveOrder(order)" class="btn btn-success">Aprobar</button> <span />
                            <button v-on:click="rejectOrder(order)" class="btn btn-danger">Rechazar</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div> <br />

        <h2 class="text-center mb-3"><strong>Órdenes Aprobadas</strong></h2>
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
                    <tr v-for="order in approvedOrders" :key="order.orderID">
                        <td>{{ order.orderID }}</td>
                        <td>{{ order.buyer }}</td>
                        <td>{{ formatDate(order.createdDate) }}</td>
                        <td class="text-end pe-5">₡ {{ order.totalAmount }}</td>
                        <td>
                            <button v-on:click="showAddress(order)" class="btn btn-info">Ver dirección</button>
                        </td>
                        <td>
                            <button v-on:click="getProductsByOrderID(order)" class="btn btn-info">
                                Ver productos
                            </button>
                        </td>
                        <td>
                            <button v-on:click="rejectOrder(order)" class="btn btn-danger">Cancelar</button>
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
    import AdminNavbar from './AdminNavbar.vue';
    import { BackendUrl } from '../main.js';
    import axios from "axios";

    export default {
        components: {
            AdminNavbar,
        },
        data() {
            return {
                ProductsModal: false,
                AddressModal: false,
                selectedProducts: [],
                selectedAddress: null,
                pendingOrders: [
                    {
                        orderID: 0,
                        buyer: '',
                        createdDate: '',
                        totalAmount: 0,
                        Address: '',
                        products: [
                            { productName: '', amount: 0 }
                        ],
                    },
                ],
                approvedOrders: [
                    {
                        orderID: 0,
                        buyer: '',
                        createdDate: '',
                        totalAmount: 0,
                        Address: '',
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
            getPendingOrders() {
                axios.get(`${BackendUrl}/Order/PendingOrders`)
                .then((response) => {
                    this.pendingOrders = response.data;
                })
                .catch((error) => {
                    console.log(error);
                });
            },
            getApprovedOrders() {
                axios.get(`${BackendUrl}/Order/ApprovedOrders`)
                    .then((response) => {
                        this.approvedOrders = response.data;
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
            approveOrder(order) {
                axios.put(`${BackendUrl}/Order/${order.orderID}/Approval`)
                .then(() => {
                    window.location.reload();
                })
                .catch((error) => {
                    console.log(error);
                });
            },
            rejectOrder(order) {
                axios.put(`${BackendUrl}/Order/${order.orderID}/Rejection`)
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
            this.getPendingOrders();
            this.getApprovedOrders();
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
