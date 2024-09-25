<template>
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card p-4 shadow" style="max-width: 400px; width: 100%">
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
                <!-- Cantons will be displayed according to the province selected or San José cantons by default-->
                <div class="form-group">
                    <label for="canton">Cantón</label>
                    <select v-model="formData.canton" @change='cantonIndex = $event.target.selectedIndex.toString()' id="canton" required class="form-control">
                        <option value="" disabled>Seleccione un cantón</option>
                        <option v-for='(canton, index) of provinces.provinces[provinceIndex].cantons' :key="index">{{ canton.name }}</option>
                    </select>
                </div>
                <!-- District will be displayed according to the canton selected or Carmen districts by default-->
                <div class="form-group">
                    <label for="district">Distrito</label>
                    <select v-model="formData.district" id="district" required class="form-control">
                        <option value="" disabled>Seleccione un cantón</option>
                        <option v-for='(district, index) of provinces.provinces[provinceIndex].cantons[cantonIndex].districts' :key="index">{{ district }}</option>
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
                    <label v-if="otherSignsCharCount > 500" for="other-signs" style="text-align: center;">Este campo de texto no debe exceder los 500 caracteres</label>
                </div>
                <div>
                    <button type="submit" class="btn btn-success btn-block">
                        Agregar
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
import axios from "axios";
import provinces from "../assets/provinces_data.json";
export default {
    //parent_route is what route will be sended the user interface after submitting the form,
    // if no prop is passed, will be "/" by default.
    props: {
        parentRoute: String,
        value: { //This is for if the programmer want to use modal-show with this component
            type: Boolean,
            default: false,
            busy: true
        }
    },
    data() {
        return {
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
            provinces, //data for province, in a .json in assets folder
        };

    },
    methods: {
        sendFormData() {
            if (!(this.formData.otherSigns.length > 500)) {
                console.log("Form data to send:\n", this.formData);
                axios
                    .post("https://localhost:7024/api/ShopController/storeFormData", {
                        province: this.formData.province,
                        canton: this.formData.canton,
                        district: this.formData.district,
                        postalCode: this.formData.postalCode,
                        otherSigns: this.formData.otherSigns
                    })
                    .then(function (response) {
                        console.log(response);
                        window.location.href = this.parent_route;
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            } else {
                console.log("Form data not sended, Otras Señas field must not exceed 500 characters");
            }
        },
    },
    computed: {
        modalShow: {
            get() {
                return this.value;
            },
            set(value) {
                this.$emit('input', value);
            }
        },
        otherSignsCharCount() {
            return this.formData.otherSigns.length;
        }
    }
};
</script>

<style scoped></style>
