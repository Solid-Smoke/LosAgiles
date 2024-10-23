<template>
    <b-modal v-model="actionModalError"
             centered hide-footer
             title=""
             content-class="modal-content-warning">
        <div class="modal-title">
            Ha ocurrido un error
        </div>
        <div class="modal-body">
            <p>{{ message }}</p>
            <p v-if="showDetails" @click="toggleDetails" class="toggle-details">
                Ver menos detalles
            </p>
            <p v-else @click="toggleDetails" class="toggle-details">
                Ver mas detalles
            </p>
            <p v-if="showDetails" class="error-details">{{ errorDetails }}</p>
        </div>
        <div class="modal-buttons-warning">
            <button @click="confirmAction" class="modal-button-confirm">Aceptar</button>
        </div>
    </b-modal>
</template>

<script>
    export default {
        data() {
            return {
                actionModalError: false,
                message: '',
                errorDetails: '',
                showDetails: false
            };
        },
        methods: {
            openModal(message, errorDetails = '') {
                this.message = message;
                this.errorDetails = errorDetails;
                this.actionModalError = true;
            },
            toggleDetails() {
                this.showDetails = !this.showDetails;
            },
            confirmAction() {
                this.actionModalError = false;
            },
        },
    };
</script>

<style scoped>
    @import '../styles/GeneralStyle.css';
    @import '../styles/ModalStyle.css';

    .error-details {
        color: #721c24;
        font-size: 0.875rem;
        margin-top: 10px;
    }

    /* Estilo para que 'Ver más/menos detalles' parezca un enlace */
    .toggle-details {
        color: #007bff;
        cursor: pointer;
        text-decoration: underline;
        font-size: 0.875rem;
        margin-top: 10px;
    }

        .toggle-details:hover {
            color: #0056b3;
        }
</style>
