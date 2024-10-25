<template>
    <b-nav-form>
        <b-form-input size="sm" placeholder="Search" v-model="searchText"></b-form-input>
        <b-button size="sm" class="my-2 my-sm-0" type="submit" 
            @click="searchProducts(this.searchText, this.startSearchIndex, this.maxResults)">Search</b-button>
    </b-nav-form>
</template>

<script>
    export default {
        props: {
            startSearchIndex: Number,
            maxResults: Number
        },
        data() {
            return {
                searchText: ""
            };
        },
        methods: {
            searchProducts(searchText, startIndex, maxResults) {
                let products = [];
                return products;
            }
        }
    }
</script>

<style scoped>

</style>