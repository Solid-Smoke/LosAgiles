<template>
    <b-modal 
             v-model="showModal" 
             centered scrollable hide-footer
             title="Registrar Empresa">
            <h3 class="text-center">Agregar dirección</h3>
            <form @submit.prevent="sendFormData">
                <div class="form-group">
                    <label for="province">Provincia</label>
                    <select v-model="formData.province" @change='provinceIndex = $event.target.selectedIndex.toString()'
                        id="province" required class="form-control">
                        <option value="" disabled>Seleccione una provincia</option>
                        <option v-for="(province, index) of provinces.provinces" :key="index">{{ province.name }}
                        </option>
                    </select>
                </div>
                <div class="form-group">
                    <label for="canton">Cantón</label>
                    <select v-model="formData.canton" @change='cantonIndex = $event.target.selectedIndex.toString()' id="canton"
                        required class="form-control">
                        <option value="" disabled>Seleccione un cantón</option>
                        <option v-for='(canton, index) of provinces.provinces[provinceIndex].cantons' :key="index">{{ canton.name }}</option>
                    </select>
                </div>
                <div class="form-group">
                    <label for="district">Distrito</label>
                    <select v-model="formData.district" id="district" required class="form-control">
                        <option value="" disabled>Seleccione un cantón</option>
                        <option v-for='(district, index) of provinces.provinces[provinceIndex].cantons[cantonIndex].districts'
                            :key="index">{{ district }}</option>
                    </select>
                </div>
                <div class="form-group">
                    <label for="postalCode">Código postal</label>
                    <input v-model="formData.postalCode" type="number" min="10101" max="70605" id="postal-code" class="form-control" required />
                </div>
                <div class="form-group">
                    <label for="other-signs">Otras señas</label>
                    <textarea v-model="formData.otherSigns" id="other-signs" class="form-control" required></textarea>
                    <label for="other-signs" style="text-align: right;"> {{ otherSignsCharCount }} /500</label>
                    <label v-if="otherSignsCharCount > 500" for="other-signs" style="text-align: center;">
                        Este campo de texto no debe exceder los 500 caracteres
                    </label>
                </div>
                <div>
                    <button type="submit" class="btn btn-success btn-block">
                        Agregar
                    </button>
                </div>
            </form>
    </b-modal>
</template>

<script>
import axios from "axios";
import provinces from "../assets/provinces_data.json";
import { BackendAPIAddress } from "@/main";
export default {
    props: {
        parentRoute: String,
        userId: Number,
        isBusiness: Boolean,
    },
    data() {
        return {
            showModal: false,
            formData: {
                province: "",
                canton: "",
                district: "",
                postalCode: "",
                otherSigns: "",
            },
            // This 2 values will change, just setted initial value.
            provinceIndex: "1",
            cantonIndex: "1",
            provinces,
        };

    },
    methods: {
        validateFormData() {
            return !(this.formData.otherSigns.length > 500);
        },
        sendFormData() {
            if (this.validateFormData()) {
                console.log("Form data to send:\n", this.formData);
                axios
                    .post(BackendAPIAddress + "/storeClientAddress", {
                        userId: this.userId,
                        province: this.formData.province,
                        canton: this.formData.canton,
                        district: this.formData.district,
                        postalCode: this.formData.postalCode,
                        otherSigns: this.formData.otherSigns
                    })
                    .then(() => {
                        window.location.href = this.parentRoute;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
                    this.closeModal();
            } else {
                console.log("Form data not sended, Otras Señas field must not exceed 500 characters");
            }
        },
        openModal() {
            this.showModal = true;
        },
        resetFields() {
            this.formData.province="";
            this.formData.canton="";
            this.formData.district="";
            this.formData.postalCode="";
            this.formData.otherSigns="";
        },
        closeModal() {
            this.resetFields();
            this.showModal = false;
        }
        
    },
    computed: {
        otherSignsCharCount() {
            return this.formData.otherSigns.length;
        },
    },
};
</script>

<style scoped></style>
