<template>
    <MainNavbar />

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
            <b-col cols="2">{{ user.name }}</b-col>
            <b-col cols="3">{{ user.email }}</b-col>
            <b-col cols="2">{{ user.role }}</b-col>
            <b-col cols="2">{{ user.state }}</b-col>
            <b-col cols="3">
                <b-button variant="success" size="sm" class="mr-2" style="margin-right: 20px;">Activar</b-button>
                <b-button variant="primary" size="sm" class="mr-2" style="margin-right: 20px;">Desactivar</b-button>
                <b-button variant="danger" size="sm" style="margin-right: 20px;">Bloquear</b-button>
            </b-col>
        </b-row>
    </div>
</template>

<script scoped>
    import MainNavbar from './MainNavbar.vue';
    
    export default {
        components: {
            MainNavbar
        },

        name: "UserList",
        data() {
            return {
                users: [
                    { name: "Maria", email: "abc@gmail.com", role: "Comprador", state: "Activo" },
                    { name: "Jose", email: "def@gmail.com", role: "Emprendedor", state: "Activo" },
                    { name: "Juan", email: "ghi@gmail.com", role: "Comprador", state: "Inactivo" },
                    { name: "Antonio", email: "jkl@gmail.com", role: "Comprador", state: "Bloqueado" },
                    { name: "David", email: "mno@gmail.com", role: "Emprendedor", state: "Activo" }
                ],
            };
        },
    }
</script>

<style>
</style>