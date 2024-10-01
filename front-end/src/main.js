import { createApp } from 'vue';
import { BootstrapVue3 } from 'bootstrap-vue-3';

// Importar archivos CSS de Bootstrap y BootstrapVue (el orden es importante)
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue-3/dist/bootstrap-vue-3.css';

import App from './App.vue';
import { createRouter, createWebHistory } from "vue-router";
import HomepageView from './components/HomepageView.vue';
import RegisterUserView from './components/RegisterUserView.vue';
import LoginUserView from './components/LoginUserView.vue';
import LoginAdminView from './components/LoginAdminView.vue';
import UsersDataView from './components/UsersDataView.vue';
import AdminUserList from './components/AdminUserList.vue';
import LoginSuperUser from './components/LoginSuperUser.vue';
import MyBusinessView from './components/MyBusinessView.vue';
import MyBusinessInventoryView from './components/MyBusinessInventoryView.vue';

// Configurar el enrutador
const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: "/", name: "Home", component: HomepageView },
        { path: "/Registro", name: "Register User", component: RegisterUserView },
        { path: "/Login", name: "Login", component: LoginUserView },
        { path: "/AdminLogin", name: "adminLogin", component: LoginAdminView },
        { path: "/usersData", name: "usersData", component: UsersDataView},
        { path: "/UserList", name: "userList", component: AdminUserList },
        { path: "/sudo", name: "SuperUserLogin", component: LoginSuperUser},
        { path: "/MyBusiness", name: "userBusiness", component: MyBusinessView },
        { path: "/MyBusinessInventory", name: "userBusinessInventory", component: MyBusinessInventoryView },
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