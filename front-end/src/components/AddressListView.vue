<template>
    <MainNavbar/>
    <div class="container mt-5">
        <h1 class="display-4 text-center">Direcciones registradas</h1>
        <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth table-striped">
            <thead>
                <tr>
                    <th>Provincia</th>
                    <th>Cantón</th>
                    <th>Distrito</th>
                    <th>Código Postal</th>
                    <th>Otras señas</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(address, index) of addresses" :key="index">
                    <td>{{ address.province }}</td>
                    <td>{{ address.canton }}</td>
                    <td>{{ address.district }}</td>
                    <td>{{ address.postalCode }}</td>
                    <td><p style="word-wrap: break-word; max-width: 400px">{{ address.otherSigns }}</p></td>
                </tr>
            </tbody>
        </table>
        <b-button @click="this.openAddAddressForm()" variant="success" style="float: right">Agregar dirección</b-button>
        <AddAddressForm ref="addAddressForm" :userId="this.userId" />
    </div>
</template>

<script>
import axios from 'axios';
import { BackendAPIAddress } from '@/main';
import AddAddressForm from './AddAddressForm.vue';
import MainNavbar from './MainNavbar.vue';
    export default {
        components: {
            AddAddressForm,
            MainNavbar
        },
        data() {
            return {
                addAddressModal: false,
                addresses: [],
                userId: -1,
            };
        },
        methods: {
            getUserAddressList() {
                axios
                .get(BackendAPIAddress +
                    "/getAllClientAddresses/details?userId=" +
                    this.userId)
                .then(
                    (response) => {
                        this.addresses = response.data;
                    })
                .catch(function (error) {
                    console.log(error);
                });
            },
            openAddAddressForm() {
                this.$refs.addAddressForm.openModal();
            },
            getUserId() {
                const user = JSON.parse(localStorage.getItem('user'));
                return Number(user[0].userID);
            }
        },
        created() {
            this.userId = this.getUserId();
            this.getUserAddressList();
        }
    }
</script>

<style scoped>

</style>