<template>
    <b-modal v-model="AddProductModal" centered scrollable hide-footer title="Añadir Producto">
        <form @submit.prevent="saveProductDetails">
            <div class="form-group">
                <label for="name" class="form-label">Nombre del Producto</label>
                <input v-model="formData.name" type="text" class="form-control" id="name" required minlength="1" maxlength="50">
                <small class="text-muted">{{ nameRemainingChars }} caracteres restantes</small>
            </div>

            <div class="form-group">
                <label for="description" class="form-label">Descripción</label>
                <textarea v-model="formData.description" class="form-control" id="description" required minlength="1" maxlength="512"></textarea>
                <small class="text-muted">{{ descriptionRemainingChars }} caracteres restantes</small>
            </div>

            <div class="form-group">
                <label for="price" class="form-label">Precio por Unidad</label>
                <input v-model="formData.price" type="number" class="form-control" id="price" min="0" required>
            </div>

            <div class="form-group">
                <label for="stock" class="form-label">Cantidad</label>
                <input v-model="formData.stock" type="number" class="form-control" id="stock" min="0" required>
            </div>

            <div class="form-group">
                <label for="weight" class="form-label">Peso (En kilogramos, Ejemplo: 0.300 para 300 gramos)</label>
                <input v-model="formData.weight" type="number" class="form-control" id="weight" min="0" step="0.001" required>
                <small class="text-muted">Máximo 3 decimales</small>
            </div>

            <div class="form-group">
                <label for="isPerishable" class="form-label">Perecedero</label><br>
                <input style="padding: 5px;" type="radio" id="isPerishable-yes" value="true" v-model="formData.isPerishable">
                <label style="margin-left: 5px;" for="isPerishable-yes">Sí</label><br>
                <input style="padding: 5px;" type="radio" id="isPerishable-no" value="false" v-model="formData.isPerishable">
                <label style="margin-left: 5px;" for="isPerishable-no">No</label>
            </div>

            <div v-if="formData.isPerishable === 'true'">
                <div class="form-group">
                    <label for="dailyAmount" class="form-label">Cantidad por Día</label>
                    <input v-model="formData.dailyAmount" type="number" class="form-control" id="dailyAmount" min="0" required>
                </div>

                <div class="form-group">
                    <label for="daysAvailable" class="form-label">Días Disponibles (Formato: L, K, M, J, V, S, D sin comas ni espacios)</label>
                    <input v-model="formData.daysAvailable" type="text" class="form-control" id="daysAvailable" required pattern="[LKMJVS]{1,7}">
                    <small class="text-muted">Ejemplo: LJ para lunes y jueves</small>
                </div>
            </div>

            <div class="form-group">
                <label for="image" class="form-label">Imagen del Producto (Formato PNG)</label><br>
                <input type="file" class="form-control-file" id="image" @change="onFileChange" accept=".png" required>
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
                isPerishable: false, // Cambiado de perishable a isPerishable
                dailyAmount: 0,
                daysAvailable: "",
                businessId: 1
            },
        };
    },
    computed: {
        nameRemainingChars() {
            return 50 - this.formData.name.length;
        },
        descriptionRemainingChars() {
            return 512 - this.formData.description.length;
        }
    },
    methods: {
        onFileChange(event) {
            const file = event.target.files[0];

            if (file && file.type === 'image/png') {
                this.formData.productImage = file;
            } else {
                alert("El archivo debe ser una imagen PNG.");
                event.target.value = null;
            }
        },
        saveProductDetails() {
            if (this.formData.name.length < 1 || this.formData.name.length > 50) {
                alert("El nombre debe tener entre 1 y 50 caracteres.");
                return;
            }
            if (this.formData.description.length < 1 || this.formData.description.length > 512) {
                alert("La descripción debe tener entre 1 y 512 caracteres.");
                return;
            }

            const formData = new FormData();
            formData.append("name", this.formData.name);
            formData.append("description", this.formData.description);
            formData.append("price", this.formData.price);
            formData.append("stock", this.formData.stock);
            formData.append("weight", this.formData.weight);
            formData.append("isPerishable", this.formData.isPerishable === 'true'); // Actualizado

            if (this.formData.isPerishable === 'true') {
                formData.append("dailyAmount", this.formData.dailyAmount);
                formData.append("daysAvailable", this.formData.daysAvailable);
            } else {
                formData.append("daysAvailable", null);
            }

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
            this.formData.name = "";
            this.formData.description = "";
            this.productImage = null;
            this.formData.stock = 0;
            this.formData.weight = 0.0;
            this.formData.isPerishable = false;
            this.formData.dailyAmount = 0;
            this.formData.daysAvailable = "";
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
