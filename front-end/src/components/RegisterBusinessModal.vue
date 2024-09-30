<template>
    <b-modal 
             v-model="RegisterBusinessModal" 
             centered scrollable hide-footer
             title="Registrar Empresa">
        <form @submit.prevent="saveBusinessData">
            <div class="form-group">
                <label for="BusinessName">Nombre de la Empresa:</label>
                <input v-model="businessData.businessName"
                       type="text"
                       id="businessName"
                       class="form-control"
                       placeholder="Nombre"
                       required />
            </div>
            <div class="form-group">
                <label for="BusinessID">Identificacion (Cedula Fisica o Cedula Juridica):</label>
                <input v-model="businessData.businessID"
                       type="text"
                       id="businessID"
                       class="form-control"
                       placeholder="Cedula"
                       required />
            </div>
            <div class="form-group">
                <label for="BusinessEmail">Correo de la Empresa:</label>
                <input v-model="businessData.businessEmail"
                       type="email"
                       id="businessEmail"
                       class="form-control"
                       placeholder="Correo"
                       required />
            </div>
            <div class="form-group">
                <label for="BusinessPhone">Telefono de la Empresa:</label>
                <input v-model="businessData.businessPhone"
                       type="number"
                       id="businessPhone"
                       class="form-control"
                       placeholder="Telefono"
                       required />
            </div>
            <div class="form-group">
                <label for="Licenses">Permisos:</label>
                <textarea v-model="businessData.licenses"
                          type="text"
                          id="licenses"
                          class="form-control" />
            </div>
            <div class="form-group">
                <label for="Province">Provincia:</label>
                <select v-model="businessData.province"
                        id="province"
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
                <input v-model="businessData.canton"
                       type="text"
                       id="canton"
                       class="form-control"
                       placeholder="Canton"
                       maxlength="40"
                       required />
            </div>

            <div class="form-group">
                <label for="District">Distrito:</label>
                <input v-model="businessData.district"
                       type="text"
                       id="district"
                       class="form-control"
                       placeholder="Distrito"
                       maxlength="40"
                       required />
            </div>

            <div class="form-group">
                <label for="PostalCode">Codigo Postal:</label>
                <input v-model="businessData.postalCode"
                       type="text"
                       id="postalCode"
                       class="form-control"
                       placeholder="Codigo Postal"
                       maxlength="10"
                       required />
            </div>

            <div class="form-group">
                <label for="OtherSigns">Otras Señales:</label>
                <textarea v-model="businessData.otherSigns"
                          id="otherSigns"
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
                businessData: {
                    userID: "", businessName: "", businessID: "",
                    businessEmail: "", businessPhone: "",
                    licenses: "",

                    province: "", canton: "", district: "",
                    postalCode: "", otherSigns: ""
                },
            };
        },
        methods: {
            saveBusinessData() {
                
                this.closeModal();
            },
            resetFormFields() {
                this.businessData.businessName = "";
                this.businessData.businessID = "";
                this.businessData.businessEmail = "";
                this.businessData.businessPhone = "";
                this.businessData.licenses = "";

                this.businessData.province = "";
                this.businessData.canton = "";
                this.businessData.district = "";
                this.businessData.postalCode = "";
                this.businessData.otherSigns = "";
            },
            openModal(data) {
                this.businessData.userID = data;
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