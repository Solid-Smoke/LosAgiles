<template>
    <MainNavbar />
    <div class="cart-container">
        <h1 class="display-4 text-center mb-4"><strong>Mi Carrito</strong></h1>

        <div class="table-responsive-sm">
            <table class="table table-striped table-hover table-cart">
                <thead>
                    <tr>
                        <th scope="col"> </th>
                        <th scope="col">Producto</th>
                        <th scope="col">Emprendimiento</th>
                        <th scope="col">Cantidad</th>
                        <th scope="col">Precio Unitario</th>
                        <th scope="col">Precio Total</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="product in cartProducts" :key="product.productID">
                        <td>
                            <input type="checkbox" v-model="selectedProducts" :value="{ productID: product.productID, total: product.total }">
                        </td>
                        <td>{{ product.name }}</td>
                        <td>{{ product.businessName }}</td>
                        <td>{{ product.quantity }}</td>
                        <td>₡ {{ product.price }}</td>
                        <td>₡ {{ product.total }}</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="cart-total">
            <h4><strong>Total: ₡ {{ totalPrice.toFixed(2) }}</strong></h4>
        </div>

        <div class="cart-buttons">
            <button @click="checkout" class="btn btn-op1">Comprar Todo</button>
            <button @click="confirmClearCart" class="btn btn-op2">Vaciar Carrito</button>
            <button @click="closeCart" class="btn btn-op-close">Cerrar Carrito</button>
        </div>

        <div v-if="selectedProducts.length > 0" class="selected-total">
            <h4><strong>Total Seleccionado: ₡ {{ selectedTotal.toFixed(2) }}</strong></h4>
        </div>

        <div v-if="selectedProducts.length > 0" class="cart-buttons">
            <button @click="checkoutSelected" class="btn btn-op1">Comprar Seleccionado</button>
            <button @click="clearSelected" class="btn btn-op2">Eliminar Seleccionado</button>
        </div>
    </div>
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
                cartProducts: [
                    {
                        productID: 1,
                        name: 'Producto 1',
                        businessName: 'Emprendimiento A',
                        quantity: 2,
                        price: 1000,
                        total: 2000,
                    },
                    {
                        productID: 2,
                        name: 'Producto 2',
                        businessName: 'Emprendimiento A',
                        quantity: 2,
                        price: 1000,
                        total: 2000,
                    },
                ],
                selectedProducts: [],
                userID: 0,
            };
        },
        methods: {
            getUserCart() {
                axios.get(`${BackendUrl}/ShoppingCart/${this.userID}`).then(
                    (response) => {
                        this.cartProducts = response.data;
                    }
                ).catch(() => {
                    alert("Error al cargar el carrito");
                    this.closeCart();
                });
            },
            confirmClearCart() {
                const confirmed = window.confirm("¿Estás seguro de que deseas vaciar el carrito?");
                if (confirmed) {
                    this.clearCart();
                }
            },
            closeCart() {
                this.$router.push({ name: 'Home' });
            },
            getUserId() {
                const user = JSON.parse(localStorage.getItem('user'));
                return Number(user[0].userID);
            },
            checkout() {
                alert("¡Compra realizada con éxito!");
            },
            checkoutSelected() {
                alert("¡Compra de productos seleccionados realizada con éxito!");
            },
            clearCart() {
                this.cartProducts = [];
                axios.delete(`${BackendUrl}/ShoppingCart/${this.userID}`).then(response => {
                    console.log(response);
                }).catch(() => {
                    alert("Error al vaciar el carrito");
                    this.closeCart();
                });
            },
            clearSelected() {
                this.selectedProducts = [];
            },
        },
        computed: {
            totalPrice() {
                return this.cartProducts.reduce((total, product) => total + product.total, 0);
            },
            selectedTotal() {
                if (this.selectedProducts.length > 0) {
                    return this.selectedProducts.reduce((total, product) => total + product.total, 0);
                }
                return 0;
            },
        },
        mounted() {
            this.userID = this.getUserId();
        },
    };
</script>

<style scoped>
    @import '../styles/GeneralStyle.css';
    @import '../styles/CartStyle.css';
</style>
