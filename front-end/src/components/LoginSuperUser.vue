<template>
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card p-4 shadow" style="max-width: 600px; width: 100%">
            <h3 class="text-center">Super Usuarios</h3>
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
import axios from 'axios';
import { BackendAPIAddress } from '@/main';

// Will be called to get the password hash
async function hashString(message) {
    const encoder = new TextEncoder();
    const data = encoder.encode(message);
    
    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    
    const hashArray = Array.from(new Uint8Array(hashBuffer));
    const hashHex = hashArray.map(byte => byte.toString(16).padStart(2, '0')).join('');
    
    return hashHex;
}
    export default {
        data() {
            return {
                formData: {
                    userName: "", UserPassword: ""
                },
                userId: ""
            };
        },
        methods: {
            login() {
                document.cookie = "userId=" + this.userId;
                console.log(document.cookie);
                window.location.href = "/";
            },
            verifyUser() {
                console.log("Datos a utilizar:", this.formData);
                hashString(this.formData.UserPassword).then(hash => {
                    axios
                        .get(BackendAPIAddress +
                            "/authSuperUser/details?userName="
                            + this.formData.userName + "&passwordHash="
                            + hash)
                        .then(
                            (response) => {
                                this.userId = response.data;
                                this.login();
                            })
                        .catch(function (error) {
                            console.log(error);
                        });
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