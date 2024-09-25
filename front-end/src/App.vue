<template>
    <router-view></router-view>
    <MainNavbar />
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
</style>