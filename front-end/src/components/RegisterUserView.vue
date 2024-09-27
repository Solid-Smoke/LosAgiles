<template>
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card p-4 shadow" style="max-width: 600px; width: 100%">
            <h3 class="text-center">Registro de usuario</h3>
            <form @submit.prevent="saveUserDetails">
                <div class="form-group">
                    <label for="firstName">Nombre:</label>
                    <input v-model="formData.firstName"
                           type="text"
                           id="firstName"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="lastnames">Apellido(s):</label>
                    <input v-model="formData.lastnames"
                           type="text"
                           id="lastnames"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="legalID">Cedula:</label>
                    <input v-model="formData.legalID"
                           type="number"
                           id="legalID"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="birthday">Fecha de nacimiento:</label>
                    <input v-model="formData.birthday"
                           type="date"
                           id="birthday"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="userEmail">Correo electr&oacute;nico:</label>
                    <input v-model="formData.userEmail"
                           type="email"
                           id="userEmail"
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
                    <label for="password">Contrase&ntilde;a:</label>
                    <input v-model="formData.password"
                           type="password"
                           id="password"
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
    export default {
        data() {
            return {
                formData: {
                    userName: "", userEmail: "", firstName: "", legalID: "",
                    lastnames: "", birthday: "", password: "", passwordConfirm: "",
                },
            };
        },
        methods: {
            validatePasswords() {
                const confirmInput = this.$refs.confirmInput;
                if (this.formData.passwordConfirm === this.formData.password) {
                    confirmInput.setCustomValidity('');
                } else {
                    confirmInput.setCustomValidity('No coincide con el espacio anterior');
                }
            },
            saveUserDetails() {
                console.log("Datos a guardar:", this.formData);
            },
        },
    };
</script>

<style>
    .form-group {
        margin-bottom: 20px;
    }
</style>