<template>
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card p-4 shadow" style="max-width: 600px; width: 100%">
            <h3 class="text-center">Inicio de sesi&oacute;n</h3>
            <form @submit.prevent="verifyUser">
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
                           required />
                </div>
                <div>
                    <button type="submit" class="btn btn-success btn-block">
                        Ingresar
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
                    userName: "", UserPassword: ""
                },
            };
        },
        methods: {
            verifyUser() {
                console.log("Datos a utilizar:", this.formData);
            },
        },
    };
</script>

<style>
    .form-group {
        margin-bottom: 20px;
    }
</style>