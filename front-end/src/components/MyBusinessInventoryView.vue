<template>
    <MainNavbar />
    <h1 class="display-4 text-center mb-4"><strong>Productos</strong></h1>
    <div class="table-responsive-sm">
        <table class="table table-striped table-hover">
            <thead>
                <tr>
                    <th scope="col">ID del Producto</th>
                    <th scope="col">Nombre</th>
                    <th scope="col">Descripción</th>
                    <th scope="col">Precio</th>
                    <th scope="col">Stock</th>
                    <th scope="col">Perecedero</th>
                    <th scope="col">Ver Detalles</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="product in products" :key="product.productID">
                    <td>{{ product.productID }}</td>
                    <td>{{ product.name }}</td>
                    <td>{{ product.description }}</td>
                    <td>{{ product.price }}</td>
                    <td>{{ product.stock }}</td>
                    <td>{{ product.perishable ? 'Sí' : 'No' }}</td>
                    <td>
                        <button class="btn btn-info">Accion 1</button>
                        <button class="btn btn-info">Accion 2</button>
                        <button class="btn btn-info">Accion 3</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    import axios from "axios";

    export default {
        components: {
            MainNavbar
        },
        data() {
            return {
                currentBusiness: {
                    businessID: 0,
                    name: '',
                    idNumber: '',
                    email: '',
                    telephone: '',
                    permissions: '',
                },
                products: [
                    {
                        "productID": 0,
                        "name": "",
                        "description": "",
                        "price": 0,
                        "stock": 0,
                        "weight": 0.0,
                        "perishable": false,
                        "dailyAmount": 0,
                        "daysAvailable": "",
                        "businessID": 0,
                        "productImage": null
                    }
                ],

            };
        },
        methods: {
            getBusinessInventory() {
                axios.get("https://localhost:7168/api/ProductsByBusinessID", {
                    params: { BusinessID: this.currentBusiness.businessID },
                }).then(
                    (response) => {
                        this.products = response.data;
                        console.log(this.products);
                    }
                );
            },
        },
        created() {
            this.currentBusiness.businessID = this.$route.query.businessID;
            this.currentBusiness.name = this.$route.query.name;
            this.currentBusiness.idNumber = this.$route.query.idNumber;
            this.currentBusiness.email = this.$route.query.email;
            this.currentBusiness.telephone = this.$route.query.telephone;
            this.currentBusiness.permissions = this.$route.query.permissions;
            this.getBusinessInventory();
        }
    };
</script>

<style></style>
