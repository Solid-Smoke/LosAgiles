<template>
    <b-modal v-model="personalDetailsModal" centered scrollable hide-footer title="Mis datos personales">
        <template v-if="userMessage">
            <h3>{{ userMessage }}</h3>
        </template>
        <template v-if="!UserData.email">
            <h3><em>Admin:</em> {{ UserData.userName }}</h3>
        </template>
        <template v-else>
            <h4><em>Usuario:</em> {{ UserData.userName }}</h4>
            <h4><em>Nombre completo:</em> {{ UserData.name }} {{ UserData.lastNames }}</h4>
            <h4><em>Correo registrado:</em> {{ UserData.email }}</h4>
        </template>
    </b-modal>
</template>

<script>
    export default {
        data() {
            return {
                personalDetailsModal: false,
                UserData: {
                    userName: null,
                    name: null,
                    lastNames: null,
                    email: null,
                },
                userMessage: "",
            };
        },
        methods: {
            getUserData() {
                const user = JSON.parse(localStorage.getItem('user'));
                if (user && user.length > 0) {
                    this.UserData.userName = user[0].userName;
                    this.UserData.name = user[0].name;
                    this.UserData.lastNames = user[0].lastNames;
                    this.UserData.email = user[0].email;
                    this.userMessage = "";
                } else {
                    this.userMessage = "Aún no ha iniciado sesión.";
                }
            },
            openModal() {
                this.getUserData();
                this.personalDetailsModal = true;
            },
            closeModal() {
                this.personalDetailsModal = false;
            },
        },
    }
</script>

<style>
    .form-group {
        margin-bottom: 20px;
    }
</style>