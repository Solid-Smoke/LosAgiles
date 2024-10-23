<template>
    <b-nav-form>
        <b-form-input size="sm" placeholder="Search" v-model="searchText" />
        <b-button size="sm" class="my-2 my-sm-0" type="submit" 
            @click="searchProducts(this.searchText, this.startSearchIndex,
                this.maxResults)">Search</b-button>
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
        data() {
            return {
                searchText: ""
            };
        },
        methods: {
            searchProducts(searchText, startIndex, maxResults) {
                axios
                    .get(
                        BackendUrl + "/Products",
                        {
                            params: {
                                searchText: searchText,
                                startIndex: startIndex,
                                maxResults: maxResults,
                            }
                        }
                    )
                    .then(
                        (response) => {
                            this.$emit('productSearchResultsObtained', response)
                        }
                    )
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        }
    }
</script>

<style scoped>

</style>