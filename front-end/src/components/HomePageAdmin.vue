<template>
  <AdminNavbar/>
  
  <b-container fluid class="px-4">
      <b-row>
          <b-col lg="6" md="6" sm="12" class="mb-4">
              <b-card style="margin-top: 20px;">
                  <b-card-title>Ganancias por Mes</b-card-title>
                  <canvas id="gananciasChart"></canvas>
              </b-card>
          </b-col>

          <b-col lg="6" md="6" sm="12" class="mb-4">
              <b-card style="margin-top: 20px;">
                  <b-card-title>Gasto en Envíos</b-card-title>
                  <canvas id="gastoEnviosChart"></canvas>
              </b-card>
          </b-col>

          <b-col lg="12" md="12" sm="12" class="mb-4">
              <b-card>
                  <b-card-title>Últimas 10 Órdenes más Recientes</b-card-title>
                  <b-list-group>
                      <b-list-group-item v-if="ordersInProgress.length === 0">No hay productos comprados recientemente.</b-list-group-item>
                      <b-list-group-item v-for="(order, index) in ordersInProgress" :key="index">
                        <div class="d-flex justify-content-between" style="width: 100%;">
                          <span  style="width: 35%"><strong>Orden #</strong>{{ order.orderID }}</span>
                          <span  style="width: 35%"><strong>Estado:</strong> {{ order.status }}</span>
                          <span  style="width: 35%"><strong>Total:</strong> &#x20a1;{{ formatPrice(order.totalAmount) }}</span>
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
    getGanancias() {
      axios
        .get(`${BackendUrl}/admin/Monthly/Revenue`)
        .then((response) => {
          this.gananciasData = response.data;
          this.renderGananciasChart();
        })
        .catch((error) => {
          console.error("Error obteniendo ganancias:", error);
        });
    },
    renderGananciasChart() {
        const gananciasMonths = this.months.map((month, index) => {
            const data = this.gananciasData.find((d) => parseInt(d.month) - 1 === index);
            return data ? data.total : 0;
        });

        const ctx = document.getElementById("gananciasChart").getContext('2d');

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
    formatPrice(price) {
      return new Intl.NumberFormat("en-US").format(price);
    },
  },
  mounted() {
    this.getGanancias();
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
</style>
