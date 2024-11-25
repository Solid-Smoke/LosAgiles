<template>
    <div v-if="isClient">
        <MainNavbar />
        <h1 class="display-4 text-center mb-4">
            <strong>Reporte de Órdenes Completadas</strong>
        </h1>
        <div class="text-center">
            <label><strong>Periodo de reporte:</strong></label>
            <input v-model="startDate"
                   type="date"
                   name="startDate-selector"
                   id="startDate-selector"
                   @change="clearEndDate" /> -
            <input v-model="endDate"
                   type="date"
                   name="endDate-selector"
                   id="endDate-selector"
                   :min="startDate"
                   :disabled="!startDate" />
            <button class="btn btn-info mx-2"
                    :disabled="!(startDate && endDate)"
                    @click="generateCompletedOrdersReport">
                Generar Reporte
            </button>
            <button class="btn btn-success mx-2"
                    :disabled="!isValidReport"
                    @click="exportReportToPDF">
                Exportar a PDF
            </button>
        </div>

        <div v-if="isValidReport" class="table-responsive mt-4">
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
                        <td>{{ order.createdDate }}</td>
                        <td>{{ order.deliveryDate }}</td>
                        <td>{{ order.receivedDate }}</td>
                        <td class="text-end">₡ {{ formatPrice(order.subtotalCost) }}</td>
                        <td class="text-end">₡ {{ formatPrice(order.deliveryCost) }}</td>
                        <td class="text-end">₡ {{ formatPrice(order.totalCost) }}</td>
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
                startDate: null,
                endDate: null,
                reportData: [],
                isValidReport: false,
            };
        },
        methods: {
            clearEndDate() {
                if (this.startDate) {
                    this.endDate = '';
                }
            },
            formatPrice(price) {
                return new Intl.NumberFormat("en-US").format(price);
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
                        this.isValidReport = !!this.reportData.length;
                    })
                    .catch((error) => {
                        console.error("Error al generar el reporte:", error);
                        this.reportData = [];
                        this.isValidReport = false;
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
