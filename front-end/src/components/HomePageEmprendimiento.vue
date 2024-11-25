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
        <b-card-title>Total de Órdenes en Progreso</b-card-title>
        <h3>{{ ordersInProgress.length }}</h3>
        </b-card>
    </b-col>
    <b-col lg="6" md="6" class="mb-4">
        <b-card class="text-center custom-summary-card">
        <b-card-title>Ganancias Totales</b-card-title>
        <h3 style="color: green;">&#x20a1;{{ formatPrice(totalRevenue) }}</h3>
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
                <span style="width: 40%; text-align: center;"><strong>Estado</strong></span>
                <span style="width: 30%; text-align: right;"><strong>Total</strong></span>
                </div>
            </b-list-group-item>
            <b-list-group-item v-for="order in ordersInProgress" :key="order.orderID">
                <div class="d-flex justify-content-between" style="width: 100%;">
                <span style="width: 30%">#{{ order.orderID }}</span>
                <span style="width: 40%; text-align: center;">{{ order.status }}</span>
                <span style="width: 30%; text-align: right;">&#x20a1;{{ formatPrice(order.totalAmount) }}</span>
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
    getMonthlyRevenue(businessID) {
    axios
        .get(`${BackendUrl}/Business/${businessID}/MonthlyRevenue`)
        .then((response) => {
        this.revenueData = response.data;
        this.totalRevenue = response.data.reduce(
            (acc, item) => acc + item.total,
            0
        );
        this.renderRevenueChart();
        })
        .catch((error) => {
        console.error("Error al obtener las ganancias mensuales:", error);
        });
    },
    getOrdersInProgress(businessID) {
    axios
        .get(`${BackendUrl}/Business/${businessID}/OrdersInProgress`)
        .then((response) => {
        this.ordersInProgress = response.data;
        })
        .catch((error) => {
        console.error("Error al obtener las órdenes en progreso:", error);
        });
    },
    renderRevenueChart() {
    const allMonths = this.months.map((month, index) => ({
        month: index + 1,
        total: 0,
    }));
    this.revenueData.forEach((data) => {
        const monthIndex = allMonths.findIndex(
        (month) => month.month === parseInt(data.month)
        );
        if (monthIndex !== -1) {
        allMonths[monthIndex].total = data.total;
        }
    });
    const ctx = document
        .getElementById("monthlyRevenueChart")
        .getContext("2d");
    new Chart(ctx, {
        type: "bar",
        data: {
        labels: allMonths.map((data) => this.months[data.month - 1]),
        datasets: [
            {
            label: "",
            data: allMonths.map((data) => data.total),
            backgroundColor: [
                "#FF6384",
                "#36A2EB",
                "#FFCE56",
                "#4BC0C0",
                "#9966FF",
                "#FF9F40",
                "#E7E9ED",
                "#76A346",
                "#D35400",
                "#8E44AD",
                "#3498DB",
                "#2ECC71",
            ],
            },
        ],
        },
        options: {
        responsive: true,
        maintainAspectRatio: true,
        plugins: {
            legend: {
            display: false,
            },
        },
        scales: {
            y: {
            beginAtZero: true,
            },
        },
        },
    });
    },
    formatPrice(price) {
    return new Intl.NumberFormat("en-US").format(price);
    },
},
mounted() {
    const businessID = this.$route.params.businessID;
    if (businessID) {
        this.fetchBusinessData(businessID);
        this.getMonthlyRevenue(businessID);
        this.getOrdersInProgress(businessID);
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
    min-height: 125px;
    background-color: #f9f9f9;
    border-radius: 10px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
