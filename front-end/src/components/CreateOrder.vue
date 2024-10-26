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
            </div>
        </b-col>
    </b-row>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import MapPointSelector from './MapPointSelector.vue';
    export default {
        components: {
            MainNavbar,
            MapPointSelector
        },
        data() {    
            return {
                cartProducts: [
                    {
                        productID: 1,
                        productName: 'Producto 1',
                        businessName: 'Emprendimiento A',
                        amount: 2,
                        price: 1000,
                        totalSales: 2000,
                    },
                    {
                        productID: 2,
                        productName: 'Producto 2',
                        businessName: 'Emprendimiento A',
                        amount: 2,
                        price: 1000,
                        totalSales: 2000,
                    },
                ],
                userID: 0,
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
            // this.getUserCart();
        },
    }
    
</script>

<style scoped>
    @import '../styles/GeneralStyle.css';
    @import '../styles/CartStyle.css';
</style>