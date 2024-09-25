<template>
    <div class="d-flex justify-content-center align-items-center vh-100">
        <div class="card p-4 shadow" style="max-width: 400px; width: 100%">
            <h3 class="text-center">Registro de usuario</h3>
            <form @submit.prevent="guardarDatosUsuario">
                <div class="form-group">
                    <label for="nombre">Nombre:</label>
                    <input v-model="datosFormulario.nombre"
                           type="text"
                           id="nombre"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="apellidos">Apellido(s):</label>
                    <input v-model="datosFormulario.apellidos"
                           type="text"
                           id="apellidos"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="cedula">Cedula:</label>
                    <input v-model="datosFormulario.cedula"
                           type="text"
                           id="cedula"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="fechaNacimiento">Fecha de nacimiento:</label>
                    <input v-model="datosFormulario.fechaNacimiento"
                           type="date"
                           id="fechaNacimiento"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="correo">Correo electronico:</label>
                    <input v-model="datosFormulario.correo"
                           type="email"
                           id="correo"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="nombreUsuario">Nombre de usuario:</label>
                    <input v-model="datosFormulario.nombreUsuario"
                           type="text"
                           id="nameUsuario"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="contrasena">Contrase&ntilde;a:</label>
                    <input v-model="datosFormulario.contrasena"
                           type="password"
                           id="contrasena"
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <label for="contrasena2">Repetir contrase&ntilde;a:</label>
                    <input type="password"
                           id=""
                           class="form-control"
                           required />
                </div>
                <div class="form-group">
                    <button type="button" class="btn btn-info btn-block" @click="ingresarDireccion">
                        Ingresar Direccion
                    </button>
                </div>
                <div>
                    <button type="submit" class="btn btn-success btn-block">
                        Guardar
                    </button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                datosFormulario: {
                    nombreUsuario: "", correo: "", nombre: "",
                    apellidos: "", fechaNacimiento: "", contrasena: ""
                },
            };
        },
        methods: {
            guardarDatosUsuario() {
                console.log("Datos a guardar:", this.datosFormulario);
            },
        },
    };
</script>

<style>
    .form-group {
        margin-bottom: 20px;
    }
</style>