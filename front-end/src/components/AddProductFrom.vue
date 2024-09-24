<template>
    <h1 class="container mt-5 text-center">AÑADIR PRODUCTO NUEVO</h1>
    <div class="container mt-5 card p-4 shadow" style="max-width: 1000px; width: 100%">
        <form @submit.prevent="submitProduct">
            <div class="forn-group">
                <label for="name" class="form-label">Nombre del Producto</label>
                <input v-model="product.name" type="text" class="form-control" id="name" required>
            </div>

            <div class="forn-group">
                <label for="description" class="form-label">Descripción</label>
                <textarea v-model="product.description" class="form-control" id="description" required></textarea>
            </div>

            <div  class="forn-group">
                <label for="price" class="form-label">Precio por Unidad</label>
                <input v-model="product.price" type="number" class="form-control" id="price" min="0" required>
            </div>

            <div class="forn-group">
                <label for="image" class="form-label">Imagen del Producto</label><br>
                <input type="file" class="form-control-file" id="image" required>
            </div><br>

            <button type="submit" class="btn btn-primary">Agregar Producto</button>
        </form>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                product: {
                    name: "",
                    description: "",
                    price: null,
                    image: ""
                }
            };
        },

        methods: {
        }
    };
</script>

<style lang="scss" scoped>

</style>