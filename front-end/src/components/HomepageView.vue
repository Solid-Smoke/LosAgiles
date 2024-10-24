<template>
    <MainNavbar />

    <b-container fluid class="px-5">
        <b-row>
            <b-col lg="2" md="3" class="d-none d-md-block category-sidebar">
                <h4 class="mb-4" style="margin-top: 20px;">Ordenar por Categoria</h4>

                <b-list-group flush>
                    <b-list-group-item class="category-item">Categoria 1</b-list-group-item>
                    <b-list-group-item class="category-item">Categoria 2</b-list-group-item>
                    <b-list-group-item class="category-item">Categoria 3</b-list-group-item>
                    <b-list-group-item class="category-item">Categoria 4</b-list-group-item>
                    <b-list-group-item class="category-item">Categoria 5</b-list-group-item>
                    <b-list-group-item class="category-item">Categoria 6</b-list-group-item>
                    <b-list-group-item class="category-item">Categoria 7</b-list-group-item>
                    <b-list-group-item class="category-item">Categoria 8</b-list-group-item>
                    <b-list-group-item class="category-item">Categoria 9</b-list-group-item>
                    <b-list-group-item class="category-item">Categoria 10</b-list-group-item>
                </b-list-group>
            </b-col>
        
            <b-col lg="10" md="9">
                <div class="container mt-1">
                    <h1 class="display-4 text-center"><strong>PRODUCTOS</strong></h1>
                </div>

                <b-row>
                    <b-col lg="3" md="4" sm="6" v-for="product in products" :key="product.productID">
                        <b-card :title="product.name" img-alt="Product Image" img-top class="mb-3">
                            <img :src="getProductImage(product.productImageBase64)" alt="Product Image" class="img-fluid d-block mx-auto" style="width: 250px; height: 250px;">
                            <b-card-text>{{ truncateDescription(product.description, 100) }}</b-card-text>
                            <b-card-text><strong>Precio: &#x20a1;{{ product.price }}</strong></b-card-text>
                            <b-button variant="primary">Añadir al Carrito</b-button>
                        </b-card>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
    </b-container>
</template>

<script>
import MainNavbar from './MainNavbar.vue';
import axios from 'axios';

export default {
    components: {
        MainNavbar
    },
    data() {
        return {
            products: []
        };
    },
    methods: {
        getProducts() {
            axios.get('https://localhost:7168/api/Products/GetAllProducts')
                .then(response => {
                    this.products = response.data;
                })
                .catch(error => {
                    console.error("Error obteniendo productos:", error);
                });
        },
        getProductImage(productImageBase64) {

            if (!productImageBase64) {
                return "https://via.placeholder.com/250";
            }

            return `data:image/png;base64,${productImageBase64}`;
        },
        truncateDescription(text, maxLength) {
            if (text.length > maxLength) {
                return text.substring(0, maxLength) + '...';
            }
            return text;
        }
    },
    mounted() {
        this.getProducts();
    }
};
</script>

<style lang="scss" scoped>
</style>
