<template>
    <b-nav-form>
        <b-form-input size="sm"
            placeholder="Buscar producto, categoría o empresa..."
            v-model="searchText" :state="inputIsValid"/>
        <b-button size="sm" class="my-2 my-sm-0" type="submit" 
            @click="searchProducts(this.startSearchIndex,
                this.maxResults, this.searchText)">Buscar</b-button>
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
                maxLenghtSearchText: 150
            };
        },
        computed: {
            inputIsValid() {
                let validationRegex =
                new RegExp('^[a-zA-Z0-9-+_$/&#\\sÀ-ÖØ-öø-ÿ]*$');
                return (this.searchText.length <= this.maxLenghtSearchText
                    && validationRegex.test(this.searchText))
            }
        },
        methods: {
            searchProducts(startIndex, maxResults, searchText) {
                if(this.inputIsValid) {
                    axios
                    .get(BackendUrl +
                        "/Products", {params: {
                            startIndex,
                            maxResults,
                            searchText: searchText.trim()
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
                }
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
            },
        },
        mounted() {
            this.searchProducts(0, this.maxResults, "");
        }
    }
</script>

<style scoped>

</style>