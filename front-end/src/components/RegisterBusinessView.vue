<template>
    <b-modal v-model="modalShow" centered scrollable hide-footer title="Registrar Empresa">
        <form @submit.prevent="guardarDatosEmpresa">
            <div class="form-group">
                <label for="nombre" >Nombre de la Empresa:</label>
                <input v-model="datosFormulario.nombre"
                       type="text"
                       id="nombre"
                       class="form-control"
                       required />
            </div>
            <div class="form-group">
                <label for="tipoCedula">Tipo de Cedula:</label>
                <select v-model="datosFormulario.tipoCedula"
                        id="tipoCedula"
                        required
                        class="form-control">
                    <option value="" disabled>
                        Seleccione un tipo
                    </option>
                    <option>Fisica</option>
                    <option>Juridica</option>
                </select>
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
                <label for="correoEmpresa">Correo de la Empresa:</label>
                <input v-model="datosFormulario.correoEmpresa"
                       type="email"
                       id="correoEmpresa"
                       class="form-control"
                       required />
            </div>
            <div class="form-group">
                <label for="telefonoEmpresa">Telefono de la Empresa:</label>
                <input v-model="datosFormulario.telefonoEmpresa"
                       type="number"
                       id="telefonoEmpresa"
                       class="form-control"
                       required />
            </div>
            <div class="form-group">
                <label for="permisos">Permisos:</label>
                <textarea v-model="datosFormulario.permisos"
                       type="text"
                       id="permisos"
                       class="form-control" />
            </div>
            <div class="form-group">
                <button type="button" class="btn btn-info btn-block" @click="ingresarDireccion">
                    Ingresar Direccion
                </button>
            </div>
            <div>
                <button type="submit" class="btn btn-success
                btn-block">
                    Guardar
                </button>
            </div>
        </form>
    </b-modal>
</template>

<script>
    export default {
        data() {
            return {
                datosFormulario: {
                    nombre: "", tipoCedula: "", cedula: "",
                    correoEmpresa: "", telefonoEmpresa: 0,
                    permisos: ""
                },
            };
        },
        methods: {
            guardarDatosEmpresa() {
                console.log("Datos a guardar:", this.datosFormulario);
            },
        },
        props: {
            value: {
                type: Boolean,
                default: false,
                busy: true
            }
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
    }
</script>

<style>
    .form-group {
        margin-bottom: 20px; /* Ajusta este valor según sea necesario */
    }
</style>