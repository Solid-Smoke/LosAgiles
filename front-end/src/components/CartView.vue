<template>
    <MainNavbar />
    <h1 class="display-4 text-center mb-4"><strong>Mi Carrito</strong></h1>
    <div class="table-responsive-sm">
        <table class="table table-striped table-hover">
            <thead>
                <tr>
                    <th scope="col">Producto</th>
                    <th scope="col">Emprendimiento</th>
                    <th scope="col">Cantidad</th>
                    <th scope="col">Precio Unitario</th>
                    <th scope="col">Total por Producto</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="product in cartProducts" :key="product.productID">
                    <td>{{ product.name }}</td>
                    <td>{{ product.businessName }}</td>
                    <td>{{ product.quantity }}</td>
                    <td>₡ {{ product.price }}</td>
                    <td>₡ {{ product.total }}</td>
                </tr>
            </tbody>
        </table>
        <div class="text-right mt-3">
            <h4><strong>Total: ₡ {{ totalPrice.toFixed(2) }}</strong></h4>
        </div>
        <div class="d-flex justify-content-center mt-4">
            <button v-on:click="checkout" class="btn btn-success mx-2">Realizar Compra</button>
            <button v-on:click="confirmClearCart" class="btn btn-danger mx-2">Descartar Todo</button>
            <a href="/" class="btn btn-secondary mx-2">Cerrar Carrito</a>
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
                ],
                userID: 0,
            };
        },
        methods: {
            getUserCart() {
                try {
                    
                    axios.get(`${BackendUrl}/ShoppingCart/${this.userID}`).then(
                        (response) => {
                            this.cartProducts = response.data;
                        }
                    );
                } catch (error) {
                    alert("Error al cargar el carrito");
                    this.closeCart();
                }  
            },
            checkout() {
                alert("Compra realizada con éxito!");
            },
            confirmClearCart() {
                const confirmed = window.confirm("¿Estás seguro de que deseas descartar todo el carrito? (Esta accion no es reversible)");
                if (confirmed) {
                    this.clearCart();
                }
            },
            clearCart() {
                this.cartProducts = [];
                try {

                    axios.delete(`${BackendUrl}/ShoppingCart/${this.userID}`).then(
                        (response) => {
                            console.log(response);
                        }
                    );
                } catch (error) {
                    alert("Error al cargar el carrito");
                    this.closeCart();
                }  
            },
            closeCart() {
                this.$router.push({ name: 'Home' });
            },
            getUserId() {
                const user = JSON.parse(localStorage.getItem('user'));
                return Number(user[0].userID);
            },
        },
        computed: {
            totalPrice() {
                return this.cartProducts.reduce((total, product) => total + product.quantity * product.price, 0);
            },
        },
        mounted() {
            this.userID = this.getUserId();
            this.getUserCart();
        },
    };
</script>

<style scoped>
</style>