<template>
    <MainNavbar />
    <b-row v-if="isClient" class="font-weight-bold text-center mb-2 border-bottom border-2 border-dark">
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
                <MapPointSelector @selected-address="(address) => selectAddress(address)"
                    @delivery-distance-kilometers="(kilometers) => setDeliveryDistanceKilometers(kilometers)"/>
                <strong>Subtotal: ₡{{ totalPrice }}</strong>
                <br>
                <strong>Costo de envío: ₡{{deliveryCost}}</strong>
                <br>
                <strong>IVA: ₡{{IVATaxAmmount}}</strong>
                <br>
                <strong>Total: ₡{{ totalOrderAmmount }}</strong>
                <br>
                <div style="display: flex; justify-content: center; margin-top: 10px;">
                    <button @click="showPaymentModal = true" type="submit" class="btn btn-success btn-block">
                        Comprar
                    </button>
                    <button @click="returnToShoppingCart" type="submit" class="btn btn-op2">
                        Volver al carrito
                    </button>
                </div>
                <b-modal 
                    v-model="showPaymentModal" 
                    centered scrollable hide-footer>
                    <MetodoPago @payment-completed="submitOrder" />
                </b-modal>
            </div>
        </b-col>
    </b-row>
    <h4 style="margin: 10%; text-align: center;" v-else>Solo los clientes registrados pueden crear órdenes</h4>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import MapPointSelector from './MapPointSelector.vue';
    import axios from 'axios';
    import { BackendUrl } from '@/main';
    import MetodoPago from './MetodoPago.vue';

    const IVATax = 0.13;

    export default {
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
        components: {
            MainNavbar,
            MapPointSelector,
            MetodoPago
        },
        data() {    
            return {
                cartProducts: [
                    {
                        productName: 'Manzana',
                        businessName: 'Finca feliz',
                        amount: 2,
                        weight: 0.5,
                        price: 1000,
                        totalSales: 2000,
                    },
                    {
                        productName: 'Adaptador sata',
                        businessName: 'Tech solutions',
                        amount: 2,
                        weight: 0.5,
                        price: 1000,
                        totalSales: 2000,
                    },
                ],
                userID: 0,
                orderAddressSelected: {},
                deliveryDistanceKilometers: 0,
                showPaymentModal: false
            }
        },
        methods: {
            setDeliveryDistanceKilometers(distance) {
                this.deliveryDistanceKilometers = distance;
            },
            selectAddress(address) {
                this.orderAddressSelected = address;
            },
            returnToShoppingCart() {
                this.$router.push({ name: 'cartView.vue' });
            },
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
            submitOrder() {
                let orderProducts = [];
                for (let i = 0; i < this.cartProducts.length; i++) {
                    orderProducts.push(
                        {
                            productID: this.cartProducts[i].productID,
                            businessID: this.cartProducts[i].businessID,
                            ammount: this.cartProducts[i].amount
                        }
                    );
                }
                axios
                    .post(BackendUrl + "/Order", {
                        clientID: this.userID,
                        deliveryAddress: this.orderAddressSelected,
                        products: orderProducts
                    })
                    .then()
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        },
        computed: {
            totalOrderAmmount() {
                return this.totalPrice + this.IVATaxAmmount + this.deliveryCost;
            },
            IVATaxAmmount() {
                return parseFloat(((this.totalPrice + this.deliveryCost) * IVATax).toFixed(2));
            },
            totalOrderWeight() {
                let totalWeight = 0;
                for(let i = 0; i < this.cartProducts.length; i++) {
                    totalWeight += this.cartProducts[i].weight; 
                }
                return totalWeight;
            },
            deliveryCost() {
                const MetropolitanRadioInKilometers = 70;
                const MetropolitanFirstKilogramDeliveryCost = 1700;
                const NonMetropolitanFirstKilogramDeliveryCost = 2350;
                const AdditionalKilogramCost = 1000;
                let additionalKilogramAmmount = 0;
                if(this.totalOrderWeight >= 2) {
                    additionalKilogramAmmount = (Math.trunc(this.totalOrderWeight) - 1) * AdditionalKilogramCost;
                }
                if (this.deliveryDistanceKilometers < MetropolitanRadioInKilometers) {
                    return MetropolitanFirstKilogramDeliveryCost + additionalKilogramAmmount;
                } else {
                    return NonMetropolitanFirstKilogramDeliveryCost + additionalKilogramAmmount;
                }
            },
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