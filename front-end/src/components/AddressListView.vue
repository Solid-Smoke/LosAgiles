<template>
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
        <b-button variant="success" style="float: right">Agregar dirección</b-button>
    </div>
</template>

<script>
import axios from 'axios';
import { BackendAPIAddress } from '@/main';
    export default {
        data() {
            return {
                addresses: [{province: "San José", canton: "Central", district: "El distrito", postalCode: "12343", otherSigns: "asdasfdf \n\nffsdf\n\ndsfsdfsdf\ndsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfdsfsdfsdfds      fsdfsdfdsfsdfsdf     dsfsdfsdfdsfsdfsdf dsfsdfsdfdsfs"},
                            {province: "San José", canton: "Central", district: "El distrito", postalCode: "12343", otherSigns: "Del palo de mango, 500 oeste, 400 sur, casa blanca"}]
            };
        },
        methods: {
            getUserAddressList() {
            axios.get(BackendAPIAddress + "/getUserAddressList/details?userId=" + this.userId.toString()).then(
                (response) => {
                    this.addreses = response.data;
                });
            }
        },
        props: {
            userId: Number
        }
    }
</script>

<style scoped>

</style>