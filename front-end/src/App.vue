<template>
    <router-view :isAdmin="isAdmin" :isClient="isClient"></router-view>
</template>


<script>
    export default {
        data() {
            return {
                adminName: "",
                adminEmail: "",
                isAdmin: false,
                isClient: false,
            };
        },
        methods: {
            getAdminData() {
                const user = JSON.parse(localStorage.getItem('user'));
                if (user && user.length > 0) {
                    this.adminName = user[0].userName;
                    this.adminEmail = user[0].email;
                    this.isAdmin = this.checkIfAdmin();
                    this.isClient = this.checkIfClient();
                }
            },
            checkIfAdmin() {
                if (this.adminName && !this.adminEmail) {
                    return true;
                }
                return false;
            },
            checkIfClient() {
                if (this.adminName && this.adminEmail) {
                    return true;
                }
                return false;
            }
        },
        created() {
            this.getAdminData();
        },
        watch: {
            '$route'() {
                this.getAdminData();
            }
        }
    };
</script>

<style>
</style>