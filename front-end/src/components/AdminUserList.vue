<template>
    <MainNavbar />

    <div class="container mt-5">
        <h1 class="display-4 text-center" style="margin-bottom: 20px;">LISTA DE USUARIOS</h1>

        <table class="table is-bordered is-striped is-narrow is-hoverable is-fullwidth">
            <thead>
                <tr class="text-center">
                    <th style="padding: 15px;">USUARIO</th>
                    <th style="padding: 15px;" >CORREO</th>
                    <th style="padding: 15px;">ROL</th>
                    <th style="padding: 15px;">ESTADO CUENTA</th>
                    <th style="padding: 15px;">ACCIONES</th>
                </tr>
            </thead>

            <tbody>
                <tr v-for = "(user, index) of users" :key = "index" class="text-center">
                    <td style="padding: 15px;">{{ user.name }}</td>
                    <td style="padding: 15px;">{{ user.email }}</td>
                    <td style="padding: 15px;">{{ user.role }}</td>
                    <td style="padding: 15px;">{{ user.state }}</td>
                    <td style="padding: 15px;">
                        <button class="btn btn-success btn-sm" style="margin-right: 20px;">Activar</button>
                        <button class="btn btn-primary btn-sm" style="margin-right: 20px;">Desactivar</button>
                        <button class="btn btn-danger btn-sm">Bloquear</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import MainNavbar from './MainNavbar.vue';
    
    export default {
        components: {
            MainNavbar
        },

        name: "UserList",
        data() {
            return {
                users: [
                    { name: "Maria", email: "abc@gmail.com", role: "Comprador", state: "Activo" },
                    { name: "Jose", email: "def@gmail.com", role: "Emprendedor", state: "Activo" },
                    { name: "Juan", email: "ghi@gmail.com", role: "Comprador", state: "Inactivo" },
                    { name: "Antonio", email: "jkl@gmail.com", role: "Comprador", state: "Bloqueado" },
                    { name: "David", email: "mno@gmail.com", role: "Emprendedor", state: "Activo" }
                ],
            };
        },
    }
</script>

<style lang="scss" scoped>
</style>