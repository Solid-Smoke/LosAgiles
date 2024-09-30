<template>
   
   <b-container fluid="lg">
        <h1 class="display-4 text-center mb-4"><strong>LISTA DE USUARIOS</strong></h1>

        <b-row class="font-weight-bold text-center border-bottom border-2 border-dark">
            <b-col>USUARIO</b-col>
            <b-col>CORREO</b-col>
            <b-col>ESTADO CUENTA</b-col>
            <b-col>NOMBRE</b-col>
            <b-col>APELLIDOS</b-col>
            <b-col>ID EMPRESA</b-col>
            <b-col>ACCIONES</b-col>
        </b-row>

        <b-row v-for="(user, index) in users" :key="index" class="text-center align-items-center mb-2 border-bottom py-2">
            <b-col>{{ user.userId }}</b-col>
            <b-col>{{ user.userName }}</b-col>
            <b-col>{{ user.email }}</b-col>
            <b-col>{{ user.state }}</b-col>
            <b-col>{{ user.name }}</b-col>
            <b-col>{{ user.lastNames }}</b-col>
            <b-col>{{ user.businessId }}</b-col>
            <b-col>
                <b-button variant="success" size="sm" class="mr-2" style="margin-right: 20px;">Activar</b-button>
                <b-button variant="primary" size="sm" class="mr-2" style="margin-right: 20px;">Desactivar</b-button>
                <b-button variant="danger" size="sm" style="margin-right: 20px;">Eliminar</b-button>
            </b-col>
        </b-row>
        <b-button-group style="float: right">
            <span>{{ actualPage }}/{{ Math.trunc(userCount / pageMaxRows) + 1 }}</span>
            <b-button @click="actualPage = 1">Inicio</b-button>
            <b-button @click="goPrevious">Anterior</b-button>
            <b-button @click="goNext">Siguiente</b-button>
        </b-button-group>
   </b-container>
</template>

<script>
import axios from 'axios';
import { BackendAPIAddress } from '@/main';
    export default {
        data() {
            return {
                users: [],
                userCount: 1, // 1 for default display, will be overwritten
                actualPage: 1,
                pageMaxRows: 50,
            }
        },
        methods: {
            getUsersData(pageNumber) {
                axios // Calculation below is for an offset for pagination
                    .get(BackendAPIAddress + "/getAllUsersData/details?offset="
                        + ((pageNumber - 1) * this.pageMaxRows).toString()
                        + "&maxRows=" + this.pageMaxRows.toString())
                    .then(
                        (response) => {
                            this.users = response.data;
                        });
            },
            getUserCount() {
                axios // Calculation below is for an offset for pagination
                    .get(BackendAPIAddress + "/getUserCount")
                    .then(
                        (response) => {
                            this.userCount = response.data;
                        });
            },
            goPrevious() {
                if(this.actualPage > 1) {
                    this.actualPage -= 1;
                }
            },
            goNext() {
                if(this.actualPage < (Math.trunc(this.userCount / this.pageMaxRows) + 1)) {
                    this.actualPage += 1;
                }
            }
        },
        created() {
            this.getUsersData(this.actualPage);
            this.getUserCount();
        }
    }
</script>

<style scoped>

</style>