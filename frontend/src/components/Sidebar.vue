<template>
  <v-navigation-drawer
    v-if="auth.isAuthenticated"
    v-model="drawer"
    app
    permanent
    elevation="4"
    class="sidebar-bg"
  >
    <v-list density="comfortable" nav>
      <v-list-item
        prepend-icon="mdi-home"
        title="Home"
        to="/"
        value="home"
      ></v-list-item>
      
      <v-list-item
        prepend-icon="mdi-view-dashboard"
        title="Dashboard"
        to="/dashboard"
        value="dashboard"
      ></v-list-item>

      <!-- 👑 Admin Only -->
      <v-list-item
        v-if="isAdmin()"
        prepend-icon="mdi-shield-account"
        title="Roles"
        to="/roles"
        value="roles"
      ></v-list-item>

      <v-list-item
        v-if="isAdmin()"
        prepend-icon="mdi-key"
        title="Permissions"
        to="/permissions"
        value="permissions"
      ></v-list-item>

      <v-list-item
        prepend-icon="mdi-account"
        title="Profile"
        to="/profile"
        value="profile"
      ></v-list-item>
    </v-list>

    <template v-slot:append>
      <div class="pa-2">
        <v-btn block color="error" prepend-icon="mdi-logout" @click="handleLogout">
          Logout
        </v-btn>
      </div>
    </template>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { auth } from '../store/auth'
import { isAdmin } from '../utils/auth'
import { useRouter } from 'vue-router'

const drawer = ref(true)
const router = useRouter()

const handleLogout = () => {
  auth.logout()
  router.push('/login')
}
</script>

<style scoped>
.sidebar-bg {
  border-right: 1px solid rgba(0, 0, 0, 0.12);
}
</style>
