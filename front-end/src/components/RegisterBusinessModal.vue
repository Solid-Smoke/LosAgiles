<template>
    <b-modal 
             v-model="RegisterBusinessModal" 
             centered scrollable hide-footer
             title="Registrar Empresa">
        <form @submit.prevent="saveBusinessData">
            <div class="form-group">
                <label for="BusinessName">Nombre de la Empresa:</label>
                <input v-model="BusinessData.BusinessName"
                       type="text"
                       id="BusinessName"
                       class="form-control"
                       placeholder="Nombre"
                       required />
            </div>
            <div class="form-group">
                <label for="ID_Type">Tipo de Cedula:</label>
                <select v-model="BusinessData.ID_Type"
                        id="ID_Type"
                        class="form-control"
                        placeholder="ID"
                        required>
                    <option value="" disabled>
                        Seleccione un tipo
                    </option>
                    <option>Fisica</option>
                    <option>Juridica</option>
                </select>
            </div>
            <div class="form-group">
                <label for="BusinessID">Cedula:</label>
                <input v-model="BusinessData.BusinessID"
                       type="number"
                       id="BusinessID"
                       class="form-control"
                       placeholder="Cedula"
                       maxlength="10"
                       pattern="\d{10}"
                       required />
            </div>
            <div class="form-group">
                <label for="BusinessEmail">Correo de la Empresa:</label>
                <input v-model="BusinessData.BusinessEmail"
                       type="email"
                       id="BusinessEmail"
                       class="form-control"
                       placeholder="Correo"
                       required />
            </div>
            <div class="form-group">
                <label for="BusinessPhone">Telefono de la Empresa:</label>
                <input v-model="BusinessData.BusinessPhone"
                       type="number"
                       id="BusinessPhone"
                       class="form-control"
                       placeholder="Telefono"
                       required />
            </div>
            <div class="form-group">
                <label for="Licenses">Permisos:</label>
                <textarea v-model="BusinessData.Licenses"
                          type="text"
                          id="Licenses"
                          class="form-control" />
            </div>
            <div class="form-group">
                <button type="button" class="btn btn-info btn-block">
                    Ingresar Direccion
                </button>
            </div>
            <div>
                <button 
                    type="submit" 
                    class="btn btn-success btn-block">
                    Guardar
                </button>
            </div>
        </form>
    </b-modal>
</template>

<script>
    export default {
        data() {
            return {
                RegisterBusinessModal:false,
                BusinessData: {
                    BusinessName: "", ID_Type: "", BusinessID: "",
                    BusinessEmail: "", BusinessPhone: "",
                    Licenses: ""
                },
            };
        },
        methods: {
            saveBusinessData() {
                console.log("Datos a guardar:", this.BusinessData);
                this.closeModal();
            },
            resetFormFields() {
                this.BusinessData.BusinessName = "";
                this.BusinessData.ID_Type = "";
                this.BusinessData.BusinessID = "";
                this.BusinessData.BusinessEmail = "";
                this.BusinessData.BusinessPhone = "";
                this.BusinessData.Licenses = "";
            },
            openModal() {
                this.RegisterBusinessModal = true; 
                this.resetFormFields();
            },
            closeModal() {
                this.RegisterBusinessModal = false;
            },
        },
    }
</script>

<style>
    .form-group {
        margin-bottom: 20px;
    }
</style>