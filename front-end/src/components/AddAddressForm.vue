<template>
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card p-4 shadow" style="max-width: 400px; width: 100%">
            <h3 class="text-center">Agregar dirección</h3>
            <form @submit.prevent="guardarPais">
                <div class="form-group">
                    <label for="postalCode">Código postal</label>
                    <input v-model="datosFormulario.postalCode" type="text" id="name" class="form-control" required />
                </div>
                <div class="form-group">
                    <label for="province">Provincia</label>
                    <select v-model="datosFormulario.province" id="province" required class="form-control">
                        <option value="" disabled>Seleccione una provincia</option>
                        <option>San José</option>
                        <option>Heredia</option>
                        <option>Alajuela</option>
                        <option>Cartago</option>
                        <option>Puntarenas</option>
                        <option>Limón</option>
                        <option>Guanacaste</option>
                    </select>
                </div>
                <div class="form-group">
                    <label for="canton">Cantón</label>
                    <input v-model="datosFormulario.canton" type="text" id="canton" class="form-control" required />
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
export default {
    data() {
        return {
            datosFormulario: {
                postalCode: "", province: "", canton: ""
            },
        };
    },
    methods: {
        guardarPais() {
            console.log("Datos a guardar:", this.datosFormulario);
            axios
                .post("https://localhost:7024/api/Paises", {
                    postalCode: this.datosFormulario.postalCode,
                    province: this.datosFormulario.province,
                    canton: this.datosFormulario.canton,
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
};
</script>

<style scoped></style>
