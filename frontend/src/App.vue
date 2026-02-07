<template>
  <v-app>
    <v-app-bar color="primary" density="compact" elevation="2">
      <template v-slot:prepend>
        <v-icon color="white" class="ml-2">mdi-application</v-icon>
      </template>
      <v-app-bar-title class="text-white font-weight-bold" style="cursor: pointer" @click="router.push('/')">
        My App
      </v-app-bar-title>
      <v-spacer></v-spacer>
      
      <div v-if="auth.isAuthenticated">
        <v-btn color="white" to="/dashboard" prepend-icon="mdi-view-dashboard">Dashboard</v-btn>
        <v-btn color="white" to="/profile" prepend-icon="mdi-account">Profile</v-btn>
        <v-btn color="white" @click="logout" prepend-icon="mdi-logout">Logout</v-btn>
      </div>
      <div v-else>
        <v-btn color="white" to="/login" prepend-icon="mdi-login">Login</v-btn>
        <v-btn color="white" variant="outlined" to="/register" class="ml-2 mr-2">Register</v-btn>
      </div>
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { auth } from './store/auth'

const router = useRouter()

const logout = () => {
  auth.logout()
  router.push('/')
}
</script>

<style scoped>
</style>
