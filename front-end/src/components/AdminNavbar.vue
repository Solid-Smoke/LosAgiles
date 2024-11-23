<template>
    <div>
        <b-navbar toggleable="lg" type="dark" variant="info">
            <b-navbar-brand href="/">Los Agiles</b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav>
                    <b-nav-item href="/AdminViewAllBusiness">Ver todas las empresas registradas</b-nav-item>
                    <b-nav-item href="/UserList">Ver todos los usuarios</b-nav-item>
                    <b-nav-item href="/confirmarOrdenes">Revisar ordenes entrantes</b-nav-item>

                    <b-nav-item-dropdown text="Reportes">
                        <b-dropdown-item href="/AdminReports/Cancelled">Canceladas</b-dropdown-item>
                    </b-nav-item-dropdown>
                </b-navbar-nav>

                <b-navbar-nav class="ms-auto">
                    <b-nav-item><strong>{{this.userName}}</strong></b-nav-item>
                    <b-nav-item @click="logout">Cerrar sesi&oacute;n</b-nav-item>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                userName: '',
            }
        },
        methods: {
            logout() {
                localStorage.removeItem('user');
                window.location.href = "/";
            },
            getUserName() {
                const user = JSON.parse(localStorage.getItem('user'));
                return user[0].userName;
            }
        },
        created() {
            this.userName = this.getUserName();
        }
    };
</script>

<style>
</style>