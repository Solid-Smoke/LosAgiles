<template>
    <b-modal v-model="actionModalWarning"
             centered hide-footer
             title=""
             content-class="modal-content-warning">
        <div class="modal-title">
            Advertencia
        </div>
        <div class="modal-body">
            <p>{{ message }}</p>
        </div>
        <div class="modal-buttons-warning">
            <button @click="confirmAction" class="modal-button-confirm">Confirmar</button>
            <button @click="cancelAction" class="modal-button-cancel">Cancelar</button>
        </div>
    </b-modal>
</template>

<script>
    export default {
        data() {
            return {
                actionModalWarning: false,
                message: ''
            };
        },
        methods: {
            openModal(message) {
                this.message = message;
                this.actionModalWarning = true;
            },
            confirmAction() {
                this.$emit('confirmed');
                this.actionModalWarning = false;
            },
            cancelAction() {
                this.actionModalWarning = false;
            }
        }
    };
</script>

<style scoped>
    @import '../styles/GeneralStyle.css';
    @import '../styles/ModalStyle.css';
</style>