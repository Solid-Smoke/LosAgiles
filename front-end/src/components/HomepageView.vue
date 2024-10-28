<template>
    <MainNavbar />
    <SearchBar :startSearchIndex="this.startSearchIndex" 
        :maxResults="this.maxSearchResultsPerPage"
        @search-made="actualResultsPage = 0;"
        @products-retreived="(products) => {this.products = products;
        }"
        @products-counted="(count) => this.searchResultsCount = count"
        id="searchbar"/>
    <b-button-group style="float: right;">
        <span>
            {{ actualResultsPage + 1 }}/{{ totalPagesBySearch }}
        </span>
        <b-button variant="primary" @click="actualResultsPage = 0;">Inicio</b-button>
        <b-button variant="primary" @click="goPreviousPage">Anterior</b-button>
        <b-button variant="primary" @click="goNextPage">Siguiente</b-button>
    </b-button-group>
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
                    <b-col lg="3" md="4" sm="6"
                    v-for="(product, index) of products" :key="index">
                        <b-card :title="product.name"
                            :img-src="getProductImage(
                                product.productImageInBase64)"
                            img-alt="Imagen de producto" img-top class="mb-3">

                        <b-card-text>{{product.description}}</b-card-text>

                        <b-card-text>
                            <strong>Precio: &#x20a1;{{ product.price }}</strong>

                        </b-card-text>
                            <b-button variant="primary" @click="openProductPage(product.productId);">
                                Añadir al Carrito
                            </b-button>
                        </b-card>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
    </b-container>
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
                maxSearchResultsPerPage: 8,
                actualResultsPage: 0,
                searchResultsCount: 0
            };
        },
        methods: {
            getProductImage(productImageBase64) {
                if (!productImageBase64) {
                    return "https://imporpec.com.bo/images/" +
                        "image_not_available.gif";
                } else {
                    return `data:image/png;base64,${productImageBase64}`;
                }
            },
            goNextPage() {
                if(this.actualResultsPage + 1 < (
                    Math.ceil(this.searchResultsCount /
                    this.maxSearchResultsPerPage)))
                {
                    this.actualResultsPage += 1;
                }
            },
            goPreviousPage() {
                if(this.actualResultsPage > 0) {
                    this.actualResultsPage -= 1;
                }
            },
            openProductPage(productID) {
                this.$router.push({ name: 'IndividualProductPage', params: { id: productID } });
            },
        },
        computed: {
            startSearchIndex() {
                return this.actualResultsPage * this.maxSearchResultsPerPage;
            },
            totalPagesBySearch() {
                let totalPagesCalculation = Math.ceil(this.searchResultsCount /
                    this.maxSearchResultsPerPage);
                if (totalPagesCalculation == 0) {
                    return 1;
                } else {
                    return totalPagesCalculation;
                }
            }
        },
    };
</script>

<style lang="css" scoped>
    #searchbar {
        padding-right: 30%;
        padding-left: 10%;
        padding-bottom: 10px;
        margin-bottom: 60px;
        background-color: #0DCAF0;
    }
</style>
