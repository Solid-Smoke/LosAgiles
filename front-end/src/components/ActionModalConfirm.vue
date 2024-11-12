<template>
    <b-modal v-model="actionModalConfirm"
             centered hide-footer
             title=""
             content-class="modal-content-warning">
        <div class="modal-title">
            Información
        </div>
        <div class="modal-body">
            <p>{{ message }}</p>
        </div>
        <div class="modal-buttons-warning">
            <button @click="confirmAction" class="modal-button-confirm">Ok</button>
        </div>
    </b-modal>
</template>

<script>
export default {
    data() {
        return {
            actionModalConfirm: false,
            message: ''
        };
    },
    methods: {
        openModal(message) {
            this.message = message;
            this.actionModalConfirm = true;
        },
        confirmAction() {
            this.$emit('confirmed');
            this.actionModalConfirm = false;
        },
    }
};
</script>

<style scoped>
    @import '../styles/GeneralStyle.css';
    @import '../styles/ModalStyle.css';
</style>
