<template>
    <MainNavbar />

    <b-container fluid class="px-5">
        <b-row style>
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
                <div class="container mt-1"><h1 class="display-4 text-center"><strong>PRODUCTOS</strong></h1></div>

                <b-row>
                    <b-col lg="3" md="4" sm="6" v-for="index in 12" :key="index">
                        <b-card title="Nombre Producto" img-src="https://via.placeholder.com/150" img-alt="Product Image" img-top class="mb-3">

                        <b-card-text>Descripción.</b-card-text>

                        <b-card-text><strong>Precio: &#x20a1;1000</strong>

                        </b-card-text><b-button variant="primary">Añadir al Carrito</b-button></b-card>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
    </b-container>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';

    export default {
        components: {
            MainNavbar
        },
    };
</script>

<style lang="scss" scoped>
</style>