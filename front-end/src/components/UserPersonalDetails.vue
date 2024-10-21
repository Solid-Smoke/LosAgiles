<template>
    <b-modal v-model="personalDetailsModal" centered scrollable hide-footer title="Mis datos personales">
        <h1>Usuario: {{ UserData.userName }}</h1>
        <h2>Nombre completo: {{ UserData.name }} {{ UserData.lastNames }}</h2>
        <h3>Correo registrado: {{ UserData.email }}</h3>
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