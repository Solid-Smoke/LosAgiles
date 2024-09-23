import { createApp } from "vue";
import App from "./App.vue";
import { createRouter, createWebHistory } from "vue-router";
import AddAddressForm from "./components/AddAddressForm.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/AddressListView/AddAddressForm",
      name: "AddAddressForm",
      component: AddAddressForm,
    },
  ],
});

createApp(App).use(router).mount("#app");
