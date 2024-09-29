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
                <label for="BusinessID">Identificacion (Cedula Fisica o Cedula Juridica):</label>
                <input v-model="BusinessData.BusinessID"
                       type="text"
                       id="BusinessID"
                       class="form-control"
                       placeholder="Cedula"
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
                <label for="Province">Provincia:</label>
                <select v-model="BusinessAddress.Province"
                        id="Province"
                        class="form-control"
                        required>
                    <option value="" disabled>Seleccione una provincia</option>
                    <option value="San José">San José</option>
                    <option value="Alajuela">Alajuela</option>
                    <option value="Cartago">Cartago</option>
                    <option value="Heredia">Heredia</option>
                    <option value="Guanacaste">Guanacaste</option>
                    <option value="Puntarenas">Puntarenas</option>
                    <option value="Limón">Limón</option>
                </select>
            </div>

            <div class="form-group">
                <label for="Canton">Canton:</label>
                <input v-model="BusinessAddress.Canton"
                       type="text"
                       id="Canton"
                       class="form-control"
                       placeholder="Canton"
                       maxlength="40"
                       required />
            </div>

            <div class="form-group">
                <label for="District">Distrito:</label>
                <input v-model="BusinessAddress.District"
                       type="text"
                       id="District"
                       class="form-control"
                       placeholder="Distrito"
                       maxlength="40"
                       required />
            </div>

            <div class="form-group">
                <label for="PostalCode">Codigo Postal:</label>
                <input v-model="BusinessAddress.PostalCode"
                       type="text"
                       id="PostalCode"
                       class="form-control"
                       placeholder="Codigo Postal"
                       maxlength="10"
                       required />
            </div>

            <div class="form-group">
                <label for="OtherSigns">Otras Señales:</label>
                <textarea v-model="BusinessAddress.OtherSigns"
                          id="OtherSigns"
                          class="form-control"
                          maxlength="500">
                </textarea>
            </div>
            <div>
                <button type="submit"
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
                    BusinessName: "", BusinessID: "",
                    BusinessEmail: "", BusinessPhone: "",
                    Licenses: ""
                },
                BusinessAddress: {
                    Province: '',
                    Canton: '',
                    District: '',
                    PostalCode: '',
                    OtherSigns: ''
                },
                loginData: {
                    userID: "",
                }
            };
        },
        methods: {
            saveBusinessData() {
                this.$emit('business-data-saved', this.BusinessData);
                this.closeModal();
            },
            resetFormFields() {
                this.BusinessData.BusinessName = "";
                this.BusinessData.BusinessID = "";
                this.BusinessData.BusinessEmail = "";
                this.BusinessData.BusinessPhone = "";
                this.BusinessData.Licenses = "";

                this.BusinessAddress.Province = "";
                this.BusinessAddress.Canton = "";
                this.BusinessAddress.District = "";
                this.BusinessAddress.PostalCode = "";
                this.BusinessAddress.OtherSigns = "";
            },
            openModal(data) {
                this.loginData = data;
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