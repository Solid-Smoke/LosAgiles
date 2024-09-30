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

            <div  class="form-group">
                <label for="stock" class="form-label">Cantidad</label>
                <input v-model="formData.stock" type="number" class="form-control" id="stock" min="0" required>
            </div>

            <div  class="form-group">
                <label for="weight" class="form-label">Peso</label>
                <input v-model="formData.weight" type="double" class="form-control" id="weight" min="0.0" required>
            </div>

            <div  class="form-group">
                <label for="perishable" class="form-label">Perecedero</label>
                <input v-model="formData.perishable" type="bool" class="form-control" id="perishable" required>
            </div>

            <div  class="form-group">
                <label for="dailyAmount" class="form-label">Cantidad por Día</label>
                <input v-model="formData.dailyAmount" type="number" class="form-control" id="dailyAmount" min="0" required>
            </div>

            <div  class="form-group">
                <label for="daysAvailable" class="form-label">Días Disponibles</label>
                <input v-model="formData.daysAvailable" type="text" class="form-control" id="daysAvailable" required>
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
import axios from 'axios';

    export default {
        data() {
            return {
                AddProductModal: false,
                formData: {
                    name: "",
                    description: "",
                    price: "",
                    productImage: null,
                    stock: 0,
                    weight: 0.0,
                    perishable: false,
                    dailyAmount: 0,
                    daysAvailable: "",
                    businessId: 1

                },
            };
        },
        methods: {
            onFileChange(event){
                const file = event.target.files[0];
                this.formData.productImage = file;
            },
            saveProductDetails() {
                const formData = new FormData();
                formData.append("name", this.formData.name);
                formData.append("description", this.formData.description);
                formData.append("price", this.formData.price);
                formData.append("stock", this.formData.stock);
                formData.append("weight", this.formData.weight);
                formData.append("perishable", this.formData.perishable);
                formData.append("dailyAmount", this.formData.dailyAmount);
                formData.append("daysAvailable", this.formData.daysAvailable);
                formData.append("businessID", this.formData.businessId);
                formData.append("productImage", this.formData.productImage);

                axios.post('https://localhost:7168/api/Products', formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                }).then(response => {
                    console.log("Producto guardado con éxito:", response.data);
                }).catch(error => {
                    console.error("Error guardando el producto:", error);
                });

                this.closeModal();
            },
            resetFormFields() {
                this.formData.name = "",
                this.formData.description = "",
                this.formData.price = ""
                this.productImage = null,
                this.stock = 0,
                this.weight = 0.0,
                this.perishable = false,
                this.dailyAmount = 0,
                this.daysAvailable = ""
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
        margin-bottom: 20px;
    }
</style>