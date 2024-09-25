<template>
    <b-modal v-model="modelShowProduct" centered scrollable hide-footer title="Añadir Producto">
        <form @submit.prevent="guardarDatosProducto">
            <div class="form-group">
                <label for="name" class="form-label">Nombre del Producto</label>
                <input v-model="datosFormulario.name" type="text" class="form-control" id="name" required>
            </div>

            <div class="form-group">
                <label for="description" class="form-label">Descripción</label>
                <textarea v-model="datosFormulario.description" class="form-control" id="description" required></textarea>
            </div>

            <div  class="form-group">
                <label for="price" class="form-label">Precio por Unidad</label>
                <input v-model="datosFormulario.price" type="number" class="form-control" id="price" min="0" required>
            </div>

            <div class="form-group">
                <label for="image" class="form-label">Imagen del Producto</label><br>
                <input type="file" class="form-control-file" id="image" required>
            </div><br>

            <button type="submit" class="btn btn-primary">Agregar Producto</button>
        </form>
    </b-modal>
</template>

<script>
    export default {
        data() {
            return {
                datosFormulario: {
                    name: "",
                    description: "",
                    price: ""
                },
            };
        },
        methods: {
            guardarDatosProducto() {
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
            modelShowProduct: {
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