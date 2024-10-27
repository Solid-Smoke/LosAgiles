<template>
  <MainNavbar />
  
  <b-container fluid class="px-5">
    <div class="container mt-1">
      <h1 class="display-4 text-center"><strong>PRODUCTOS</strong></h1>
    </div>

    <b-row>
      <b-col lg="10" md="9">

        <b-row>
          <b-col lg="3" md="4" sm="6" v-for="product in products" :key="product.productID">
            <b-card :title="product.name" img-alt="Product Image" img-top class="product-card mb-3" @click="goToProduct(product.productID)">
              <img :src="getProductImage(product.productImageBase64)" alt="Product Image" class="img-fluid d-block mx-auto" style="width: 250px; height: 250px;" />
              <b-card-text class="product-description">{{ truncateDescription(product.description, 127) }}</b-card-text>
              <b-card-text><strong>Precio: &#x20a1;{{ product.price }}</strong></b-card-text>
              <b-button variant="primary">Añadir al Carrito</b-button>
            </b-card>
          </b-col>
        </b-row>
      </b-col>
    </b-row>
  </b-container>
</template>

<script>
import { BackendUrl } from '@/main';
import MainNavbar from './MainNavbar.vue';
import axios from 'axios';

export default {
  components: {
    MainNavbar,
  },
  data() {
    return {
      products: [],
    };
  },
  methods: {
    getProducts() {
      axios
        .get(`${BackendUrl}/Products/GetAllProducts`)
        .then((response) => {
          console.log('Datos API: ', response.data);
          this.products = response.data;
        })
        .catch((error) => {
          console.error('Error obteniendo productos:', error);
        });
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
  },
  mounted() {
    this.getProducts();
  },
};
</script>

<style scoped>
.product-card {
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
  
  &:hover {
    transform: translateY(-5px) scale(1.03);
    box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.2);
    border: 1px solid #007bff;
  }
}

.product-description {
  text-align: justify;
}
</style>
