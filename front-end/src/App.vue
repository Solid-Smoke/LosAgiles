<template>
    <MainNavbar />
    <router-view></router-view>
</template>


<script>
    import MainNavbar from './components/MainNavbar.vue'

    export default {
        components: {
            MainNavbar,
        },
        data() {
            return {

            }
        },
    };
</script>

<style>
</style>