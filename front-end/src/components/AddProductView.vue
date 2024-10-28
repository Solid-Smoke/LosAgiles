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
                <label for="category" class="form-label">Categoría</label>
                <select v-model="formData.category" class="form-control" id="category" required>
                    <option value="">Seleccione una categoría</option>
                    <option value="Hogar">Hogar</option>
                    <option value="Electronicos">Electrónicos</option>
                    <option value="Frutas">Frutas</option>
                    <option value="Prendas">Prendas</option>
                </select>
            </div>

            <div class="form-group">
                <label for="price" class="form-label">Precio por unidad</label>
                <input v-model="formData.price" type="number" class="form-control" id="price" min="0" max="5000000" required>
                <small class="text-muted">El precio debe ser mayor a cero y no puede exceder 5 millones.</small>
            </div>

            <div class="form-group">
                <label for="stock" class="form-label">Cantidad</label>
                <input v-model="formData.stock" type="number" class="form-control" id="stock" min="1" max="100" required>
                <small class="text-muted">Cantidad entre 1 y 100.</small>
            </div>

            <div class="form-group">
                <label for="weight" class="form-label">Peso (En kilogramos, Ejemplo: 0.300 para 300 gramos)</label>
                <input v-model="formData.weight" type="number" class="form-control" id="weight" min="0" max="500" step="0.001" required>
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
                    <label for="dailyAmount" class="form-label">Cantidad diaria</label>
                    <input v-model="formData.dailyAmount" type="number" class="form-control" id="dailyAmount" min="1" max="100" required>
                    <small class="text-muted">Cantidad diaria entre 1 y 100.</small>
                </div>

                <div class="form-group">
                    <label for="daysAvailable" class="form-label">Días Disponibles (Formato: L, K, M, J, V, S, D sin comas ni espacios)</label>
                    <input v-model="formData.daysAvailable" type="text" class="form-control" id="daysAvailable" required pattern="[LKMJVSD]{1,7}">
                    <small class="text-muted">Ejemplo: LJ para Lunes y Jueves</small>
                </div>
            </div>

            <div class="form-group">
                <label for="image" class="form-label">Imagen del Producto (Formato PNG, Máximo 2 MB)</label><br>
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
                category: "",
                price: "",
                productImage: null,
                stock: 0,
                weight: 0.0,
                isPerishable: false,
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

            if (file) {
                if (file.type !== 'image/png') {
                    alert("El archivo debe ser una imagen PNG.");
                    event.target.value = null;
                    return;
                }

                const maxSize = 2 * 1024 * 1024;
                if (file.size > maxSize) {
                    alert("La imagen no debe exceder los 2 MB.");
                    event.target.value = null;
                    return;
                }

                this.formData.productImage = file;
            }
        },
        validateDaysAvailable() {
            const days = this.formData.daysAvailable.split('');
            const uniqueDays = new Set(days);
            return days.length === uniqueDays.size;
        },
        saveProductDetails() {
            if (!this.validateDaysAvailable()) {
                return;
            }

            const formData = new FormData();
            formData.append("name", this.formData.name);
            formData.append("description", this.formData.description);
            formData.append("category", this.formData.category);
            formData.append("price", this.formData.price);
            formData.append("stock", this.formData.stock);
            formData.append("weight", this.formData.weight);
            formData.append("isPerishable", this.formData.isPerishable === 'true');

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
            }).catch(error => {
                console.error("Error guardando el producto:", error);
            });

            this.closeModal();
        },
        resetFormFields() {
            this.formData.name = "";
            this.formData.description = "";
            this.formData.category = "";
            this.productImage = null;
            this.formData.stock = 0;
            this.formData.weight = 0.0;
            this.formData.isPerishable = false;
            this.formData.dailyAmount = 0;
            this.formData.daysAvailable = "";
        },
        openModal(businessId) {
            this.AddProductModal = true;
            this.resetFormFields();
            this.formData.businessId = businessId;
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
