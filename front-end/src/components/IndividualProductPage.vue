<template>
    <MainNavbar />
  
    <b-container class="product-page">
      <b-row class="justify-content-center">
        <b-col lg="8" md="10" sm="12" class="text-center">
          <img :src="getProductImage(product.productImageBase64)" alt="Product Image" class="img-fluid product-image mb-3" />
          <h2 class="product-name">{{ product.name }}</h2>
          <p class="product-price"><strong>Precio:</strong> &#x20a1;{{ product.price ? product.price.toFixed(2) : 'No disponible' }}</p>
        </b-col>
      </b-row>
  
      <b-row class="justify-content-center">
        <b-col lg="8" md="10" sm="12" class="text-left">
          <p><strong>Stock:</strong> {{ product.stock }}</p>
          <p><strong>Peso:</strong> {{ product.weight }} kg</p>
          <p class="product-description"><strong>Descripción:</strong> {{ product.description }}</p>
          <p><strong>Categoría:</strong> {{ product.category }}</p>
  
          <div v-if="product.isPerishable">
            <p><strong>Cantidad diaria:</strong> {{ product.dailyAmount }}</p>
            <p><strong>Días disponibles:</strong> {{ product.daysAvailable }}</p>
          </div>
  
          <p class="business-name"><strong>Emprendimiento:</strong> {{ product.businessName }}</p>
  
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
  import { BackendUrl } from '@/main';
import MainNavbar from './MainNavbar.vue';
  import axios from 'axios';
  
  export default {
    components: {
      MainNavbar,
    },
    data() {
      return {
        product: {},
        quantity: 1,
      };
    },
    methods: {
      getProductImage(productImageBase64) {
        if (!productImageBase64) {
          return "https://imporpec.com.bo/images/" + "image_not_available.gif";
        }
        return `data:image/png;base64,${productImageBase64}`;
      },
      getProductDetails(productId) {
        axios
          .get(`${BackendUrl}/Products/${productId}`)
          .then((response) => {
            this.product = response.data;
          })
          .catch((error) => {
            console.error('Error obteniendo detalles del producto:', error);
          });
      },
    },
    mounted() {
      const productId = this.$route.params.id;
      this.getProductDetails(productId);
    },
  };
  </script>
  
  <style scoped>
  .product-page {
    min-height: 90vh;
    padding: 10px 5px;
    display: flex;
    flex-direction: column;
    justify-content: center;
  }
  
  .product-image {
    width: 150px;
    height: auto;
    border-radius: 10px;
    object-fit: cover;
  }
  
  .product-name {
    font-size: 1.5rem;
    font-weight: bold;
    margin-bottom: 8px;
  }
  
  .product-price {
    font-size: 1.2rem;
    color: #333;
    margin-bottom: 12px;
  }
  
  .business-name {
    font-size: 1rem;
    color: #070707;
    margin-bottom: 10px;
  }
  
  .product-description {
    text-align: justify;
  }
  
  .add-to-cart-btn {
    background-color: #007bff;
    font-size: 1rem;
    padding: 6px 15px;
  }
  
  b-form-input {
    font-size: 1rem;
  }
  
  .product-page div {
    margin-bottom: 3px;
  }
  </style>
  