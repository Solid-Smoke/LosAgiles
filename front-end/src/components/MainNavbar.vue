<template>
    <div>
        <b-navbar toggleable="lg" type="dark" variant="info">
            <b-navbar-brand href="/">Los Agiles</b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav>
                    <b-nav-item-dropdown text="Mi perfil" right>
                        <b-dropdown-item @click="openPersonalDetailsModal">Mis datos</b-dropdown-item>
                        <b-dropdown-item href="/VerCarrito">Carrito</b-dropdown-item>
                        <b-dropdown-item href="/MisOrdenes">Ordenes</b-dropdown-item>
                        <b-dropdown-item href="/direcciones">Direcciones de entrega</b-dropdown-item>
                        <b-dropdown-divider></b-dropdown-divider>
                        <b-dropdown-item href="/ClientReports/CompletedOrders">Ordenes Completadas</b-dropdown-item>
                    </b-nav-item-dropdown>

                    <b-nav-item-dropdown text="Mis empresas">
                        <b-dropdown-item @click="openRegisterBusinessModal">Registrar Empresa</b-dropdown-item>
                        <b-dropdown-item href="/MyBusiness">Ver empresa</b-dropdown-item>
                    </b-nav-item-dropdown>
                </b-navbar-nav>

                <b-navbar-nav class="ms-auto">
                    <b-nav-item><strong>{{this.userName}}</strong></b-nav-item>
                    <b-nav-item @click="logout">Cerrar sesi&oacute;n</b-nav-item>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
    <RegisterBusinessModal ref="registerBusinessModal" />
    <UserPersonalDetails ref="personalDetailsModal" />
</template>

<script>
    import RegisterBusinessModal from './RegisterBusinessModal.vue';
    import UserPersonalDetails from './UserPersonalDetails.vue';

    export default {
        components: {
            RegisterBusinessModal,
            UserPersonalDetails
        },
        data() {
            return {
                registerBusinessModal: false,
                personalDetailsModal: false,
                loginData: {
                    userID: "0",
                },
                userName: '',
                searchProducts: () => {},
                products: []
            }
        },
        methods: {
            openRegisterBusinessModal() {
                this.$refs.registerBusinessModal.openModal(this.loginData.userID);        
            },
            openPersonalDetailsModal() {
                this.$refs.personalDetailsModal.openModal();
            },
            getUserName() {
                const user = JSON.parse(localStorage.getItem('user'));
                return user[0].userName;
            },
            logout() {
                localStorage.removeItem('user');
                window.location.href = "/";
            },
        },
        created() {
            this.userName = this.getUserName();
        }
    };
</script>

<style>
</style>