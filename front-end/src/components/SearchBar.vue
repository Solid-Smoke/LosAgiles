<template>
    <b-nav-form>
        <b-form-input size="sm" placeholder="Search" v-model="searchText" />
        <b-button size="sm" class="my-2 my-sm-0" type="submit" 
            @click="searchProducts(this.startSearchIndex,
                this.maxResults, this.searchText)">Search</b-button>
    </b-nav-form>
</template>

<script>
import { BackendUrl } from '@/main';
import axios from 'axios';
    export default {
        props: {
            startSearchIndex: Number,
            maxResults: Number,
        },
        watch: {
            startSearchIndex: function() {
                this.searchProducts(this.startSearchIndex,
                    this.maxResults, this.searchText)
            }
        },
        data() {
            return {
                products: [],
                searchText: "",
            };
        },
        methods: {
            searchProducts(startIndex, maxResults, searchText) {
                axios
                .get(BackendUrl +
                    "/Products", {params: {
                        startIndex,
                        maxResults,
                        searchText
                    }})
                .then(
                    (response) => {
                        this.countProductsBySearch(this.searchText);
                        this.products = response.data;
                        this.$emit('productsRetreived', this.products);
                    })
                .catch(function (error) {
                    console.log(error);
                });
            },
            countProductsBySearch(searchText) {
                axios
                .get(BackendUrl +
                    "/Products/CountProductsBySearch",
                    {params: {searchText}})
                .then(
                    (response) => {
                        this.$emit('productsCounted', response.data);
                    })
                .catch(function (error) {
                    console.log(error);
                });
            }
        },
        mounted() {
            this.searchProducts(0, this.maxResults, "");
        }
    }
</script>

<style scoped>

</style>