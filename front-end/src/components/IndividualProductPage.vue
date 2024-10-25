<template>
    <MainNavbar />

    <b-container class="product-page">
        <b-row class="justify-content-center">
            <b-col lg="8" md="10" sm="12" class="text-center">
                <img :src="getProductImage(product.productImageBase64)" alt="Product Image" class="img-fluid product-image mb-4" />
                <h2 class="product-name">{{ product.name }}</h2>
                <p class="product-price"><strong>Precio:</strong> &#x20a1;{{ product.price ? product.price.toFixed(2) : 'No disponible' }}</p>
            </b-col>
        </b-row>

        <b-row class="justify-content-center">
            <b-col lg="8" md="10" sm="12" class="text-left">
                <p><strong>Stock:</strong> {{ product.stock }}</p>
                <p><strong>Peso:</strong> {{ product.weight }} kg</p>
                <p><strong>Descripción:</strong> {{ product.description }}</p>
                <p><strong>Categoría:</strong> {{ product.category }}</p>

                <div v-if="product.isPerishable">
                    <p><strong>Cantidad diaria:</strong> {{ product.dailyAmount }}</p>
                    <p><strong>Días disponibles:</strong> {{ product.daysAvailable }}</p>
                </div>

                <b-form-group label="Cantidad a ordenar:">
                    <b-form-input type="number" v-model="quantity" min="1" :max="product.stock" />
                </b-form-group>

                <div class="text-center mt-2">
                    <b-button variant="primary" class="add-to-cart-btn">Añadir al Carrito</b-button>
                </div>
            </b-col>
        </b-row>
    </b-container>
</template>

<script>
export default {

};
</script>
  
<style scoped>
  
</style>
  