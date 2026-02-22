<template>
  <v-app>
    <v-app-bar color="primary" density="compact" elevation="2">
      <template v-slot:prepend>
        <v-app-bar-nav-icon @click="sidebar = !sidebar" v-if="auth.isAuthenticated" />
        <v-icon color="white" class="ml-2">mdi-application</v-icon>
      </template>
      <v-app-bar-title class="text-white font-weight-bold" style="cursor: pointer" @click="router.push('/')">
        {{ $t('app.title') }}
      </v-app-bar-title>
      <v-spacer></v-spacer>
      
      <!-- Language Selector -->
      <v-select
        v-model="$i18n.locale"
        :items="languages"
        item-title="label"
        item-value="code"
        style="width: 80px"
        class="mr-2"
        hide-details
        variant="outlined"
      />

      <div v-if="auth.isAuthenticated">
        <v-btn color="white" to="/dashboard" prepend-icon="mdi-view-dashboard">{{ $t('menu.dashboard') }}</v-btn>
        <v-btn color="white" to="/profile" prepend-icon="mdi-account">{{ $t('menu.profile') }}</v-btn>
        <v-btn color="white" @click="logout" prepend-icon="mdi-logout">{{ $t('menu.logout') }}</v-btn>
      </div>
      <div v-else>
        <v-btn color="white" to="/login" prepend-icon="mdi-login">{{ $t('auth.login') }}</v-btn>
        <v-btn color="white" variant="outlined" to="/register" class="ml-2 mr-2">{{ $t('auth.register') }}</v-btn>
      </div>
    </v-app-bar>

    <!-- Navigation Drawer (Side Menu) -->
    <v-navigation-drawer v-if="auth.isAuthenticated" v-model="sidebar" temporary>
      <v-list>
        <v-list-item @click="router.push('/dashboard')" title="{{ $t('menu.dashboard') }}" prepend-icon="mdi-view-dashboard" />
        
        <!-- Admin Only Section -->
        <template v-if="isAdmin">
          <v-divider class="my-2"></v-divider>
          <v-list-subheader>{{ $t('menu.management') }}</v-list-subheader>
          
          <v-list-item @click="router.push('/users')" title="{{ $t('menu.users') }}" prepend-icon="mdi-account-multiple" />
          <v-list-item @click="router.push('/roles')" title="{{ $t('menu.roles') }}" prepend-icon="mdi-shield-account" />
          <v-list-item @click="router.push('/permissions')" title="{{ $t('menu.permissions') }}" prepend-icon="mdi-lock" />
          <v-list-item @click="router.push('/assignments')" title="{{ $t('menu.assignments') }}" prepend-icon="mdi-link" />
        </template>

        <v-divider class="my-2"></v-divider>
        
        <v-list-item @click="router.push('/profile')" title="{{ $t('menu.profile') }}" prepend-icon="mdi-account" />
        <v-list-item @click="logout" title="{{ $t('menu.logout') }}" prepend-icon="mdi-logout" />
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { auth } from './store/auth'
import { getUserRole } from './utils/auth'

const router = useRouter()
const { locale } = useI18n()
const sidebar = ref(false)

const languages = [
  { code: 'en', label: 'English' },
  { code: 'ar', label: 'العربية' },
  { code: 'fr', label: 'Français' }, 
]


const isAdmin = computed(() => {
  const role = getUserRole()
  return role === 'Admin'
})

const logout = () => {
  auth.logout()
  router.push('/')
}
</script>

<style scoped>
</style>
