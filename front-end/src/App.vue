<template>
    <div>
        <b-navbar toggleable="lg" type="dark" variant="info">
            <b-navbar-brand href="/">Los Ágiles</b-navbar-brand>

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
                        <b-dropdown-item @click="openModal">Registrar Empresa</b-dropdown-item>
                        <b-dropdown-item href="#" disabled>Ver empresa</b-dropdown-item>
                    </b-nav-item-dropdown>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
    <RegisterBusinessView v-model="modalShow"/>
    <router-view></router-view>
</template>


<script>
    import RegisterBusinessView from './components/RegisterBusinessView.vue';

    export default {
        components: {
            RegisterBusinessView
        },
        data() {
            return {
                modalShow: false
            }
        },
        methods: {
            openModal() {
                this.modalShow = true;
            }
        }
    };
</script>

<style>
</style>