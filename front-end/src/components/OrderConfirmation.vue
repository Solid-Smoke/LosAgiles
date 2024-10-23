<template>
    <template v-if="adminName">
        <template v-if="adminEmail"> <!-- Asegurarse de cambiar esto, esta asi solo para trabajarlo -->
            <h1 class="display-4 text-center mb-4"><strong>403 Forbidden</strong></h1>
            <h3 class="text-center mb-4">Página solo administradores <br /><a href="/">Regresar</a></h3>
        </template>
        <template v-else>
            <MainNavbar />
            <h1 class="display-4 text-center mb-4"><strong>Revisión de ordenes</strong></h1>
            <div class="table-responsive-sm">
                <table class="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th scope="col"># de orden</th>
                            <th scope="col">Comprador</th>
                            <th scope="col">Fecha de creación</th>
                            <th scope="col">Productos</th>
                            <th scope="col">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="order in orders" :key="order.orderID">
                            <td>{{ order.orderID }}</td>
                            <td>{{ order.associatedUser }}</td>
                            <td>{{ order.createdDate }}</td>
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

            <!-- Modal for displaying products -->
            <b-modal v-model="ProductsModal" centered scrollable hide-footer title="Productos de la orden">
                <template v-if="selectedProducts.length">
                    <ul>
                        <li v-for="product in selectedProducts" :key="product.productID">
                            {{ product.name }} - Cantidad: {{ product.amount }} - Precio: {{ product.price }}
                        </li>
                    </ul>
                </template>
            </b-modal>
        </template>
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
                selectedProducts: [],
                orders: [
                    {
                        orderID: 1000,
                        associatedUser: '@maria123', //La idea es recibir el nombre, no el id, se encarga un metodo
                        createdDate: '23/10/2024', //Se va a usar para tener un trigger 
                        state: 'Pendiente',
                        products: [ //Se saca del join entre orders > OrderProducts > Products
                            { productID: 1, name: 'Producto A', amount: 2, price: 10 },
                            { productID: 3, name: 'Producto C', amount: 1, price: 15 },
                        ],
                    },
                    {
                        orderID: 1001,
                        associatedUser: '@jose124', //La idea es recibir el nombre, no el id, se encarga un metodo
                        createdDate: '23/10/2024', //Se va a usar para tener un trigger 
                        state: 'Pendiente',
                        products: [
                            { productID: 2, name: 'Producto B', amount: 5, price: 5 },
                        ],
                    },
                ],
                adminName: "",
                adminEmail: "",
            };
        },
        methods: {
            getAdminData() {
                const user = JSON.parse(localStorage.getItem('user'));
                if (user && user.length > 0) {
                    this.adminName = user[0].userName;
                    this.adminEmail = user[0].email;
                }
            },
            showProducts(order) {
                this.selectedProducts = order.products;
                this.ProductsModal = true;
            },
            //approveOrder(order) {
            //    // Your approval logic here
            //},
            //rejectOrder(order) {
            //    // Your rejection logic here
            //},
        },
        created() {
            this.getAdminData();
        },
    }
</script>

<style></style>
