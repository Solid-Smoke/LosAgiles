<template>
    <RegisterBusinessModal ref="RegisterBusinessModal" />
    <div>
        <b-navbar toggleable="lg" type="dark" variant="info">
            <b-navbar-brand href="/">Los Agiles</b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <!-- Right aligned nav items -->
                <b-navbar-nav class="ml-auto">
                    <b-nav-form>
                        <b-form-input size="sm" class="mr-sm-2" placeholder="Search"></b-form-input>
                        <b-button size="sm" class="my-2 my-sm-0" type="submit">Search</b-button>
                    </b-nav-form>

                    <b-nav-item-dropdown text="Mi perfil" right>
                        <b-dropdown-item href="#">Mis datos</b-dropdown-item>
                    </b-nav-item-dropdown>

                    <b-nav-item-dropdown text="Mis empresas" right>
                        <b-dropdown-item @click="OpenRegisterBusinessModal">Registrar Empresa</b-dropdown-item>
                        <b-dropdown-item href="#" disabled>Ver empresa</b-dropdown-item>
                    </b-nav-item-dropdown>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
</template>

<script>
    import RegisterBusinessModal from './RegisterBusinessModal.vue';

    export default {
        components: {
            RegisterBusinessModal
        },
        data() {
            return {
                RegisterBusinessModal: false
            }
        },
        methods: {
            OpenRegisterBusinessModal() {
                this.$refs.RegisterBusinessModal.openModal();
            
            },
        },
    };
</script>

<style></style>