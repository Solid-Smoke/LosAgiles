<template>
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card p-4 shadow" style="max-width: 400px; width: 100%">
            <h3 class="text-center">Agregar dirección</h3>
            <form @submit.prevent="guardarPais">
                <div class="form-group">
                    <label for="postalCode">Código postal</label>
                    <input v-model="formData.postalCode" type="text" id="name" class="form-control" required />
                </div>
                <div class="form-group">
                    <label for="province">Provincia</label>
                    <select v-model="formData.province" @change='provinceIndex = $event.target.selectedIndex' id="province" required class="form-control" >
                        <option value="" disabled>Seleccione una provincia</option>
                        <option v-for="(province, index) of provinces.provinces" :key="index" >{{ province.nombre }}
                        </option>
                    </select>
                </div>
                <select v-model="formData.canton" id="province" required class="form-control">
                    <option value="" disabled>Seleccione un cantón</option>
                    <option>Prueba</option>
                </select>
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
    data() {
        return {
            formData: {
                postalCode: "",
                province: "",
                canton: "",
                district: ""
            },
            provinceIndex: 0,
            cantonIndex: 0,
            provinces,
        };
        
    },
    props: {
        value: {
            type: Boolean,
            default: false,
            busy: true
        }
    },
    methods: {
        guardarPais() {
            console.log("Seleccionada", this.provinceIndex, this.cantonIndex);
            console.log("Datos a guardar:", this.formData);
            axios
                .post("https://localhost:7024/api/Paises", {
                    postalCode: this.formData.postalCode,
                    province: this.formData.province,
                    canton: this.formData.canton,
                })
                .then(function (response) {
                    console.log(response);
                    window.location.href = "/";
                })
                .catch(function (error) {
                    console.log(error);
                });

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
        }
    }
};
</script>

<style scoped></style>
