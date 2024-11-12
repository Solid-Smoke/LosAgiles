<template>
    <b-nav-form>
        <b-form-input placeholder="Buscar producto, categoría o empresa..." v-model="searchText" :state="inputIsValid" aria-describedby="input-live-feedback" />

        <b-button size="sm" class="my-2 my-sm-0" type="submit" @click="onSearchClick" >Buscar</b-button>
       
        <b-form-invalid-feedback id="input-live-feedback" style="background-color: white;" >
            El texto tiene más de {{ maxLenghtSearchText }} caracteres o se
            ingresó algún caracter especial no permitido. Caracteres especiales
            permitidos: {{ allowedSpecialCharacters }}
        </b-form-invalid-feedback>
    </b-nav-form>
</template>

<script>
import { BackendUrl } from "@/main";
import axios from "axios";

export default {
    props: {
        startSearchIndex: Number,
        maxResults: Number,
    },
    watch: {
        startSearchIndex() {
            this.searchProducts(this.startSearchIndex, this.maxResults, this.searchText);
        },
        searchText() {
            this.$emit("searchMade");
        },
    },
    data() {
        return {
            products: [],
            searchText: "",
            maxLenghtSearchText: 50,
            allowedSpecialCharacters: "-+_$/&#",
        };
    },
    computed: {
        inputIsValid() {
            let validationRegex = new RegExp("^[a-zA-Z0-9-+_$/&#\\sÀ-ÖØ-öø-ÿ]*$");
            return (
                this.searchText.length <= this.maxLenghtSearchText &&
                validationRegex.test(this.searchText)
            );
        },
    },
    methods: {
        onSearchClick() {
            this.$emit("searchMade");
            if (this.searchText.trim() === "") {
                this.$emit("resetSearch");
            } else {
                this.searchProducts(0, this.maxResults, this.searchText);
            }
        },
        searchProducts(startIndex, maxResults, searchText) {
            if (this.inputIsValid) {
                axios
                    .get(`${BackendUrl}/Products`, {
                        params: {
                            startIndex,
                            maxResults,
                            searchText: searchText.trim() || "",
                        },
                    })
                    .then((response) => {
                        this.products = response.data.map((product) => ({
                            ...product,
                            productID: product.productID,
                        }));

                        this.$emit("productsRetrieved", this.products);
                        this.countProductsBySearch(searchText);
                    })
                    .catch((error) => {
                        console.error("Error en la búsqueda de productos:", error);
                    });
            }
        },
        countProductsBySearch(searchText) {
            axios
                .get(`${BackendUrl}/Products/CountProductsBySearch`, {
                    params: { searchText: searchText.trim() || "" },
                })
                .then((response) => {
                    this.$emit("productsCounted", response.data);
                })
                .catch((error) => {
                    console.error("Error contando productos:", error);
                });
        },
    },
    mounted() {
        this.searchProducts(0, this.maxResults, "");
    },
};
</script>

<style scoped>
</style>
