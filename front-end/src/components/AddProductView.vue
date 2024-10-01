<template>
    <b-modal v-model="AddProductModal" centered scrollable hide-footer title="Añadir Producto">
        <form @submit.prevent="saveProductDetails">
            <div class="form-group">
                <label for="name" class="form-label">Nombre del Producto</label>
                <input v-model="formData.name" type="text" class="form-control" id="name" required>
            </div>

            <div class="form-group">
                <label for="description" class="form-label">Descripción</label>
                <textarea v-model="formData.description" class="form-control" id="description" required></textarea>
            </div>

            <div  class="form-group">
                <label for="price" class="form-label">Precio por Unidad</label>
                <input v-model="formData.price" type="number" class="form-control" id="price" min="0" required>
            </div>

            <div class="form-group">
                <label for="image" class="form-label">Imagen del Producto</label><br>
                <input type="file" class="form-control-file" id="image" @change="onFileChange" required>
            </div><br>

            <button type="submit" class="btn btn-success btn-block">Añadir Producto</button>
        </form>
    </b-modal>
</template>

<script>
    export default {
        data() {
            return {
                AddProductModal: false,
                formData: {
                    name: "",
                    description: "",
                    price: "",
                    productImage: null
                },
            };
        },
        methods: {
            onFileChange(event){
                const file = event.target.files[0];
                this.formData.productImage = file;
            },
            saveProductDetails() {
                console.log("Datos a guardar: ", this.formData);
                this.closeModal();
            },
            resetFormFields() {
                this.formData.name = "",
                this.formData.description = "",
                this.formData.price = ""
            },
            openModal() {
                this.AddProductModal = true; 
                this.resetFormFields();
            },
            closeModal() {
                this.AddProductModal = false;
            }
        },
    }
</script>

<style>
    .form-group {
        margin-bottom: 20px; /* Ajusta este valor según sea necesario */
    }
</style>