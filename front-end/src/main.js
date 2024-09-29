import { createApp } from 'vue';
import { BootstrapVue3 } from 'bootstrap-vue-3';

// Importar archivos CSS de Bootstrap y BootstrapVue (el orden es importante)
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue-3/dist/bootstrap-vue-3.css';

import App from './App.vue';
import { createRouter, createWebHistory } from "vue-router";
import HomepageView from './components/HomepageView.vue';
import AddAddressForm from "./components/AddAddressForm.vue";
import RegisterUserView from './components/RegisterUserView.vue';

// Configurar el enrutador
const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: "/", name: "Home", component: HomepageView },
        { path: "/AddressListView/AddAddressForm", name: "AddAddressForm", component: AddAddressForm, },
        { path: "/Registro", name: "Register User", component: RegisterUserView },
    ],
});

export const BackendAPIAddress = "https://localhost:7168/api/Shop";

// Crear la aplicación Vue
const app = createApp(App);

// Hacer BootstrapVue disponible en toda la aplicación
app.use(BootstrapVue3);

// Usar el enrutador
app.use(router);



// Montar la aplicación
app.mount("#app");