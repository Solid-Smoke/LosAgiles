<template>
    <MainNavbar />
    <SearchBar :startSearchIndex="this.startSearchIndex" 
        :maxResults="this.maxSearchResultsPerPage"
        @products-retreived="(products) => this.products = products"/>

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
                    <b-col lg="3" md="4" sm="6" v-for="(product, index) of products" :key="index">
                        <b-card :title="product.name" :img-src="getProductImage(product.productImageInBase64)" img-alt="Product Image" img-top class="mb-3">

                        <b-card-text>{{product.description}}</b-card-text>

                        <b-card-text><strong>Precio: &#x20a1;{{ product.price }}</strong>

                        </b-card-text><b-button variant="primary">Añadir al Carrito</b-button></b-card>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
    </b-container>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import SearchBar from './SearchBar.vue';
    import axios from 'axios';
    import { BackendUrl } from '@/main';

    export default {
        components: {
            MainNavbar,
            SearchBar
        },
        data() {
            return {
                products: [
                    {
                        name: "",
                        description: "",
                        price: 0,
                        bussinessName: "",
                        productImage: "",
                        productImageInBase64: ""
                    }
                ],
                maxSearchResultsPerPage: 15,
                actualResultsPage: 0
            };
        },
        methods: {
            searchProducts(startIndex, maxResults) {
                axios
                .get(BackendUrl +
                    "/Products", {params: {
                        startIndex,
                        maxResults
                    }})
                .then(
                    (response) => {
                        this.products = response.data;
                        console.log(this.products);
                    })
                .catch(function (error) {
                    console.log(error);
                });
            },
            getProductImage(productImageBase64) {
                if (!productImageBase64) {
                    console.log("No image available , using placeholder")
                    return "https://via.placeholder.com/250";
                }
                return `data:image/png;base64,${productImageBase64}`;
            },
        },
        computed: {
            startSearchIndex() {
                return this.actualResultsPage * this.maxSearchResultsPerPage;
            }
        },
    };
</script>

<style lang="scss" scoped>
</style>
