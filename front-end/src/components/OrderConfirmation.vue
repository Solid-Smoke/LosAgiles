<template>
    <template v-if="isAdmin">
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
                        <td>{{ order.userName }} {{ order.userLastName }}</td>
                        <td>{{ order.createdDate }}</td>
                        <td>{{ order.total }}</td>
                        <td>
                            <button v-on:click="showAddress(order)" class="btn btn-info">Ver dirección</button>
                        </td>
                        <td>
                            <button v-on:click="showProducts(order)" class="btn btn-info">Ver productos</button>
                        </td>
                        <td>
                            <button v-on:click="approveOrder(order)" class="btn btn-success">Aprobar</button> <span />
                            <button v-on:click="rejectOrder(order)" class="btn btn-danger">Rechazar</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <b-modal v-model="ProductsModal" centered scrollable hide-footer title="Productos de la orden">
            <template v-if="selectedProducts.length">
                <ul>
                    <li v-for="product in selectedProducts" :key="product.productID">
                        {{ product.name }} - Cantidad: {{ product.amount }}
                    </li>
                </ul>
            </template>
        </b-modal>

        <b-modal v-model="AddressModal" centered scrollable hide-footer title="Dirección de la orden">
            <template v-if="true">
                <p>Dirección aquí</p>
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
    //import axios from "axios";

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
                        orderID: 1000,
                        userName: 'Maria', //La idea es recibir el nombre, no el id, se encarga un metodo
                        userLastName: 'Rodriguez',
                        createdDate: '23/10/2024', //Se va a usar para tener un trigger 
                        total: 35,
                        state: 'Pendiente',
                        selectedAction: null,
                        products: [ //Se saca del join entre orders > OrderProducts > Products
                            { productID: 1, name: 'Producto A', amount: 2 },
                            { productID: 3, name: 'Producto C', amount: 1 },
                        ],
                        address: '',
                    },
                    {
                        orderID: 1001,
                        userName: 'Jose', //La idea es recibir el nombre, no el id, se encarga un metodo
                        userLastName: 'Arias',
                        createdDate: '23/10/2024', //Se va a usar para tener un trigger 
                        state: 'Pendiente',
                        total: 25,
                        selectedAction: null,
                        products: [
                            { productID: 2, name: 'Producto B', amount: 5 },
                        ],
                        address: '',
                    },
                ],
            };
        },
        methods: {
            showProducts(order) {
                this.selectedProducts = order.products;
                this.ProductsModal = true;
            },
            showAddress(order) {
                this.selectedAddress = order.address;
                this.AddressModal = true;
            },
            //approveOrder(order) {
            //},
            //rejectOrder(order) {
            //},
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
