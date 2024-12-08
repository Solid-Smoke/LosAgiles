<template>
  <MainNavbar/>
  <SearchBar :startSearchIndex="startSearchIndex" :maxResults="productsPerPage" @search-made="isSearchActive = true; currentPage = 1;" @products-retrieved="updateProducts" @products-counted="(count) => searchResultsCount = count" @resetSearch="getProducts" id="searchbar" />

  <b-container fluid class="px-4">
      <b-row>
          <b-col lg="8" md="8">
              <b-row>
                  <b-col lg="3" md="4" sm="6" v-for="product in paginatedProducts" :key="product.productID">
                    <b-card :title="product.name" img-alt="Product Image" img-top class="product-card mb-3" @click="goToProduct(product.productID)">
                        <img :src="getProductImage(product.productImageBase64)" alt="Product Image" class="img-fluid d-block mx-auto product-image" style="width: 135px; height: 135px;" />
                        <b-card-text class="product-description">{{ truncateDescription(product.description, 128) }}</b-card-text>
                        <b-card-text><strong>Precio: &#x20a1;{{ formatPrice(product.price) }}</strong></b-card-text>
                    </b-card>
                  </b-col>
              </b-row>
          </b-col>

          <b-col lg="4" md="4">
            <b-card class="orders mb-3" style="height: calc(50% - 1rem); overflow-y: scroll;">
              <b-card-title>Órdenes en Progreso</b-card-title>
              <b-list-group>
                <b-list-group-item v-if="ordersInProgress.length === 0">No hay órdenes en progreso.</b-list-group-item>
                <b-list-group-item v-if="ordersInProgress.length !== 0">
                  <div class="d-flex justify-content-between">
                    <span style="width: 30%"><strong>Orden</strong></span>
                    <span style="width: 40%; text-align: center"><strong>Estado</strong></span>
                    <span style="width: 30%; text-align: right"><strong>Total</strong></span>
                  </div>
                </b-list-group-item>
                <b-list-group-item v-for="order in ordersInProgress" :key="order.orderID">
                  <div class="d-flex justify-content-between" style="width: 100%;">
                    <span style="width: 30%">#{{ order.orderID }}</span>
                    <span style="width: 40%; text-align: center">{{ order.status }}</span>
                    <span style="width: 30%; text-align: right">&#x20a1;{{ formatPrice(order.totalAmount) }}</span>
                  </div>
                </b-list-group-item>
              </b-list-group>
            </b-card>

            <b-card class="ten-products mb-3" style="height: calc(50% - 1rem); overflow-y: scroll;">
              <b-card-title>Últimos 10 Productos Comprados</b-card-title>
              <b-list-group>
                <b-list-group-item v-if="lastTenPurchased.length === 0">No hay productos comprados recientemente.</b-list-group-item>
                <b-list-group-item v-if="lastTenPurchased.length !== 0">
                  <div class="d-flex justify-content-between">
                    <span style="width: 30%"><strong>Producto</strong></span>
                    <span style="width: 40%; text-align: center"><strong>Cantidad</strong></span>
                    <span style="width: 30%; text-align: right"><strong>Precio Unitario</strong></span>
                  </div>
                </b-list-group-item>
                <b-list-group-item v-for="item in lastTenPurchased" :key="item.productID">
                  <div class="d-flex justify-content-between" style="width: 100%;">
                    <span style="width: 30%;"> {{ item.productName }}</span>
                    <span style="width: 40%; text-align: center"> {{ item.amount }}</span>
                    <span style="width: 30%; text-align: right">&#x20a1;{{ formatPrice(item.price) }}</span>
                  </div>
                </b-list-group-item>
              </b-list-group>
            </b-card>
          </b-col>

          <p v-if="paginatedProducts.length === 0" class="text-center mt-4"><strong>No hay productos disponibles.</strong></p>

          <b-pagination v-model="currentPage" :total-rows="isSearchActive ? searchResultsCount : products.length" :per-page="productsPerPage" align="center" class="mt-3" @change="onPageChange"></b-pagination>
      </b-row>
  </b-container>
</template>

<script>
import { BackendUrl } from '@/main';
import MainNavbar from './MainNavbar.vue';
import SearchBar from './SearchBar.vue';
import axios from 'axios';

export default {
  components: {
    MainNavbar,
    SearchBar,
  },
  data() {
    return {
      products: [],
      currentPage: 1,
      productsPerPage: 8,
      startSearchIndex: 0,
      searchResultsCount: 0,
      isSearchActive: false,
      ordersInProgress: [],
      lastTenPurchased: [],
    };
  },
  computed: {
    paginatedProducts() {
      const start = (this.currentPage - 1) * this.productsPerPage;
      const end = start + this.productsPerPage;
      return this.products.slice(start, end);
    },
  },
  methods: {
    getProducts() {
      axios
        .get(`${BackendUrl}/Products/GetAllProducts`)
        .then((response) => {
          this.products = response.data;
          this.isSearchActive = false;
          this.currentPage = 1;
        })
        .catch((error) => {
          console.error('Error obteniendo productos:', error);
        });
    },
    updateProducts(products) {
      this.products = products;
      this.currentPage = 1;
      this.isSearchActive = true;
    },
    getOrdersInProgress(userID) {
      axios
          .get(`${BackendUrl}/Order/Orders/Excluding/Completed/${userID}`)
          .then((response) => {
              console.log("Órdenes en progreso:", response.data);
              this.ordersInProgress = response.data;
          })
          .catch((error) => {
              console.error('Error obteniendo órdenes en progreso:', error);
          });
    },
    getLastTenPurchased(userID) {
      console.log("Obteniendo últimos 10 productos comprados para usuario:", userID);
      axios
          .get(`${BackendUrl}/Order/Last/Ten/Purchased/${userID}`)
          .then((response) => {
              console.log("Respuesta del backend (Últimos 10 productos):", response.data);
              this.lastTenPurchased = response.data;
          })
          .catch((error) => {
              console.error("Error obteniendo productos comprados:", error);
          });
    },
    getProductImage(productImageBase64) {
      if (!productImageBase64) {
        return 'https://imporpec.com.bo/images/image_not_available.gif';
      }
      return `data:image/png;base64,${productImageBase64}`;
    },
    truncateDescription(text, maxLength) {
      if (text.length > maxLength) {
        return text.substring(0, maxLength) + '...';
      }
      return text;
    },
    formatPrice(price) {
      if (price !== null && price !== undefined) {
        return new Intl.NumberFormat('en-US').format(price);
      }
      return price;
    },
    goToProduct(productID) {
      this.$router.push({ name: 'IndividualProductPage', params: { id: productID } });
    },
    onPageChange(page) {
      this.currentPage = page;
    },
  },
  mounted() {
    const userID = JSON.parse(localStorage.getItem('user'))?.[0]?.userID;
    this.getProducts();
    this.getOrdersInProgress(userID);
    this.getLastTenPurchased(userID);
  },
};
</script>

<style scoped>
#searchbar {
  padding-right: 20%;
  padding-left: 20%;
  padding-bottom: 10px;
  margin-bottom: 20px;
  background-color: #0DCAF0;
  width: 100%;
}

.product-card {
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
  width: 100%;
}

.orders {
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
}

.ten-products {
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
}

.product-card:hover {
  transform: translateY(-5px) scale(1.03);
  box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.2);
  border: 1px solid #007bff;
}

.product-description {
  min-height: 95px;
  text-align: justify;
}

b-card {
  height: auto;
  margin-bottom: 20px;
}

.scrollable-card {
  overflow-y: auto;
  max-height: 400px;
}
</style>
