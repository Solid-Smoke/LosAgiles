<template>
    <template v-if="isAdmin">
        <AdminNavbar />
        <div class="container mt-5">
            <h1 class="display-4 text-center mb-4"><strong>LISTA DE USUARIOS</strong></h1>

            <b-row class="font-weight-bold text-center mb-2 border-bottom border-2 border-dark">
                <b-col cols="2" style="padding: 15px;">USUARIO</b-col>
                <b-col cols="3" style="padding: 15px;">CORREO</b-col>
                <b-col cols="2" style="padding: 15px;">ROL</b-col>
                <b-col cols="2" style="padding: 15px;">ESTADO CUENTA</b-col>
                <b-col cols="3" style="padding: 15px;">ACCIONES</b-col>
            </b-row>

            <b-row v-for="(user, index) in users" :key="index" class="text-center align-items-center mb-2 border-bottom py-2">
                <b-col cols="2">{{ user.userName }}</b-col>
                <b-col cols="3">{{ user.email }}</b-col>
                <b-col cols="2">{{ user.rol }}</b-col>
                <b-col cols="2">{{ user.accountState }}</b-col>
                <b-col cols="3">
                    <b-button variant="success" size="sm" class="mr-2" style="margin-right: 20px;">Activar</b-button>
                    <b-button variant="primary" size="sm" class="mr-2" style="margin-right: 20px;">Desactivar</b-button>
                    <b-button variant="danger" size="sm">Bloquear</b-button>
                </b-col>
            </b-row>
        </div>
    </template>

    <template v-else>
        <h1 class="display-4 text-center mb-4"><strong>403 Forbidden</strong></h1>
        <h3 class="text-center mb-4">Página solo administradores <br /><a href="/">Regresar</a></h3>
    </template>
</template>

<script>
    import AdminNavbar from './AdminNavbar.vue';
    import { BackendUrl } from '../main.js';
    import axios from 'axios';
    
    export default {
        components: {
            AdminNavbar
        },

        name: "UserList",
        data() {
            return {
                users: [],
            };
        },

        methods: {
            getData() {
                axios.get(`${BackendUrl}/Clients`).then((response) => {
                    this.users = response.data;
                });
            },
        },

        created: function () {
            this.getData();
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

<style>
</style>