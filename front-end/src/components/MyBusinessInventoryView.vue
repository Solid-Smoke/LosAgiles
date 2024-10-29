<template>
    <MainNavbar />
    <h1 class="display-4 text-center mb-4"><strong>Inventario</strong></h1>
    <div class="table-responsive-sm">
        <table class="table-custom table-striped">
            <thead class="table-header">
                <tr>
                    <th scope="col">ID del Producto</th>
                    <th scope="col">Nombre</th>
                    <th scope="col">Descripción</th>
                    <th scope="col">Precio</th>
                    <th scope="col">Stock</th>
                    <th scope="col">Perecedero</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="product in products" :key="product.productID">
                    <td class="table-cell">{{ product.productID }}</td>
                    <td class="table-cell">{{ product.name }}</td>
                    <td class="table-cell">{{ product.description }}</td>
                    <td class="table-cell">{{ product.price }}</td>
                    <td class="table-cell">{{ product.stock }}</td>
                    <td class="table-cell">{{ product.perishable ? 'Sí' : 'No' }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import { BackendUrl } from '../main.js';
    import axios from "axios";

    export default {
        components: {
            MainNavbar
        },
        data() {
            return {
                currentBusinessId: "",
                products: [
                    {
                        productID: 1,
                        name: "Product 1",
                        description: "Description for Product 1",
                        category: "Category A",
                        price: 100,
                        stock: 20,
                        weight: 1.5,
                        isPerishable: true,
                        dailyAmount: 5,
                        daysAvailable: "Mon-Sun",
                        businessID: null,
                    },
                    {
                        productID: 2,
                        name: "Product 2",
                        description: "Description for Product 2",
                        category: "Category B",
                        price: 200,
                        stock: 10,
                        weight: 2.0,
                        isPerishable: false,
                        dailyAmount: null,
                        daysAvailable: "Tue-Sat",
                        businessID: null,
                    },
                ],

            };
        },
        methods: {
            getBusinessInventory() {
                axios.get(`${BackendUrl}/Products/Business/${this.currentBusinessId}`, {
                }).then(
                    (response) => {
                        this.products = response.data;
                    }
                );
            },
        },
        created() {
            this.currentBusinessId = this.$route.query.businessID;
            this.getBusinessInventory();
        }
    };
</script>

<style scoped>
    @import '../styles/GeneralStyle.css';
</style>
