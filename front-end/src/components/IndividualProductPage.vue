<template>
  <template v-if="isClient"><MainNavbar /></template>
  <template v-if="isAdmin"><AdminNavbar /></template>
  <template v-if="!isClient && !isAdmin"><UnregisteredNavbar /></template>

  <b-container class="product-page">
    <b-row class="justify-content-center">
      <b-col lg="8" md="10" sm="12" class="text-center">
        <img :src="getProductImage(product.productImageBase64)" alt="Product Image" class="img-fluid product-image mb-3" />
        <h2 class="product-name">{{ product.name }}</h2>
        <p class="product-price"><strong>Precio:</strong> &#x20a1;{{ formatPrice(product.price) }}</p>
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
          <b-form-select v-model="quantity" :options="quantityOptions" />
        </b-form-group>

        <div class="text-center mt-2">
          <template v-if="isClient">
              <b-button variant="primary" class="add-to-cart-btn" @click="confirmAddToCart">Añadir al Carrito</b-button>
          </template>
          <template v-else>
              <b-button variant="primary" class="add-to-cart-btn" @click="redirectToRegister">Registrarse en el sitio</b-button>
          </template>
          <b-button variant="primary" class="close-add-to-cart-btn" @click="closeCart">Volver al Homepage</b-button>
        </div>
      </b-col>
    </b-row>
    <ActionModalConfirm ref="confirmProductModal"/>
    <ActionModalError ref="errorProductModal"/>
    <ActionModalWarning ref="warningProductModal" @confirmed="addToShoppingCart" />
  </b-container>
</template>
  
  <script>
  import { BackendUrl } from '@/main';
  import MainNavbar from './MainNavbar.vue';
  import AdminNavbar from './AdminNavbar.vue';
  import UnregisteredNavbar from './UnregisteredNavbar.vue';
  import ActionModalConfirm from './ActionModalConfirm.vue';
  import ActionModalError from './ActionModalError.vue';
  import ActionModalWarning from './ActionModalWarning.vue';
  import axios from 'axios';
  
  export default {
    components: {
      MainNavbar,
      AdminNavbar,
      UnregisteredNavbar,
      ActionModalConfirm,
      ActionModalWarning,
      ActionModalError,
  },
  data() {
    return {
      product: {},
      quantity: 1,
    };
  },
  computed: {
    quantityOptions() {
      return Array.from({ length: this.product.stock }, (_, i) => i + 1);
    }
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
    confirmAddToCart(){
        this.$refs.warningProductModal.openModal("¿Deseas añadir este producto al carrito?");
    },
    addToShoppingCart() {
      const user = JSON.parse(localStorage.getItem('user'));
      const id = Number(user[0].userID);

      axios
        .post(`${BackendUrl}/ShoppingCart/${id}`, {
            productId: this.product.productID,
            amount: this.quantity
        })
        .then(() => {
            this.$refs.confirmProductModal.openModal("Se ha añadido el producto al carrito de forma exitosa");
            setTimeout(() => {
                this.$router.push({ name: 'Home' });
            }, 2000);
        })
        .catch(() => {
            this.$refs.errorProductModal.openModal("Error al añadir el producto al carrito");
        });
    },
    closeCart() {
      this.$router.push({ name: 'Home' });
    },
    redirectToRegister() {
        this.$router.push('/registro');
    },
    formatPrice(price) {
      if (price !== null && price !== undefined) {
        return new Intl.NumberFormat('en-US').format(price);
      }
      return price;
    },
  },
  mounted() {
    const productId = this.$route.params.id;
    this.getProductDetails(productId);
  },
  props: {
      isAdmin: {
          type: Boolean,
          required: true,
      },
      isClient: {
          type: Boolean,
          required: true,
      },
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
  background-color: #1a8654;
  font-size: 1rem;
  padding: 10px 20px;
  margin: 0 10px;
}

.close-add-to-cart-btn {
  background-color: #0cbde1;
  font-size: 1rem;
  padding: 10px 20px;
  margin: 0 10px;
}

.b-form-input {
  font-size: 1rem;
}

.product-page div {
  margin-bottom: 3px;
}
</style>