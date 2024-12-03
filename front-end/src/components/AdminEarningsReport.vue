<template>
    <template v-if="isAdmin">
        <AdminNavbar />
        <h1 class="display-4 text-center mb-4"><strong>Generador de reportes de ganancias</strong></h1>
        <div>
            <div class="text-center">
                <label><strong>Año del reporte:</strong></label>
                <input v-model="year"
                       type="text"
                       name="year-selector"
                       id="year-selector"
                       @input="filterYearInput"
                       @keyup.enter="validateAndGenerateReport" />
                <span class="ms-2" />

                <label><strong>Emprendimiento:</strong></label>
                <select v-model="selectedBusinessId"
                        class="form-select d-inline w-auto">
                    <option :value="null">Todos los emprendimientos</option>
                    <option v-for="business in businesses" :key="business.businessID" :value="business.businessID">
                        {{ business.name }}
                    </option>
                </select>
                <span class="ms-2" />

                <button v-on:click="validateAndGenerateReport" class="btn btn-info" :disabled="!year">
                    Generar reporte
                </button>
                <span class="ms-3" />

                <button v-on:click="openWarningPDFModal" class="btn btn-success" :disabled="!earningsReport.length">
                    Exportar a PDF
                </button>
            </div>
        </div>
        <template v-if="isValidReport">
            <div class="table-responsive-sm">
                <table class="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th scope="col">Nombre del Emprendimiento</th>
                            <th scope="col">Mes</th>
                            <th scope="col">Costo Total de Compras</th>
                            <th scope="col">Costo de Envío</th>
                            <th scope="col">Costo Total</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="record in earningsReport" :key="record.businessName + record.month">
                            <td>{{ record.businessName || 'General' }}</td>
                            <td>{{ record.month }}</td>
                            <td class="text-end pe-5">₡ {{ record.totalPurchase }}</td>
                            <td class="text-end pe-5">₡ {{ record.deliveryCost }}</td>
                            <td class="text-end pe-5">₡ {{ record.totalCost }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <ActionModalWarning ref="warningPDFModal" @confirmed="generatePDF" />
        </template>
        <template v-else>
            <br />
            <h3 class="text-center mb-4">No se encontraron datos para el año especificado</h3>
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
    import axios from 'axios';

    export default {
        components: {
            AdminNavbar,
            ActionModalWarning,
        },
        data() {
            return {
                year: '', 
                selectedBusinessId: null, 
                isValidReport: false,
                earningsReport: [],
                businesses: [], 
            };
        },
        watch: {
            selectedBusinessId() {
                if (this.year && Number(this.year) > 0) {
                    this.validateAndGenerateReport();
                }
            },
        },
        methods: {
            filterYearInput(event) {
                this.year = event.target.value.replace(/[^0-9]/g, ''); 
            },
            validateAndGenerateReport() {
                if (!this.year || Number(this.year) <= 0) {
                    alert('Por favor, ingrese un año válido (mayor a 0).');
                    return;
                }
                this.generateReport();
            },
            generateReport() {
                axios
                    .get(`${BackendUrl}/Reports/Earnings`, {
                        params: {
                            year: this.year,
                            businessId: this.selectedBusinessId,
                        },
                    })
                    .then((response) => {
                        this.earningsReport = response.data;
                        this.isValidReport = true;
                    })
                    .catch((error) => {
                        console.error(error);
                    });
            },
            fetchBusinesses() {
                axios
                    .get(`${BackendUrl}/Business`)
                    .then((response) => {
                        this.businesses = response.data;
                    })
                    .catch((error) => {
                        console.error("Error al cargar los emprendimientos:", error);
                    });
            },
            openWarningPDFModal() {
                this.$refs.warningPDFModal.openModal("¿Seguro de que desea generar un reporte en PDF?");
            },
            generatePDF() {
                axios
                    .get(`${BackendUrl}/Reports/Earnings/pdf`, {
                        params: {
                            year: this.year,
                            businessId: this.selectedBusinessId,
                        },
                        responseType: 'blob',
                    })
                    .then((response) => {
                        const url = window.URL.createObjectURL(new Blob([response.data], { type: 'application/pdf' }));
                        const link = document.createElement('a');
                        link.href = url;
                        link.download = `EarningsReport(${this.year}).pdf`;

                        document.body.appendChild(link);
                        link.click();
                        window.URL.revokeObjectURL(url);
                        document.body.removeChild(link);
                    })
                    .catch((error) => {
                        console.error(error);
                    });
            },
        },
        created() {
            this.fetchBusinesses();
        },
        props: {
            isAdmin: {
                type: Boolean,
                required: true,
            },
        },
    };
</script>
