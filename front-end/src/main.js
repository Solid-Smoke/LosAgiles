import { createApp } from 'vue';
import { BootstrapVue3 } from 'bootstrap-vue-3';

// Import CSS files from Bootstrap and BootstrapVue (The order is important)
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue-3/dist/bootstrap-vue-3.css';

import App from './App.vue';
import { createRouter, createWebHistory } from "vue-router";
import HomepageView from './components/HomepageView.vue';
import AddAddressForm from "./components/AddAddressForm.vue";
import RegisterUserView from './components/RegisterUserView.vue';
import LoginUserView from './components/LoginUserView.vue';
import LoginAdminView from './components/LoginAdminView.vue';
import UsersDataView from './components/UsersDataView.vue';
import AddressListView from './components/AddressListView.vue';
import AdminUserList from './components/AdminUserList.vue';
import LoginSuperUser from './components/LoginSuperUser.vue';
import MyBusinessView from './components/MyBusinessView.vue';
import MyBusinessInventoryView from './components/MyBusinessInventoryView.vue';
import AdminViewAllBusiness from './components/AdminViewAllBusiness.vue';
import CartView from './components/CartView.vue';
import MetodoPago from './components/MetodoPago.vue';
import OrderConfirmation from './components/OrderConfirmation.vue';
import MyOrders from './components/MyOrders.vue';

export const BackendUrl = "https://localhost:7168/api";
export const LosAgilesMapsApiKey = "AIzaSyAJrdPkYVkZfamzm2DvyuvHyaJDAd0rOP4";

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: "/", name: "Home", component: HomepageView },
        { path: "/AddressListView/AddAddressForm", name: "AddAddressForm", component: AddAddressForm, },
        { path: "/Registro", name: "Register User", component: RegisterUserView },
        { path: "/Login", name: "Login", component: LoginUserView },
        { path: "/AdminLogin", name: "adminLogin", component: LoginAdminView },
        { path: "/usersData", name: "usersData", component: UsersDataView},
        { path: "/direcciones", name: "AddressListView", component: AddressListView },
        { path: "/UserList", name: "userList", component: AdminUserList },
        { path: "/sudo", name: "SuperUserLogin", component: LoginSuperUser},
        { path: "/MyBusiness", name: "userBusiness", component: MyBusinessView },
        { path: "/MyBusinessInventory", name: "userBusinessInventory", component: MyBusinessInventoryView },
        { path: "/AdminViewAllBusiness", name: "adminViewAllBusiness", component: AdminViewAllBusiness },
        { path: "/VerCarrito", name: "cartView.vue", component: CartView },
        {  path: "/MetodoPago", name: "MetodoPago", component: MetodoPago},
        { path: "/ConfirmarOrdenes", name: "Confirmacion de ordenes", component: OrderConfirmation },
        { path: "/MisOrdenes", name: "Mis ordenes", component: MyOrders },
    ],
});

export const BackendAPIAddress = "https://localhost:7168/api/Shop";
const app = createApp(App);

// Make BootstrapVue application-wide
app.use(BootstrapVue3);

app.use(router);

app.mount("#app");