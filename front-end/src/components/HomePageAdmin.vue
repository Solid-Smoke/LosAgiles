<template>
  <AdminNavbar/>
  
  <b-container fluid class="px-4">
      <b-row class="mt-4">
          <b-col lg="6" md="6" sm="12" class="mb-4">
              <b-card class="text-center custom-summary-card">
                  <b-card-title>Ganancias Totales</b-card-title>
                  <h3 style="color: green;">&#x20a1;{{ formatPrice(totalRevenue) }}</h3>
              </b-card>
          </b-col>
          <b-col lg="6" md="6" sm="12" class="mb-4">
              <b-card class="text-center custom-summary-card">
                  <b-card-title>Gastos de Envío Totales</b-card-title>
                  <h3 style="color: red;">&#x20a1;-{{ formatPrice(totalShippingExpenses) }}</h3>
              </b-card>
          </b-col>
      </b-row>

      <b-row>
          <b-col lg="6" md="6" sm="12" class="mb-4">
              <b-card style="margin-top: 20px;">
                  <b-card-title>Ganancias por Mes</b-card-title>
                  <canvas id="revenueChart"></canvas>
              </b-card>
          </b-col>

          <b-col lg="6" md="6" sm="12" class="mb-4">
              <b-card style="margin-top: 20px;">
                  <b-card-title>Gasto en Envíos</b-card-title>
                  <canvas id="shippingExpensesChart"></canvas>
              </b-card>
          </b-col>

          <b-col lg="12" md="12" sm="12" class="mb-4">
              <b-card>
                  <b-card-title>Últimas 10 Órdenes más Recientes</b-card-title>
                  <b-list-group>
                      <b-list-group-item v-if="ordersInProgress.length === 0">No hay productos comprados recientemente.</b-list-group-item>
                      <b-list-group-item v-if="ordersInProgress.length !== 0">
                        <div class="d-flex justify-content-between">
                          <span style="width: 30%"><strong>Orden</strong></span>
                          <span style="width: 40%"><strong>Estado</strong></span>
                          <span style="width: 30%"><strong>Total</strong></span>
                        </div>
                      </b-list-group-item>
                      <b-list-group-item v-for="(order, index) in ordersInProgress" :key="index">
                        <div class="d-flex justify-content-between" style="width: 100%;">
                          <span  style="width: 30%">#{{ order.orderID }}</span>
                          <span  style="width: 40%">{{ order.status }}</span>
                          <span  style="width: 30%">&#x20a1;{{ formatPrice(order.totalAmount) }}</span>
                        </div>
                      </b-list-group-item>
                  </b-list-group>
              </b-card>
          </b-col>
      </b-row>
  </b-container>
</template>

<script>
import { BackendUrl } from "@/main";
import AdminNavbar from "./AdminNavbar.vue";
import axios from "axios";
import { Chart } from 'chart.js/auto'; 

export default {
  components: {
    AdminNavbar,
  },
  data() {
    return {
      gananciasData: [],
      gastosData: [],
      ordersInProgress: [],
      totalRevenue: 0,
      totalShippingExpenses: 0,
      months: [
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", 
        "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
      ],
      barColors: [
        "#42A5F5", "#66BB6A", "#FF7043", "#AB47BC", "#FFEB3B", "#FF4081", 
        "#26C6DA", "#8D6E63", "#FF5252", "#5C6BC0", "#26A69A", "#FFB74D"
      ],
    };
  },
  methods: {
    getRevenue() {
      axios
        .get(`${BackendUrl}/admin/Monthly/Revenue`)
        .then((response) => {
          this.gananciasData = response.data;
          this.totalRevenue = response.data.reduce((acc, item) => acc + item.total, 0);
          this.renderRevenueChart();
        })
        .catch((error) => {
          console.error("Error obteniendo ganancias:", error);
        });
    },
    getShippingExpenses() {
      axios
        .get(`${BackendUrl}/admin/Shipping/Expenses`)
        .then((response) => {
          this.gastosData = response.data;
          this.totalShippingExpenses = response.data.reduce((acc, item) => acc + item.total, 0);
          this.renderShippingExpensesChart();
        })
        .catch((error) => {
          console.error("Error obteniendo gastos de envíos:", error);
        });
    },
    getOrdersInProgress() {
      axios
        .get(`${BackendUrl}/admin/Orders/In/Progress`)
        .then((response) => {
          this.ordersInProgress = response.data;
        })
        .catch((error) => {
          console.error("Error obteniendo órdenes en progreso:", error);
        });
    },
    renderRevenueChart() {
      const gananciasMonths = this.months.map((month, index) => {
        const data = this.gananciasData.find((d) => parseInt(d.month) - 1 === index);
        return data ? data.total : 0;
      });

      const ctx = document.getElementById("revenueChart").getContext('2d');

      new Chart(ctx, {
          type: 'bar',
          data: {
              labels: this.months,
              datasets: [{
                  data: gananciasMonths,
                  backgroundColor: this.barColors,
                  borderColor: "#42A5F5",
                  fill: true,
              }],
          },
          options: {
              responsive: true,
              maintainAspectRatio: true,
              scales: {
                  y: {
                      beginAtZero: true,
                  }
              },
              plugins: {
                  legend: {
                      display: false
                  }
              }
          },
      });
    },
    renderShippingExpensesChart() {
      const gastoEnviosMonths = this.months.map((month, index) => {
          const data = this.gastosData.find((d) => parseInt(d.month) - 1 === index);
          return data ? data.total : 0; 
      });

      const ctx = document.getElementById("shippingExpensesChart").getContext('2d');

      new Chart(ctx, {
          type: 'bar',
          data: {
              labels: this.months,
              datasets: [{
                  data: gastoEnviosMonths,
                  backgroundColor: this.barColors,
                  borderColor: "#FF7043",
                  fill: true,
              }],
          },
          options: {
              responsive: true,
              maintainAspectRatio: true,
              scales: {
                  y: {
                      beginAtZero: true,
                  }
              },
              plugins: {
                  legend: {
                      display: false
                  }
              }
          },
      });
    },
    formatPrice(price) {
      return new Intl.NumberFormat("en-US").format(price);
    },
  },
  mounted() {
    this.getRevenue();
    this.getShippingExpenses();
    this.getOrdersInProgress();
  },
};
</script>

<style scoped>
.card {
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
}

.card-title {
  font-size: 1.25rem;
  margin-bottom: 1rem;
}

canvas {
  height: 250px;
  width: 100%;
}

b-list-group-item {
  font-size: 0.875rem;
}

.custom-summary-card {
  min-height: 150px;
  background-color: #f9f9f9;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
