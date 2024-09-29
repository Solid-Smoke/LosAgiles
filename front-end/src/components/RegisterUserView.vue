<template>
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card p-4 shadow" style="max-width: 600px; width: 100%">
            <h3 class="text-center">Registro de usuario</h3>
            <form @submit.prevent="saveUserDetails">
                <div class="form-group">
                    <label for="Name">Nombre:</label>
                    <input v-model="formData.Name"
                           type="text"
                           id="Name"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="lastNames">Apellido(s):</label>
                    <input v-model="formData.lastNames"
                           type="text"
                           id="lastNames"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="BirthDate">Fecha de nacimiento:</label>
                    <input v-model="formData.BirthDate"
                           type="date"
                           id="BirthDate"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="Email">Correo electr&oacute;nico:</label>
                    <input v-model="formData.Email"
                           type="email"
                           id="Email"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="userName">Nombre de usuario:</label>
                    <input v-model="formData.userName"
                           type="text"
                           id="userName"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="UserPassword">Contrase&ntilde;a:</label>
                    <input v-model="formData.UserPassword"
                           type="password"
                           id="UserPassword"
                           class="form-control"
                           @input="validatePasswords"
                           required />
                </div>
                <div class="form-group">
                    <label for="passwordConfirm">Repetir contrase&ntilde;a:</label>
                    <input v-model="formData.passwordConfirm"
                           type="password"
                           id="passwordConfirm"
                           class="form-control"
                           @input="validatePasswords"
                           ref="confirmInput"
                           required />
                </div>
                <div class="form-group">
                    <button type="button" class="btn btn-info btn-block"
                            @click="ingresarDireccion">
                        Ingresar direcci&oacute;n de entrega
                    </button>
                </div>
                <div>
                    <button type="submit" class="btn btn-success btn-block">
                        Registrarse
                    </button>
                    <a href="/" class="mx-2">
                        <button type="button" class="btn btn-secondary">
                            Regresar
                        </button>
                    </a>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
import axios from "axios";
import { BackendAPIAddress } from "@/main";

    export default {
        data() {
            return {
                formData: {
                    userName: "", Email: "", Name: "", lastNames: "",
                   BirthDate: "", UserPassword: "", passwordConfirm: "",
                },
            };
        },
        methods: {
            validatePasswords() {
                const confirmInput = this.$refs.confirmInput;
                if (this.formData.passwordConfirm === this.formData.UserPassword) {
                    confirmInput.setCustomValidity('');
                } else {
                    confirmInput.setCustomValidity('No coincide con el espacio anterior');
                }
            },
            saveUserDetails() {
                console.log("Datos a guardar:", this.formData);
                axios
                    .post(BackendAPIAddress + "/storeUsers", {
                        UserID: 0,
                        userName: this.formData.userName,
                        Email: this.formData.Email,
                        Name: this.formData.Name,
                        lastNames: this.formData.lastNames,
                        BirthDate: this.formData.BirthDate,
                        UserPassword: this.formData.UserPassword
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

<style>
    .form-group {
        margin-bottom: 20px;
    }
</style>