<template>
    <MainNavbar />
    <div class="cart-container">
        <h1 class="display-4 text-center mb-4"><strong>Mi Carrito</strong></h1>
        <div class="table-responsive-sm">
            <table class="table table-striped table-hover table-cart">
                <thead>
                    <tr>
                        <th scope="col">
                            <input type="checkbox"
                                   @change="toggleSelectAll($event)"
                                   :checked="areAllSelected">
                        </th>
                        <th scope="col">Producto</th>
                        <th scope="col">Emprendimiento</th>
                        <th scope="col">Cantidad</th>
                        <th scope="col">Precio Unitario</th>
                        <th scope="col">Precio Total</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="product in cartProducts"
                        :key="product.productID"
                        @click="toggleSelect(product)">
                        <td>
                            <input type="checkbox" 
                                   v-model="selectedProducts" 
                                   :value="{ productID: product.productID, 
                                             quantity: product.amount,
                                             total: product.totalSales }">
                        </td>
                        <td>{{ product.productName }}</td>
                        <td>{{ product.businessName }}</td>
                        <td>{{ product.amount }}</td>
                        <td>₡ {{ product.price }}</td>
                        <td>₡ {{ product.totalSales }}</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div v-if="selectedProducts.length === 0" class="cart-total">
            <h4><strong>Total: ₡ {{ totalPrice.toFixed(2) }}</strong></h4>
        </div>

        <div v-if="selectedProducts.length === 0" class="cart-buttons">
            <button @click="checkout"
                    class="btn btn-op1">
                Comprar Todo
            </button>
            <button @click="openCleanCartWarningModal"
                    class="btn btn-op2">
                Vaciar Carrito
            </button>
            <button @click="closeCart"
                    class="btn btn-op-close">
                Cerrar Carrito
            </button>
        </div>

        <div v-if="selectedProducts.length > 0" class="selected-total">
            <h4><strong>Total: ₡ {{ selectedTotal.toFixed(2) }}</strong></h4>
        </div>

        <div v-if="selectedProducts.length > 0" class="cart-buttons">
            <button class="btn btn-op1">
                Comprar Todo
            </button>
            <button class="btn btn-op2">
                Eliminar Seleccionado
            </button>
            <button @click="closeCart"
                    class="btn btn-op-close">
                Cerrar Carrito
            </button>
        </div>
    </div>
    <ActionModalConfirm ref="confirmCartModal" />
    <ActionModalWarning ref="warningCleanCartModal" @confirmed="clearCart" />
    <ActionModalWarning ref="warningInvalidCartModal" @confirmed="deleteInvalidItems" />
    <ActionModalError ref="errorCartModal" />
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import ActionModalConfirm from './ActionModalConfirm.vue';
    import ActionModalWarning from './ActionModalWarning.vue';
    import ActionModalError from './ActionModalError.vue';
    import { BackendUrl } from '../main.js';
    import axios from "axios";

    export default {
        components: {
            MainNavbar,
            ActionModalConfirm,
            ActionModalWarning,
            ActionModalError,
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
                selectedProducts: [],
                invalidProducts: [],
                userID: 0,
                confirmCartModal: false,
                warningCleanCartModal: false,
                warningInvalidCartModal: false,
                errorCartModal: false,
            };
        },
        methods: {
            getUserCart() {
                axios.get(`${BackendUrl}/ShoppingCart/${this.userID}`)
                    .then((response) => {
                        this.cartProducts = response.data;
                    })
                    .catch((error) => {
                        this.$refs.errorCartModal.openModal("Error al cargar el carrito", error);
                    });
            },
            closeCart() {
                this.$router.push({ name: 'Home' });
            },
            toggleSelect(product) {
                const productIndex =
                    this.selectedProducts.findIndex(selected => selected.productID === product.productID);
                if (productIndex > -1) {
                    this.selectedProducts.splice(productIndex, 1);
                } else {
                    this.selectedProducts.push({
                        productID: product.productID,
                        quantity: product.quantity,
                        total: product.total,
                    });
                }
            },
            getUserId() {
                const user = JSON.parse(localStorage.getItem('user'));
                return Number(user[0].userID);
            },
            checkout() {
                this.VerifyCartItems();
                this.$refs.confirmCartModal.openModal("Su compra se a completado");
            },
            openCleanCartWarningModal() {
                this.$refs.warningCleanCartModal.openModal("¿Estás seguro de que deseas vaciar el carrito? (Esta accion es irreversible)");
            },
            clearCart() {
                axios.delete(`${BackendUrl}/ShoppingCart/${this.userID}`)
                    .then(() => {
                        this.$refs.confirmCartModal.openModal("Carrito vaciado");
                        this.cartProducts = [];
                    })
                    .catch((error) => {
                        this.$refs.errorCartModal.openModal("Error al eliminar el carrito", error);
                    });
            },
            toggleSelectAll(event) {
                if (event.target.checked) {
                    this.selectedProducts = this.cartProducts.map(product => ({
                        productID: product.productID,
                        quantity: product.quantity,
                        total: product.total,
                    }));
                } else {
                    this.selectedProducts = [];
                }
            },
            VerifyCartItems() {
                axios.get(`${BackendUrl}/ShoppingCart/${this.clientId}/Verify`)
                .then((response) => {
                    this.invalidItems = response.data;
                    if (this.invalidItems.length > 0) {
                        this.$refs.warningInvalidCartModal.openModal("Se han detectado items invalidos (Exceden el stock disponible) \n ¿Desea eliminarlos del carrito?");
                    } else {
                        this.$refs.confirmCartModal.openModal("Su carrito es valido, se puede procesar la compra");
                    }
                })
                .catch((error) => {
                    this.$refs.errorCartModal.openModal("Error al verificar los productos del carrito", error);
                });

                
            },
            deleteInvalidItems() {
                axios.delete(`/api/ShoppingCart/${this.clientId}/delete-items`, this.invalidItems)
                    .then(() => {
                        this.$refs.confirmCartModal.openModal("Se han eliminado los elementos invalidos del carrito");
                        this.getUserCart();
                    })
                    .catch((error) => {
                        this.$refs.errorCartModal.openModal("Error al eliminar los elementos invalidos del carrito", error);
                    });
            }
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
    };
</script>

<style scoped>
    @import '../styles/GeneralStyle.css';
    @import '../styles/CartStyle.css';
</style>
