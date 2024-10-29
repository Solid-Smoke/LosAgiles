<template>
    <MainNavbar />
    <SearchBar 
        :startSearchIndex="startSearchIndex" 
        :maxResults="productsPerPage"
        @search-made="isSearchActive = true; currentPage = 1;"
        @products-retrieved="updateProducts"
        @products-counted="(count) => searchResultsCount = count"
        @resetSearch="getProducts"
        id="searchbar" />

    <b-container fluid class="px-5">
        <b-row style>
            <b-col>
                <div v-if="products.length == 0" class="container mt-1">
                    <h6 class="text-center">
                    <br><br><br><br>
                    <strong>No se encontraron productos</strong>
                    </h6>
                </div>
                <b-row>
                    <b-col lg="3" md="4" sm="6" v-for="product in paginatedProducts" :key="product.productID">
                        <b-card :title="product.name" img-alt="Product Image" img-top class="product-card mb-3" @click="goToProduct(product.productID)">
                            <img :src="getProductImage(product.productImageBase64)" alt="Product Image" class="img-fluid d-block mx-auto product-image" style="width: 250px; height: 250px;"/>
                            <b-card-text class="product-description">{{ truncateDescription(product.description, 165) }}</b-card-text>
                            <b-card-text><strong>Precio: &#x20a1;{{ product.price }}</strong></b-card-text>
                        </b-card>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
    </b-container>
    <div style="display: flex; justify-content: flex-end;">
        <b-button-group style="float: right; margin-bottom: 10%; margin-right: 5%">
            <span>
                {{ actualResultsPage + 1 }}/{{ totalPagesBySearch }}
            </span>
            <b-button variant="success" style="font-size: 1.25rem;" @click="actualResultsPage = 0;">Inicio</b-button>
            <b-button variant="primary" style="font-size: 1.25rem;" @click="goPreviousPage">◄ Anterior</b-button>
            <b-button variant="primary" style="font-size: 1.25rem;" @click="goNextPage">Siguiente ►</b-button>
        </b-button-group>
    </div>
</template>

<script>
import MainNavbar from './MainNavbar.vue';
import SearchBar from './SearchBar.vue';

export default {
    components: {
        MainNavbar,
        SearchBar
    },
    data() {
        return {
            products: [],
            maxSearchResultsPerPage: 16,
            actualResultsPage: 0,
            searchResultsCount: 0
        };
    },
    methods: {
        getProducts() {
            axios
                .get(`${BackendUrl}/Products/GetAllProducts`)
                .then((response) => {
                    this.products = response.data;
                    this.isSearchActive = false;
                    this.currentPage = 1;
                })
                .catch((error) => {
                    console.error('Error obteniendo productos:', error);
                });
        },
        updateProducts(products) {
            this.products = products;
            this.currentPage = 1;
            this.isSearchActive = true;
        },
        getProductImage(productImageBase64) {
            if (!productImageBase64) {
                return "https://imporpec.com.bo/images/" + "image_not_available.gif";
            }
            return `data:image/png;base64,${productImageBase64}`;
        },
        truncateDescription(text, maxLength) {
            if (text.length > maxLength) {
                return text.substring(0, maxLength) + '...';
            }
            return text;
        },
        goToProduct(productID) {
            this.$router.push({ name: 'IndividualProductPage', params: { id: productID } });
        },
        onPageChange(page) {
            this.currentPage = page;
        },
    },  
    mounted() {
        this.getProducts();
    },
};
</script>

<style scoped>
#searchbar {
    padding-right: 30%;
    padding-left: 10%;
    padding-bottom: 10px;
    margin-bottom: 60px;
    background-color: #0DCAF0;
}

.product-card {
    cursor: pointer;
    transition: transform 0.2s ease, box-shadow 0.2s ease;
    box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
    width: 100%;
}

.product-card:hover {
    transform: translateY(-5px) scale(1.03);
    box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.2);
    border: 1px solid #007bff;
}

.product-description {
    min-height: 95px;
    text-align: justify;
}
</style>
