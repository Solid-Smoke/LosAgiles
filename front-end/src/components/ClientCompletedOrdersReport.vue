<template>
    <div v-if="isClient">
        <MainNavbar />
        <h1 class="display-4 text-center mb-4">
            <strong>Reporte de Órdenes Completadas</strong>
        </h1>
        <div class="text-center">
            <label><strong>Fecha de inicio:</strong></label>
            <input type="date"
                   v-model="startDate"
                   class="form-control d-inline w-auto mx-2"
                   :max="endDate || ''"
                   @change="adjustStartDate" />

            <label><strong>Fecha de fin:</strong></label>
            <input type="date"
                   v-model="endDate"
                   class="form-control d-inline w-auto mx-2"
                   :min="startDate || ''"
                   @change="adjustEndDate" />

            <button class="btn btn-info mx-2"
                    :disabled="!isValidDates"
                    @click="generateCompletedOrdersReport">
                Generar Reporte
            </button>

            <button class="btn btn-success mx-2"
                    :disabled="!reportData.length"
                    @click="exportReportToPDF">
                Exportar a PDF
            </button>
        </div>

        <div v-if="reportData.length" class="table-responsive mt-4">
            <table class="table table-striped table-hover">
                <thead>
                    <tr>
                        <th>ID de Orden</th>
                        <th>Emprendimiento</th>
                        <th>Cantidad</th>
                        <th>Fecha de Creación</th>
                        <th>Fecha de Entrega</th>
                        <th>Fecha de Recepción</th>
                        <th>Subtotal</th>
                        <th>Costo de Envío</th>
                        <th>Total</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="order in reportData" :key="order.orderID">
                        <td>{{ order.orderID }}</td>
                        <td>{{ order.businessName || "General" }}</td>
                        <td>{{ order.amount }}</td>
                        <td>{{ formatDate(order.createdDate) }}</td>
                        <td>{{ formatDate(order.deliveryDate) }}</td>
                        <td>{{ formatDate(order.receivedDate) }}</td>
                        <td class="text-end">₡ {{ order.subtotalCost }}</td>
                        <td class="text-end">₡ {{ order.deliveryCost }}</td>
                        <td class="text-end">₡ {{ order.totalCost }}</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div v-else class="text-center mt-4">
            <h3>No se encontraron datos para el rango de fechas seleccionado.</h3>
        </div>
    </div>
    <div v-else>
        <h1 class="display-4 text-center mb-4"><strong>403 Forbidden</strong></h1>
        <h3 class="text-center mb-4">Página solo para clientes <br /><a href="/">Regresar</a></h3>
    </div>
</template>

<script>
    import axios from "axios";
    import MainNavbar from './MainNavbar.vue';
    import { BackendUrl } from '../main.js';

    export default {
        components: {
            MainNavbar,
        },
        data() {
            return {
                startDate: "",
                endDate: "",
                reportData: [],
            };
        },
        computed: {
            isValidDates() {
                return this.startDate && this.endDate && new Date(this.startDate) <= new Date(this.endDate);
            },
        },
        methods: {
            formatDate(date) {
                if (!date) return "-";
                return new Date(date).toLocaleDateString();
            },

            adjustStartDate() {
                if (this.startDate && this.endDate && new Date(this.startDate) > new Date(this.endDate)) {
                    this.endDate = "";
                }
            },

            adjustEndDate() {
                if (this.startDate && this.endDate && new Date(this.startDate) > new Date(this.endDate)) {
                    this.startDate = "";
                }
            },

            generateCompletedOrdersReport() {
                const user = JSON.parse(localStorage.getItem('user'));
                const id = Number(user[0].userID);
                axios
                    .get(`${BackendUrl}/Reports/CompletedOrders/${id}`, {
                        params: {
                            startDate: this.startDate,
                            endDate: this.endDate,
                        },
                    })
                    .then((response) => {
                        this.reportData = response.data;
                    })
                    .catch((error) => {
                        console.error("Error al generar el reporte:", error);
                        this.reportData = [];
                    });
            },

            exportReportToPDF() {
                const user = JSON.parse(localStorage.getItem('user'));
                const id = Number(user[0].userID);
                axios
                    .get(`${BackendUrl}/Reports/CompletedOrders/${id}/pdf`, {
                        params: {
                            startDate: this.startDate,
                            endDate: this.endDate,
                        },
                        responseType: "blob",
                    })
                    .then((response) => {
                        const url = window.URL.createObjectURL(new Blob([response.data], { type: "application/pdf" }));
                        const link = document.createElement("a");
                        link.href = url;
                        link.download = `CompletedOrdersReport(${this.startDate} - ${this.endDate}).pdf`;
                        document.body.appendChild(link);
                        link.click();
                        window.URL.revokeObjectURL(url);
                        document.body.removeChild(link);
                    })
                    .catch((error) => {
                        console.error("Error al exportar el reporte a PDF:", error);
                    });
            },
        },
        props: {
            isClient: {
                type: Boolean,
                required: true,
            },
        },
    };
</script>
