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
                    <label for="LastNames">Apellido(s):</label>
                    <input v-model="formData.LastNames"
                           type="text"
                           id="LastNames"
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
                    <label for="UserName">Nombre de usuario:</label>
                    <input v-model="formData.UserName"
                           type="text"
                           id="UserName"
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
import { BackendUrl } from '../main.js';
import axios from "axios";

    export default {
        data() {
            return {
                formData: {
                    UserName: "", Email: "", Name: "", LastNames: "",
                    BirthDate: "", UserPassword: "", passwordConfirm: "",
                    AccountState: "", Rol: ""
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
                    .post(`${BackendUrl}/Clients`, {
                        UserID: 0, // Ignored in back-end
                        UserName: this.formData.UserName,
                        Email: this.formData.Email,
                        Name: this.formData.Name,
                        LastNames: this.formData.LastNames,
                        BirthDate: this.formData.BirthDate,
                        UserPassword: this.formData.UserPassword,
                        AccountState: this.formData.AccountState, // Ignored in back-end
                        Rol: this.formData.Rol //Ignored in back-end
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