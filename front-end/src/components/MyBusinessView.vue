<template>
    <MainNavbar />
    <div class="container mt-5">
        <h1 class="display-4 text-center">Ver Mis Empresas</h1>
    </div>
    <div>
        <b-table striped responsive small hover :items="items" :fields="fields"></b-table>
    </div>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    export default {
        components: {
            MainNavbar
        },
        data() {
            return {
                // Note `isActive` is left out and will not appear in the rendered table
                fields: ['Identificacion', 'Nombre','Email', 'Telefono', 'Permisos',
                        'Telefono', 'Provincia', 'Canton', 'Distrito','Extra'],
                items: [
                    {
                        Identificacion: '123456789',
                        Nombre: 'AAAAAAA',
                        Email: 'empresa1@correo.com',
                        Telefono: '+50612345678',
                        Permisos: '-------------',
                        Provincia: 'San Jose',
                        Canton: 'Central',
                        Distrito: 'Carmen',
                    },
                    {
                        Identificacion: '987654321',
                        Nombre: 'BBBBBBBBBBBB',
                        Email: 'empresa2@correo.com',
                        Telefono: '+50687654321',
                        Permisos: '-------------',
                        Provincia: 'Alajuela',
                        Canton: 'Alajuela',
                        Distrito: 'Alajuela'
                    },
                    {
                        Identificacion: '111222333',
                        Nombre: 'CCCCCCCC',
                        Email: 'empresa3@correo.com',
                        Telefono: '+50623456789',
                        Permisos: '-------------',
                        Provincia: 'Cartago',
                        Canton: 'Cartago',
                        Distrito: 'Cartago'
                    }
                ]
            }
        },

        methods: {
            
        },
    };
</script>

<style></style>