import { createApp } from 'vue'
import App from './App.vue'

import { createRouter, createWebHistory } from "vue-router";
import HomepageView from './components/HomepageView.vue';
import RegisterBusinessView from './components/RegisterBusinessView.vue';
import AddProductFrom from './components/AddProductFrom.vue';


const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: "/", name: "Home", component: HomepageView },
        { path: "/RegistrarEmprendimiento", name: "Register Business", component: RegisterBusinessView },
        { path: "/AddProductFrom", name: "AñadirProducto", component: AddProductFrom }
    ],
});

createApp(App).use(router).mount("#app");
