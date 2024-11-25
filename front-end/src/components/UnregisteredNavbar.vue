<template>
    <div>
        <b-navbar toggleable="lg" type="dark" variant="info">
            <b-navbar-brand href="/">Los Agiles</b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav class="ms-auto">
                    <b-nav-item><router-link to="/login" class="discreteLink">Iniciar sesi&oacute;n</router-link></b-nav-item>
                </b-navbar-nav>

                <b-navbar-nav>
                    <b-nav-item><router-link to="/registro" class="discreteLink">Registrarse</router-link></b-nav-item>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
</template>

<script>
    export default {
        data() {
            return {
            }
        },
    };
</script>

<style>
    .discreteLink {
        color: black;
        text-decoration: none;
    }

        .discreteLink:hover {
            color: black;
            text-decoration: none;
        }
</style>