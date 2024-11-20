<template>
<MainNavbar />
<div class="container mt-4">
    <div class="banner mt-4 p-4 text-white text-center">
    <h2>Bienvenido, {{ businessName }}</h2>
    <p>¡Explora tus métricas y gestiona tu negocio eficientemente!</p>
    </div>

    <b-row class="mt-4">
    <b-col lg="6" md="6" class="mb-4">
        <b-card class="text-center custom-summary-card">
        <b-card-title>Total de Órdenes</b-card-title>
        <h3>{{ ordersInProgress.length }}</h3>
        </b-card>
    </b-col>
    <b-col lg="6" md="6" class="mb-4">
        <b-card class="text-center custom-summary-card">
        <b-card-title>Ganancias Totales</b-card-title>
        <h3>&#x20a1;{{ totalRevenue }}</h3>
        </b-card>
    </b-col>
    </b-row>

    <b-row class="mt-4">
    <b-col lg="6" md="6" class="mb-4">
        <b-card class="custom-card">
        <b-card-title>Ganancias por Mes</b-card-title>
        <canvas id="monthlyRevenueChart"></canvas>
        </b-card>
    </b-col>

    <b-col lg="6" md="6" class="mb-4">
        <b-card class="custom-card overflow-y: scroll;">
        <b-card-title>Órdenes en Progreso</b-card-title>
        <b-list-group>
            <b-list-group-item v-if="ordersInProgress.length === 0">No hay órdenes en progreso.</b-list-group-item>
            <b-list-group-item v-if="ordersInProgress.length !== 0">
                <div class="d-flex justify-content-between">
                <span style="width: 30%"><strong>Orden</strong></span>
                <span style="width: 40%"><strong>Estado</strong></span>
                <span style="width: 30%"><strong>Total</strong></span>
                </div>
            </b-list-group-item>
            <b-list-group-item v-for="order in ordersInProgress" :key="order.orderID">
                <div class="d-flex justify-content-between" style="width: 100%;">
                <span style="width: 30%">#{{ order.orderID }}</span>
                <span style="width: 40%">{{ order.status }}</span>
                <span style="width: 30%">&#x20a1;{{order.totalAmount }}</span>
                </div>
            </b-list-group-item>
            </b-list-group>
        </b-card>
    </b-col>
    </b-row>
</div>
</template>

<script>
import { BackendUrl } from "@/main";
import MainNavbar from "./MainNavbar.vue";
import axios from "axios";
import { Chart } from "chart.js/auto";

export default {
components: {
    MainNavbar,
},
data() {
    return {
        businessName: "",
        revenueData: [],
        ordersInProgress: [],
        totalRevenue: 0,
        months: [
            "Enero",
            "Febrero",
            "Marzo",
            "Abril",
            "Mayo",
            "Junio",
            "Julio",
            "Agosto",
            "Septiembre",
            "Octubre",
            "Noviembre",
            "Diciembre",
        ],
    };
},
methods: {
    fetchBusinessData(businessID) {
    axios
        .get(`${BackendUrl}/Business/${businessID}`)
        .then((response) => {
            this.businessName = response.data.name || "Nombre no disponible";
        })
        .catch((error) => {
            console.error("Error al obtener datos del negocio:", error);
            this.businessName = "Error al cargar el nombre del negocio";
        });
    },
},
mounted() {
    const businessID = this.$route.params.businessID;
    if (businessID) {
        this.fetchBusinessData(businessID);
    } else {
        console.error("No se proporcionó un ID de negocio válido.");
    }
},
};
</script>

<style scoped>
h1 {
    color: #007bff;
}
.banner {
    background-color: #007bff;
    border-radius: 10px;
}
.custom-card {
    min-height: 400px;
}
.custom-summary-card {
    min-height: 150px;
    background-color: #f9f9f9;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
