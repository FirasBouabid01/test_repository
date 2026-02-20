<template>
  <v-app>
    <Sidebar v-if="auth.isAuthenticated" />

    <v-app-bar color="primary" density="compact" elevation="2">
      <template v-slot:prepend>
        <v-icon color="white" class="ml-2">mdi-application</v-icon>
      </template>
      <v-app-bar-title class="text-white font-weight-bold" style="cursor: pointer" @click="router.push('/')">
        My App
      </v-app-bar-title>
      <v-spacer></v-spacer>
      
      <div v-if="!auth.isAuthenticated" class="mr-4">
        <v-btn color="white" to="/login" prepend-icon="mdi-login">Login</v-btn>
        <v-btn color="white" variant="outlined" to="/register" class="ml-2">Register</v-btn>
      </div>
    </v-app-bar>

    <v-main>
      <v-container fluid>
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { auth } from './store/auth'
import Sidebar from './components/Sidebar.vue'

const router = useRouter()

const logout = () => {
  auth.logout()
  router.push('/')
}
</script>

<style scoped>
</style>
