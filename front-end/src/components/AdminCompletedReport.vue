<template>
    <template v-if="isAdmin">
        <AdminNavbar />
        <h1 class="display-4 text-center mb-4"><strong>Generador de reportes de ordenes completadas</strong></h1>
        <div>
            <div class="text-center">
                <label><strong>Periodo de reporte:</strong></label> <span />
                <input v-model="startDate" type="date" name="startDate-selector" id="startDate-selector" @change="clearEndDate"> -
                <input v-model="endDate" type="date" name="endDate-selector" :min="startDate" id="endDate-selector" :disabled="!startDate">
                <span class="ms-2"/>

                <button v-on:click="generateReport" class="btn btn-info" :disabled="!(startDate && endDate)">
                    Generar reporte
                </button> <span class="ms-3"/>

                <button v-on:click="openWarningPDFModal" class="btn btn-success" :disabled="!isValidReport">
                    Exportar a PDF
                </button>
            </div>
        </div>
        <template v-if="isValidReport">
            <div class="table-responsive-sm">
                <table class="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th scope="col">Número de orden</th>
                            <th scope="col">ID de cliente</th>
                            <th scope="col">Emprendimientos asociados</th>
                            <th scope="col">Cantidad de Items en la compra</th>
                            <th scope="col">Fecha de creación</th>
                            <th scope="col">Fecha de envío</th>
                            <th scope="col">Fecha de recibido</th>
                            <th scope="col">Costo total de los items</th>
                            <th scope="col">Costo de envío</th>
                            <th scope="col">Costo total de los compra</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="order in orders" :key="order.orderID">
                            <td>{{ order.orderID }}</td>
                            <td>{{ order.userID }}</td>
                            <td>{{ order.businessName }}</td>
                            <td>{{ order.amount }}</td>
                            <td>{{ formatDate(order.createdDate) }}</td>
                            <td>{{ order.deliveryDate ? formatDate(order.deliveryDate) : '-' }}</td>
                            <td>{{ order.receivedDate ? formatDate(order.receivedDate) : '-' }}</td>
                            <td class="text-end pe-5">₡ {{ formatPrice(order.subtotalCost) }}</td>
                            <td class="text-end pe-5">₡ {{ formatPrice(order.deliveryCost) }}</td>
                            <td class="text-end pe-5">₡ {{ formatPrice(order.totalCost) }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <ActionModalWarning ref="warningPDFModal" @confirmed="generatePDF" />
        </template>
        <template v-else>
            <br />
            <h3 class="text-center mb-4">No ha seleccionado un periodo con órdenes que cumplan el criterio</h3>
        </template>
    </template>

    <template v-else>
        <h1 class="display-4 text-center mb-4"><strong>403 Forbidden</strong></h1>
        <h3 class="text-center mb-4">Página solo para administradores <br /><a href="/">Regresar</a></h3>
    </template>
</template>

<script>
    import AdminNavbar from './AdminNavbar.vue';
    import ActionModalWarning from './ActionModalWarning.vue';
    import { BackendUrl } from '../main.js';
    import axios from "axios";

    export default {
        components: {
            AdminNavbar,
            ActionModalWarning,
        },
        data() {
            return {
                startDate: null,
                endDate: null,
                isValidReport: false,
                currentReportStartDate: null,
                currentReportEndDate: null,
                orders: [
                    {
                        orderID: 0,
                        userID: 0,
                        businessName: '',
                        amount: 0,
                        createdDate: '',
                        deliveryDate: '',
                        receivedDate: '',
                        subtotalCost: 0,
                        deliveryCost: 0,
                        totalCost: 0,
                    },
                ],
            };
        },
        methods: {
            clearEndDate() {
                if (this.startDate) {
                    this.endDate = '';
                }
            },
            generateReport() {
                axios.get(`${BackendUrl}/Reports/CompletedOrders`, {
                    params: {
                        startDate: this.startDate,
                        endDate: this.endDate
                    }
                })
                .then((response) => {
                    this.orders = response.data;
                    if (this.orders[0]) {
                        this.isValidReport = true;
                    } else {
                        this.isValidReport = false;
                    }
                    this.currentReportStartDate = this.startDate;
                    this.currentReportEndDate = this.endDate;
                })
                .catch((error) => {
                    console.log(error);
                });
            },
            openWarningPDFModal() {
                this.$refs.warningPDFModal.openModal("Seguro de que desea generar un report en PDF?");
            },
            generatePDF() {
                axios.get(`${BackendUrl}/Reports/CompletedOrders/pdf`, {
                    params: {
                        startDate: this.currentReportStartDate,
                        endDate: this.currentReportEndDate
                    },
                    responseType: 'blob'
                })
                .then((response) => {
                    const url = window.URL.createObjectURL(new Blob([response.data], { type: 'application/pdf' }));
                    const link = document.createElement('a');
                    link.href = url;
                    link.download = `AllCompletedOrdersReport(${new Date().toLocaleDateString()}).pdf`;

                    document.body.appendChild(link);
                    link.click();
                    window.URL.revokeObjectURL(url);
                    document.body.removeChild(link);
                })
                .catch((error) => {
                    console.log(error);
                });
            },
            formatDate(dateString) {
                const date = new Date(dateString);
                const year = date.getFullYear();
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const day = String(date.getDate()).padStart(2, '0');
                return `${day}/${month}/${year}`;
            },
            formatPrice(price) {
                return new Intl.NumberFormat("en-US").format(price);
            },
        },
        props: {
            isAdmin: {
                type: Boolean,
                required: true,
            },
            isClient: {
                type: Boolean,
                required: true,
            },
        },
    }
</script>

<style></style>