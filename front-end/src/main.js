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
import AdminUserList from './components/AdminUserList.vue';
import MyBusinessView from './components/MyBusinessView.vue';

// Configurar el enrutador
const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: "/", name: "Home", component: HomepageView },
        { path: "/Registro", name: "Register User", component: RegisterUserView },
        { path: "/Login", name: "Login", component: LoginUserView },
        { path: "/AdminLogin", name: "adminLogin", component: LoginAdminView },
        { path: "/UserList", name: "userList", component: AdminUserList },
        { path: "/MyBusiness", name: "userBusiness", component: MyBusinessView },
    ],
});

// Crear la aplicación Vue
const app = createApp(App);

// Hacer BootstrapVue disponible en toda la aplicación
app.use(BootstrapVue3);

// Usar el enrutador
app.use(router);



// Montar la aplicación
app.mount("#app");