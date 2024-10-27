<template>
    <MainNavbar />
    <b-row class="font-weight-bold text-center mb-2 border-bottom border-2 border-dark">
        <b-col>   
            <div class="cart-container">
                <h2 class="display-4 text-center mb-4"><strong>Productos a ordenar</strong></h2>
                <div class="table-responsive-sm">
                    <table class="table table-striped table-hover table-cart">
                        <thead>
                            <tr>
                                <th scope="col">Producto</th>
                                <th scope="col">Emprendimiento</th>
                                <th scope="col">Cantidad</th>
                                <th scope="col">Peso</th>
                                <th scope="col">Precio Unitario</th>
                                <th scope="col">Precio Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="product in cartProducts"
                                :key="product.productID">
                                <td>{{ product.productName }}</td>
                                <td>{{ product.businessName }}</td>
                                <td>{{ product.amount }}</td>
                                <td>{{ product.weight }} Kg</td>
                                <td>₡ {{ product.price }}</td>
                                <td>₡ {{ product.totalSales }}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </b-col>
        <b-col>
            <div class="cart-container">
                <h2 class="display-4 text-center mb-4"><strong>Detalles orden</strong></h2>
                <MapPointSelector />
                <strong>Subtotal: ₡4000</strong>
                <br>
                <strong>Costo de envío: ₡2350</strong>
                <br>
                <strong>Total: ₡</strong>
                <br>
                <div @click="showPaymentModal = true" style="display: flex; justify-content: center; margin-top: 10px;">
                    <button type="submit" class="btn btn-success btn-block">
                        Comprar
                    </button>
                    <button type="submit" class="btn btn-op2">
                        Cancelar
                    </button>
                </div>
                <b-modal 
                    v-model="showPaymentModal" 
                    centered scrollable hide-footer>
                    <MetodoPago />
                </b-modal>
            </div>
        </b-col>
    </b-row>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import MapPointSelector from './MapPointSelector.vue';
    import axios from 'axios';
    import { BackendUrl } from '@/main';
    import MetodoPago from './MetodoPago.vue';
    export default {
        components: {
            MainNavbar,
            MapPointSelector,
            MetodoPago
        },
        data() {    
            return {
                cartProducts: [
                    {
                        productID: 1,
                        productName: 'Manzana',
                        businessName: 'Finca feliz',
                        amount: 2,
                        weight: 0.5,
                        price: 1000,
                        totalSales: 2000,
                    },
                    {
                        productID: 2,
                        productName: 'Adaptador sata',
                        businessName: 'Tech solutions',
                        amount: 2,
                        weight: 0.5,
                        price: 1000,
                        totalSales: 2000,
                    },
                ],
                userID: 0,
                showPaymentModal: false
            }
        },
        methods: {
            getUserCart() {
                axios.get(`${BackendUrl}/ShoppingCart/${this.userID}`)
                .then((response) => {
                    this.cartProducts = response.data;
                    console.log(this.cartProducts);
                })
                .catch((error) => {
                    this.$refs.errorCleanCartModal.openModal("Error al cargar el carrito", error);
                });
            },
            getUserId() {
                const user = JSON.parse(localStorage.getItem('user'));
                return Number(user[0].userID);
            },
        },
        computed: {
            totalPrice() {
                return this.cartProducts.reduce((total, product) => total + product.totalSales, 0);
            },
            selectedTotal() {
                if (this.selectedProducts.length > 0) {
                    return this.selectedProducts.reduce((total, product) => total + product.total, 0);
                }
                return 0;
            },
            areAllSelected() {
                return this.selectedProducts.length === this.cartProducts.length;
            },
        },
        mounted() {
            this.userID = this.getUserId();
            this.getUserCart();
        },
    }
    
</script>

<style scoped>
    @import '../styles/GeneralStyle.css';
    @import '../styles/CartStyle.css';
</style>