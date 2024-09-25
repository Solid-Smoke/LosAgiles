<template>
    <MainNavbar />
    <router-view></router-view>
</template>


<script>
    import MainNavbar from './components/MainNavbar.vue';

    export default {
        components: {
            MainNavbar
        },
    };
</script>

<style>
    .discreteLink {
        color:black;
        text-decoration: none;
    }

    .discreteLink:hover {
        color: black;
        text-decoration: none;
    }
</style>