<template>
    <div>
        <b-navbar toggleable="lg" type="dark" variant="info">
            <b-navbar-brand href="/">Los Agiles</b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav>
                    <b-nav-form>
                        <b-form-input size="sm" placeholder="Search"></b-form-input>
                        <b-button size="sm" class="my-2 my-sm-0" type="submit">Search</b-button>
                    </b-nav-form>

                    <b-nav-item-dropdown text="Mi perfil" right>
                        <b-dropdown-item href="#">Mis datos</b-dropdown-item>
                        <b-dropdown-item href="/direcciones">Direcciones de entrega</b-dropdown-item>
                    </b-nav-item-dropdown>

                    <b-nav-item-dropdown text="Mis empresas">
                        <b-dropdown-item @click="openRegisterBusinessModal">Registrar Empresa</b-dropdown-item>
                        <b-dropdown-item href="/MyBusiness">Ver empresa</b-dropdown-item>
                        <b-dropdown-item @click="openProductModal">Añadir Producto</b-dropdown-item>
                    </b-nav-item-dropdown>

                    <b-nav-item-dropdown text="Admin">
                        <b-dropdown-item href="/AdminViewAllBusiness">Ver todas las empresas registradas</b-dropdown-item>
                    </b-nav-item-dropdown>
                </b-navbar-nav>

                <b-navbar-nav class="ms-auto">
                    <b-nav-item><a href="/login" class="discreteLink">Iniciar sesi&oacute;n</a></b-nav-item>
                </b-navbar-nav>

                <b-navbar-nav>
                    <b-nav-item><a href="/registro" class="discreteLink">Registrarse</a></b-nav-item>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
    <RegisterBusinessModal ref="registerBusinessModal" />
    <AddProductView ref="addProductModal" />
</template>

<script>
    import RegisterBusinessModal from './RegisterBusinessModal.vue';
    import AddProductView from './AddProductView.vue';

    export default {
        components: {
            RegisterBusinessModal,
            AddProductView
        },
        data() {
            return {
                registerBusinessModal: false,
                addProductModal: false,
                loginData: {
                    userID: "1",
                }
            }
        },
        methods: {
            openRegisterBusinessModal() {
                this.$refs.registerBusinessModal.openModal(this.loginData.userID);        
            },
            openProductModal() {
                this.$refs.addProductModal.openModal();
            }
        },
    };
</script>

<style>
    .discreteLink {
        color: black;
        text-decoration: none;
    }

    .discreteLink:hover {
        color: black;
        text-decoration: none;
    }
</style>